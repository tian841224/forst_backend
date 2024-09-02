using admin_backend.Data;
using admin_backend.DTOs.Case;
using admin_backend.DTOs.CaseDiagnosisResult;
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

        public CaseRecordService(ILogger<CaseRecordService> log, IDbContextFactory<MysqlDbContext> contextFactory, IMapper mapper, Lazy<IFileService> fileService, Lazy<IOperationLogService> operationLogService, ICaseDiagnosisResultService caseDiagnosisResultService)
        {
            _log = log;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _fileService = fileService;
            _operationLogService = operationLogService;
            _caseDiagnosisResultService = caseDiagnosisResultService;
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
            var caseDiagnosisResult = (await _caseDiagnosisResultService.Get(new GetCaseDiagnosisResultDto { CaseId = caseEntity .Id})).Items.FirstOrDefault();

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
                CaseDiagnosisResultResponse = caseDiagnosisResult ,
                CaseStatus = caseEntity.CaseStatus,
            };

            return result;
        }

        public async Task<PagedResult<CaseResponse>> Get(GetCaseRecordDto dto)
        {
            var result = new List<CaseResponse>();
            await using var _context = await _contextFactory.CreateDbContextAsync();
            IQueryable<CaseRecord> caseEntity = _context.CaseRecord;

            //if (!string.IsNullOrEmpty(dto.Keyword))
            //{
            //    string keyword = dto.Keyword.ToLower();
            //    caseEntity = caseEntity.Where(x =>
            //        x.Unit.ToLower().Contains(keyword) ||
            //        x.Name.ToLower().Contains(keyword) ||
            //        x.Author.ToLower().Contains(keyword) ||
            //        x.Id.ToString().Contains(keyword)
            //    );
            //}

            if (dto.AdminUserId.HasValue)
            {
                caseEntity = caseEntity.Where(x => x.AdminUserId == dto.AdminUserId.Value);
            }

            if (dto.CaseNumber.HasValue)
            {
                caseEntity = caseEntity.Where(x => x.CaseNumber == dto.CaseNumber.Value);
            }

            //案件日期
            if (!string.IsNullOrEmpty(dto.Case_StartTime) && !string.IsNullOrEmpty(dto.Case_EndTime))
            {
                //處理時間格式
                if (!DateTime.TryParse(dto.Case_StartTime, out var StartTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.Case_StartTime));
                }
                if (!DateTime.TryParse(dto.Case_EndTime, out var EndTime))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.Case_EndTime));
                }
                caseEntity = caseEntity.Where(x => x.ApplicationDate >= StartTime && x.ApplicationDate < EndTime);
            }

            if (dto.CaseStatus.HasValue)
            {
                caseEntity = caseEntity.Where(x => x.CaseStatus == dto.CaseStatus.Value);
            }

            foreach (var item in await caseEntity.ToListAsync())
            {
                //處理上傳檔案
                var fileList = new List<CaseRecordFileDto>();
                if (!string.IsNullOrEmpty(item.Photo))
                {
                    var fileUpload = JsonSerializer.Deserialize<List<CaseRecordFileDto>>(item.Photo);
                    if (fileUpload != null)
                    {
                        fileList.AddRange(fileUpload.Select(x => new CaseRecordFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File, "image") }));
                    }
                }

                var treeBasicInfoId = await _context.TreeBasicInfo.FirstOrDefaultAsync(x => x.Id == item.TreeBasicInfoId) ?? new TreeBasicInfo();
                var adminUser = await _context.AdminUser.FirstOrDefaultAsync(x => x.Id == item.AdminUserId) ?? new AdminUser();
                var forestCompartmentLocation = await _context.ForestCompartmentLocation.FirstOrDefaultAsync(x => x.Id == item.ForestCompartmentLocationId) ?? new ForestCompartmentLocation();
                //案件回覆
                var caseDiagnosisResult = (await _caseDiagnosisResultService.Get(new GetCaseDiagnosisResultDto { CaseId = item.Id })).Items.FirstOrDefault();

                result.Add(new CaseResponse
                {
                    Id = item.Id,
                    CreateTime = item.CreateTime,
                    UpdateTime = item.UpdateTime,
                    CaseNumber = item.CaseNumber,
                    AdminUserId = item.AdminUserId,
                    AdminUserName = adminUser.Name,
                    ApplicantAccount = item.ApplicantAccount,
                    ApplicantName = item.ApplicantName,
                    ApplicationDate = item.ApplicationDate,
                    UnitName = item.UnitName,
                    County = item.County,
                    District = item.District,
                    Address = item.Address,
                    Phone = item.Phone,
                    Fax = item.Fax,
                    Email = item.Email,
                    DamageTreeCounty = item.DamageTreeCounty,
                    DamageTreeDistrict = item.DamageTreeDistrict,
                    DamageTreeAddress = item.DamageTreeAddress,
                    ForestCompartmentLocationId = item.ForestCompartmentLocationId,
                    ForestPostion = forestCompartmentLocation.Postion,
                    AffiliatedUnit = forestCompartmentLocation.AffiliatedUnit,
                    ForestSection = item.ForestSection,
                    ForestSubsection = item.ForestSubsection,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    DamagedArea = item.DamagedArea,
                    DamagedCount = item.DamagedCount,
                    PlantedArea = item.PlantedArea,
                    PlantedCount = item.PlantedCount,
                    TreeBasicInfoId = item.TreeBasicInfoId,
                    ScientificName = treeBasicInfoId.ScientificName,
                    TreeName = treeBasicInfoId.Name,
                    Others = item.Others,
                    DamagedPart = item.DamagedPart,
                    TreeHeight = item.TreeHeight,
                    TreeDiameter = item.TreeDiameter,
                    LocalPlantingTime = item.LocalPlantingTime,
                    FirstDiscoveryDate = item.FirstDiscoveryDate,
                    DamageDescription = item.DamageDescription,
                    LocationType = item.LocationType,
                    BaseCondition = item.BaseCondition,
                    Photo = fileList,
                    CaseDiagnosisResultResponse = caseDiagnosisResult,
                    CaseStatus = item.CaseStatus,
                });

                ////發布日期
                //if (!string.IsNullOrEmpty(dto.Case_StartTime) && !string.IsNullOrEmpty(dto.Case_EndTime))
                //{
                //    //處理時間格式
                //    if (!DateTime.TryParse(dto.Case_StartTime, out var StartTime))
                //    {
                //        throw new ArgumentException("Invalid date format", nameof(dto.Case_StartTime));
                //    }
                //    if (!DateTime.TryParse(dto.Case_EndTime, out var EndTime))
                //    {
                //        throw new ArgumentException("Invalid date format", nameof(dto.Case_EndTime));
                //    }
                //    caseEntity = caseEntity.Where(x => x.ApplicationDate >= StartTime && x.ApplicationDate < EndTime);
                //}
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
                ForestCompartmentLocationId = dto.ForestCompartmentLocationId,
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

            // 新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
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

            // 更新資料
            if (dto.AdminUserId.HasValue)
                caseEntity.AdminUserId = dto.AdminUserId.Value;

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

            if (dto.ForestCompartmentLocationId.HasValue)
                caseEntity.ForestCompartmentLocationId = dto.ForestCompartmentLocationId.Value;

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

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CaseRecord.Update(caseEntity);

            // 新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
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
    }
}
