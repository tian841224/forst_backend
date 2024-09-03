using admin_backend.Data;
using admin_backend.DTOs.Case;
using admin_backend.DTOs.CaseDiagnosisResult;
using admin_backend.DTOs.CaseHistory;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Transactions;

namespace admin_backend.Services
{
    public class CaseRecordService : ICaseRecordService
    {
        private readonly ILogger<CaseRecordService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IFileService> _fileService;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly ICaseDiagnosisResultService _caseDiagnosisResultService;
        private readonly ICaseHistoryService _caseHistoryService;

        public CaseRecordService(ILogger<CaseRecordService> log, IDbContextFactory<MysqlDbContext> contextFactory, IMapper mapper, Lazy<IFileService> fileService, Lazy<IOperationLogService> operationLogService,
            ICaseDiagnosisResultService caseDiagnosisResultService, ICaseHistoryService caseHistoryService)
        {
            _log = log;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _fileService = fileService;
            _operationLogService = operationLogService;
            _caseDiagnosisResultService = caseDiagnosisResultService;
            _caseHistoryService = caseHistoryService;
        }

        public async Task<CaseResponse> Get(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var caseEntity = await _context.CaseRecord.FirstOrDefaultAsync(x => x.Id == Id);

            if (caseEntity == null)
            {
                throw new ApiException($"找不到此案件資料 - {Id}");
            }

            var fileList = new List<CaseRecordFileDto>();
            if (!string.IsNullOrEmpty(caseEntity.Photo))
            {
                var fileUpload = JsonSerializer.Deserialize<List<CaseRecordFileDto>>(caseEntity.Photo);
                if (fileUpload != null)
                {
                    fileList.AddRange(fileUpload.Select(x => new CaseRecordFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File, "image") }));
                }
            }

            var treeBasicInfoId = await _context.TreeBasicInfo.FirstOrDefaultAsync(x => x.Id == caseEntity.TreeBasicInfoId) ?? new TreeBasicInfo();
            var adminUser = await _context.AdminUser.FirstOrDefaultAsync(x => x.Id == caseEntity.AdminUserId);
            var forestCompartmentLocation = await _context.ForestCompartmentLocation.FirstOrDefaultAsync(x => x.Id == caseEntity.ForestCompartmentLocationId) ?? new ForestCompartmentLocation();
            //案件回覆
            var caseDiagnosisResult = (await _caseDiagnosisResultService.Get(new GetCaseDiagnosisResultDto { CaseNumber = caseEntity.Id })).Items.FirstOrDefault();

            var result = new CaseResponse
            {
                Id = caseEntity.Id,
                CreateTime = caseEntity.CreateTime,
                UpdateTime = caseEntity.UpdateTime,
                CaseNumber = caseEntity.CaseNumber,
                AdminUserId = caseEntity.AdminUserId,
                AdminUserName = adminUser?.Name,
                ApplicantAccount = caseEntity.ApplicantAccount,
                ApplicantName = caseEntity.ApplicantName,
                ApplicationDate = caseEntity.ApplicationDate,
                UnitName = caseEntity.UnitName,
                County = caseEntity.County,
                District = caseEntity.District,
                Address = caseEntity.Address,
                Phone = caseEntity.Phone,
                Fax = caseEntity.Fax,
                Email = caseEntity.Email,
                DamageTreeCounty = caseEntity.DamageTreeCounty,
                DamageTreeDistrict = caseEntity.DamageTreeDistrict,
                DamageTreeAddress = caseEntity.DamageTreeAddress,
                ForestCompartmentLocationId = caseEntity.ForestCompartmentLocationId,
                ForestPostion = forestCompartmentLocation.Postion,
                AffiliatedUnit = forestCompartmentLocation.AffiliatedUnit,
                ForestSection = caseEntity.ForestSection,
                ForestSubsection = caseEntity.ForestSubsection,
                Latitude = caseEntity.Latitude,
                Longitude = caseEntity.Longitude,
                DamagedArea = caseEntity.DamagedArea,
                DamagedCount = caseEntity.DamagedCount,
                PlantedArea = caseEntity.PlantedArea,
                PlantedCount = caseEntity.PlantedCount,
                TreeBasicInfoId = caseEntity.TreeBasicInfoId,
                ScientificName = treeBasicInfoId.ScientificName,
                TreeName = treeBasicInfoId.Name,
                Others = caseEntity.Others,
                DamagedPart = caseEntity.DamagedPart,
                TreeHeight = caseEntity.TreeHeight,
                TreeDiameter = caseEntity.TreeDiameter,
                LocalPlantingTime = caseEntity.LocalPlantingTime,
                FirstDiscoveryDate = caseEntity.FirstDiscoveryDate,
                DamageDescription = caseEntity.DamageDescription,
                LocationType = caseEntity.LocationType,
                BaseCondition = caseEntity.BaseCondition,
                Photo = fileList,
                CaseDiagnosisResultResponse = caseDiagnosisResult,
                CaseStatus = caseEntity.CaseStatus,
            };

            return result;
        }

        public async Task<PagedResult<CaseResponse>> Get(GetCaseRecordDto dto)
        {
            var result = new List<CaseResponse>();
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var query = from caseRecord in _context.CaseRecord
                        join diagnosis in _context.CaseDiagnosisResult
                        on caseRecord.Id equals diagnosis.CaseId into caseDiagnosisGroup
                        from caseDiagnosis in caseDiagnosisGroup.DefaultIfEmpty()
                        select new { caseRecord, caseDiagnosis };

            if (dto.DamageClassId.HasValue)
            {
                var commonIds = await _context.CommonDamage.Where(x => x.DamageClassId == dto.DamageClassId).Select(x => x.Id).ToListAsync();
                query = query.Where(x => commonIds.Contains(x.caseDiagnosis.CommonDamageId ?? 0));
            }

            if (dto.DamageTypeId.HasValue)
            {
                var commonIds = await _context.CommonDamage.Where(x => x.DamageTypeId == dto.DamageTypeId).Select(x => x.Id).ToListAsync();
                query = query.Where(x => commonIds.Contains(x.caseDiagnosis.CommonDamageId ?? 0));
            }

            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                string keyword = dto.Keyword.ToLower();
                query = query.Where(x =>
                    x.caseRecord.Address.ToLower().Contains(keyword) ||
                     x.caseRecord.District.ToLower().Contains(keyword) ||
                    x.caseRecord.County.ToLower().Contains(keyword) ||
                    x.caseRecord.ApplicantName.ToLower().Contains(keyword) ||
                    x.caseRecord.Id.ToString().Contains(keyword)
                );
            }

            if (dto.AdminUserId.HasValue)
            {
                query = query.Where(x => x.caseRecord.AdminUserId == dto.AdminUserId.Value);
            }

            if (dto.CaseNumber.HasValue)
            {
                query = query.Where(x => x.caseRecord.CaseNumber == dto.CaseNumber.Value);
            }

            //案件日期
            if (!string.IsNullOrEmpty(dto.StartTime) && !string.IsNullOrEmpty(dto.EndTime))
            {
                //處理時間格式
                if (!DateTime.TryParse(dto.StartTime, out var StartTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.StartTime));
                }
                if (!DateTime.TryParse(dto.EndTime, out var EndTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.EndTime));
                }
                query = query.Where(x => x.caseRecord.ApplicationDate >= StartTime && x.caseRecord.ApplicationDate < EndTime);
            }

            if (dto.CaseStatus.HasValue)
            {
                query = query.Where(x => x.caseRecord.CaseStatus == dto.CaseStatus.Value);
            }

            foreach (var item in await query.ToListAsync())
            {
                //處理上傳檔案
                var fileList = new List<CaseRecordFileDto>();
                if (!string.IsNullOrEmpty(item.caseRecord.Photo))
                {
                    var fileUpload = JsonSerializer.Deserialize<List<CaseRecordFileDto>>(item.caseRecord.Photo);
                    if (fileUpload != null)
                    {
                        fileList.AddRange(fileUpload.Select(x => new CaseRecordFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File, "image") }));
                    }
                }

                var treeBasicInfoId = await _context.TreeBasicInfo.FirstOrDefaultAsync(x => x.Id == item.caseRecord.TreeBasicInfoId) ?? new TreeBasicInfo();
                var adminUser = await _context.AdminUser.FirstOrDefaultAsync(x => x.Id == item.caseRecord.AdminUserId) ?? new AdminUser();
                var forestCompartmentLocation = await _context.ForestCompartmentLocation.FirstOrDefaultAsync(x => x.Id == item.caseRecord.ForestCompartmentLocationId) ?? new ForestCompartmentLocation();

                //案件回覆
                var getCaseDiagnosisResultDto = new GetCaseDiagnosisResultDto
                {
                    CaseNumber = item.caseRecord.Id
                };

                var caseDiagnosisResult = (await _caseDiagnosisResultService.Get(getCaseDiagnosisResultDto)).Items.FirstOrDefault();

                result.Add(new CaseResponse
                {
                    Id = item.caseRecord.Id,
                    CreateTime = item.caseRecord.CreateTime,
                    UpdateTime = item.caseRecord.UpdateTime,
                    CaseNumber = item.caseRecord.CaseNumber,
                    AdminUserId = item.caseRecord.AdminUserId,
                    AdminUserName = adminUser.Name,
                    ApplicantAccount = item.caseRecord.ApplicantAccount,
                    ApplicantName = item.caseRecord.ApplicantName,
                    ApplicationDate = item.caseRecord.ApplicationDate,
                    UnitName = item.caseRecord.UnitName,
                    County = item.caseRecord.County,
                    District = item.caseRecord.District,
                    Address = item.caseRecord.Address,
                    Phone = item.caseRecord.Phone,
                    Fax = item.caseRecord.Fax,
                    Email = item.caseRecord.Email,
                    DamageTreeCounty = item.caseRecord.DamageTreeCounty,
                    DamageTreeDistrict = item.caseRecord.DamageTreeDistrict,
                    DamageTreeAddress = item.caseRecord.DamageTreeAddress,
                    ForestCompartmentLocationId = item.caseRecord.ForestCompartmentLocationId,
                    ForestPostion = forestCompartmentLocation.Postion,
                    AffiliatedUnit = forestCompartmentLocation.AffiliatedUnit,
                    ForestSection = item.caseRecord.ForestSection,
                    ForestSubsection = item.caseRecord.ForestSubsection,
                    Latitude = item.caseRecord.Latitude,
                    Longitude = item.caseRecord.Longitude,
                    DamagedArea = item.caseRecord.DamagedArea,
                    DamagedCount = item.caseRecord.DamagedCount,
                    PlantedArea = item.caseRecord.PlantedArea,
                    PlantedCount = item.caseRecord.PlantedCount,
                    TreeBasicInfoId = item.caseRecord.TreeBasicInfoId,
                    ScientificName = treeBasicInfoId.ScientificName,
                    TreeName = treeBasicInfoId.Name,
                    Others = item.caseRecord.Others,
                    DamagedPart = item.caseRecord.DamagedPart,
                    TreeHeight = item.caseRecord.TreeHeight,
                    TreeDiameter = item.caseRecord.TreeDiameter,
                    LocalPlantingTime = item.caseRecord.LocalPlantingTime,
                    FirstDiscoveryDate = item.caseRecord.FirstDiscoveryDate,
                    DamageDescription = item.caseRecord.DamageDescription,
                    LocationType = item.caseRecord.LocationType,
                    BaseCondition = item.caseRecord.BaseCondition,
                    Photo = fileList,
                    CaseDiagnosisResultResponse = caseDiagnosisResult,
                    CaseStatus = item.caseRecord.CaseStatus,
                });
            }

            return result.GetPaged(dto.Page!);
        }
        public async Task<CaseResponse> Add(AddCaseRecordDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            //編案件編號
            var maxCaseNumber = await _context.CaseRecord
                .MaxAsync(x => (int?)x.CaseNumber) ?? 0;

            if (maxCaseNumber == 0)
            {
                var today = DateTime.Now;
                string datePrefix = today.ToString("yyyyMMdd");
                maxCaseNumber = int.Parse(datePrefix + "01");
            }
            else maxCaseNumber = maxCaseNumber + 1;

            //處理上傳檔案
            var fileUploadList = new List<CaseRecordFileDto>();
            var id = 0;
            foreach (var file in dto.Photo)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file!.FileName)}";
                var fileUploadDto = await _fileService.Value.UploadFile(fileName, file);
                fileUploadList.Add(new CaseRecordFileDto { Id = ++id, File = fileName });
            }
            var jsonResult = JsonSerializer.Serialize(fileUploadList);

            //處理時間格式
            if (!DateTime.TryParse(dto.ApplicationDate, out var ApplicationDate))
            {
                throw new ArgumentException("Invalid date format", nameof(dto.ApplicationDate));
            }
            if (!DateTime.TryParse(dto.FirstDiscoveryDate, out var FirstDiscoveryDate))
            {
                throw new ArgumentException("Invalid date format", nameof(dto.FirstDiscoveryDate));
            }

            //取得對應林班資料
            var forestCompartmentLocation = await _context.ForestCompartmentLocation.FirstOrDefaultAsync(x => x.AffiliatedUnit == dto.AffiliatedUnit && x.Postion == dto.ForestPostion);
            if (forestCompartmentLocation == null)
                throw new ApiException($"找不到對應的林班資料:{dto.ForestPostion}/{dto.AffiliatedUnit}");

            var caseEntity = new CaseRecord
            {
                CaseNumber = maxCaseNumber,
                ApplicantAccount = dto.ApplicantAccount,
                ApplicantName = dto.ApplicantName,
                ApplicationDate = ApplicationDate,
                UnitName = dto.UnitName,
                County = dto.County,
                District = dto.District,
                Address = dto.Address,
                Phone = dto.Phone,
                Fax = dto.Fax,
                Email = dto.Email,
                DamageTreeCounty = dto.DamageTreeCounty,
                DamageTreeDistrict = dto.DamageTreeDistrict,
                DamageTreeAddress = dto.DamageTreeAddress,
                ForestCompartmentLocationId = forestCompartmentLocation.Id,
                ForestSection = dto.ForestSection,
                ForestSubsection = dto.ForestSubsection,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                DamagedArea = dto.DamagedArea,
                DamagedCount = dto.DamagedCount,
                PlantedArea = dto.PlantedArea,
                PlantedCount = dto.PlantedCount,
                TreeBasicInfoId = dto.TreeBasicInfoId,
                Others = dto.Others,
                DamagedPart = dto.DamagedPart,
                TreeHeight = dto.TreeHeight,
                TreeDiameter = dto.TreeDiameter,
                LocalPlantingTime = dto.LocalPlantingTime,
                FirstDiscoveryDate = FirstDiscoveryDate,
                DamageDescription = dto.DamageDescription,
                LocationType = dto.LocationType,
                BaseCondition = dto.BaseCondition,
                Photo = jsonResult,
                CaseStatus = dto.CaseStatus,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await _context.CaseRecord.AddAsync(caseEntity);


            if (await _context.SaveChangesAsync() > 0)
            {
                // 新增案件歷程
                await _caseHistoryService.Add(new AddCaseHistoryDto
                {
                    CaseId = caseEntity.Id,
                    ActionTime = DateTime.Now,
                    ActionType = ActionTypeEnum.Add,
                });

                // 新增操作紀錄
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增案件 {caseEntity.CaseNumber}",
                });
            }
            scope.Complete();

            return _mapper.Map<CaseResponse>(caseEntity);
        }

        public async Task<CaseResponse> Update(int Id, UpdateCaseRecordDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var caseEntity = await _context.CaseRecord.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (caseEntity == null)
            {
                throw new ApiException($"找不到此案件資料 - {Id}");
            }

            if (!string.IsNullOrEmpty(dto.ApplicationDate))
            {
                if (!DateTime.TryParse(dto.ApplicationDate, out var ApplicationDate))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.ApplicationDate));
                }
                caseEntity.ApplicationDate = ApplicationDate;
            }

            if (!string.IsNullOrEmpty(dto.UnitName))
                caseEntity.UnitName = dto.UnitName;

            if (!string.IsNullOrEmpty(dto.District))
                caseEntity.District = dto.District;

            if (!string.IsNullOrEmpty(dto.County))
                caseEntity.County = dto.County;

            if (!string.IsNullOrEmpty(dto.Address))
                caseEntity.Address = dto.Address;

            if (!string.IsNullOrEmpty(dto.Phone))
                caseEntity.Phone = dto.Phone;

            if (!string.IsNullOrEmpty(dto.Fax))
                caseEntity.Fax = dto.Fax;

            if (!string.IsNullOrEmpty(dto.Email))
                caseEntity.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.DamageTreeCounty))
                caseEntity.DamageTreeCounty = dto.DamageTreeCounty;

            if (!string.IsNullOrEmpty(dto.DamageTreeDistrict))
                caseEntity.DamageTreeDistrict = dto.DamageTreeDistrict;

            if (!string.IsNullOrEmpty(dto.DamageTreeAddress))
                caseEntity.DamageTreeAddress = dto.DamageTreeAddress;

            if (!string.IsNullOrEmpty(dto.AffiliatedUnit) && !string.IsNullOrEmpty(dto.ForestPostion))
            {
                //取得對應林班資料
                var forestCompartmentLocation = await _context.ForestCompartmentLocation.FirstOrDefaultAsync(x => x.AffiliatedUnit == dto.AffiliatedUnit && x.Postion == dto.ForestPostion);
                if (forestCompartmentLocation == null)
                    throw new ApiException($"找不到對應的林班資料:{dto.ForestPostion}/{dto.AffiliatedUnit}");

                caseEntity.ForestCompartmentLocationId = forestCompartmentLocation.Id;
            }
                
            if (!string.IsNullOrEmpty(dto.ForestSection))
                caseEntity.ForestSection = dto.ForestSection;

            if (!string.IsNullOrEmpty(dto.ForestSubsection))
                caseEntity.ForestSubsection = dto.ForestSubsection;

            if (!string.IsNullOrEmpty(dto.Latitude))
                caseEntity.Latitude = dto.Latitude;

            if (!string.IsNullOrEmpty(dto.Longitude))
                caseEntity.Longitude = dto.Longitude;

            if (dto.DamagedArea.HasValue)
                caseEntity.DamagedArea = dto.DamagedArea.Value;

            if (dto.DamagedCount.HasValue)
                caseEntity.DamagedCount = dto.DamagedCount.Value;

            if (dto.PlantedArea.HasValue)
                caseEntity.PlantedArea = dto.PlantedArea.Value;

            if (dto.PlantedCount.HasValue)
                caseEntity.PlantedCount = dto.PlantedCount.Value;

            if (dto.TreeBasicInfoId.HasValue)
                caseEntity.TreeBasicInfoId = dto.TreeBasicInfoId.Value;

            if (!string.IsNullOrEmpty(dto.Others))
                caseEntity.Others = dto.Others;

            if (dto.DamagedPart != null && dto.DamagedPart.Count > 0)
                caseEntity.DamagedPart = dto.DamagedPart;

            if (!string.IsNullOrEmpty(dto.TreeHeight))
                caseEntity.TreeHeight = dto.TreeHeight;

            if (!string.IsNullOrEmpty(dto.TreeDiameter))
                caseEntity.TreeDiameter = dto.TreeDiameter;

            if (!string.IsNullOrEmpty(dto.LocalPlantingTime))
                caseEntity.LocalPlantingTime = dto.LocalPlantingTime;

            if (!string.IsNullOrEmpty(dto.FirstDiscoveryDate))
            {
                if (!DateTime.TryParse(dto.FirstDiscoveryDate, out var FirstDiscoveryDate))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.FirstDiscoveryDate));
                }
                caseEntity.FirstDiscoveryDate = FirstDiscoveryDate;
            }

            if (!string.IsNullOrEmpty(dto.DamageDescription))
                caseEntity.DamageDescription = dto.DamageDescription;

            if (dto.LocationType != null && dto.LocationType.Count > 0)
                caseEntity.LocationType = dto.LocationType;

            if (dto.BaseCondition != null && dto.BaseCondition.Count > 0)
                caseEntity.BaseCondition = dto.BaseCondition;

            if (dto.CaseStatus.HasValue)
                caseEntity.CaseStatus = dto.CaseStatus.Value;

            // 更新資料
            if (dto.AdminUserId.HasValue)
            {
                caseEntity.CaseStatus = CaseStatusEnum.Pending_Review;
                caseEntity.AdminUserId = dto.AdminUserId.Value;
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CaseRecord.Update(caseEntity);

            if (await _context.SaveChangesAsync() > 0)
            {
                if (dto.AdminUserId.HasValue)
                {
                    // 新增案件歷程
                    await _caseHistoryService.Add(new AddCaseHistoryDto
                    {
                        CaseId = caseEntity.Id,
                        ActionTime = DateTime.Now,
                        ActionType = ActionTypeEnum.Assign,
                    });
                }

                // 新增操作紀錄
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改案件 {caseEntity.Id}",
                });
            }
            scope.Complete();

            return _mapper.Map<CaseResponse>(caseEntity);
        }

        public async Task<CaseResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var caseEntity = await _context.CaseRecord.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (caseEntity == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CaseRecord.Remove(caseEntity);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除案件-{caseEntity.Id}",
                });
            }
            scope.Complete();
            return _mapper.Map<CaseResponse>(caseEntity);
        }

        public async Task<List<CaseRecordFileDto>> UploadFile(int Id, List<IFormFile> files)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var caseRecord = await _context.CaseRecord.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (caseRecord == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            var fileUploadList = new List<CaseRecordFileDto> { };
            var id = 0;

            if (!string.IsNullOrEmpty(caseRecord.Photo))
            {
                var oldFile = JsonSerializer.Deserialize<List<CaseRecordFileDto>>(caseRecord.Photo) ?? new List<CaseRecordFileDto>();
                fileUploadList.AddRange(oldFile);
                id = oldFile.Select(x => x.Id).Max();
            }

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file!.FileName)}";
                var fileUploadDto = await _fileService.Value.UploadFile(fileName, file);
                fileUploadList.Add(new CaseRecordFileDto { Id = ++id, File = fileName });
            }

            var jsonResult = JsonSerializer.Serialize(fileUploadList);
            caseRecord.Photo = jsonResult;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CaseRecord.Update(caseRecord);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改案件附件{caseRecord.Id}",
                });
            }
            scope.Complete();

            return fileUploadList.Select(x => new CaseRecordFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File) }).ToList();
        }

        public async Task DeleteFile(int Id, int fileId)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var caseRecord = await _context.CaseRecord.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (caseRecord == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            var fileList = JsonSerializer.Deserialize<List<CaseRecordFileDto>>(caseRecord.Photo);
            if (fileList!.Where(x => x.Id == fileId).Any())
            {
                var removeFile = fileList!.Where(_x => _x.Id == fileId).FirstOrDefault();
                if (removeFile != null)
                    fileList!.Remove(removeFile!);
            }

            if (fileList != null && fileList.Any())
            {
                var jsonResult = JsonSerializer.Serialize(fileList);
                caseRecord.Photo = jsonResult;
            }
            else
            {
                caseRecord.Photo = string.Empty;
            }
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CaseRecord.Update(caseRecord);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"刪除案件附件-{caseRecord.Id}/{fileId}",
                });
            }
            scope.Complete();
        }
    }
}
