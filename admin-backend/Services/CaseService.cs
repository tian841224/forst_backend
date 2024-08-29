using admin_backend.Data;
using admin_backend.DTOs.Case;
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
    public class CaseService : ICaseService
    {
        private readonly ILogger<AdSettingService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IFileService> _fileService;
        private readonly IOperationLogService _operationLogService;


        public CaseService(ILogger<AdSettingService> log, IDbContextFactory<MysqlDbContext> contextFactory, IMapper mapper, Lazy<IFileService> fileService, IOperationLogService operationLogService)
        {
            _log = log;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _fileService = fileService;
            _operationLogService = operationLogService;
        }

        public async Task<CaseResponse> Get(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var caseEntity = await _context.Case.FirstOrDefaultAsync(x => x.Id == Id);

            if (caseEntity == null)
            {
                throw new ApiException($"找不到此案件資料 - {Id}");
            }

            var fileList = new List<CaseFileDto>();
            if (!string.IsNullOrEmpty(caseEntity.Photo))
            {
                var fileUpload = JsonSerializer.Deserialize<List<CaseFileDto>>(caseEntity.Photo);
                if (fileUpload != null)
                {
                    fileList.AddRange(fileUpload.Select(x => new CaseFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File, "image") }));
                }
            }

            var treeBasicInfoId = await _context.TreeBasicInfo.FirstOrDefaultAsync(x => x.Id == caseEntity.TreeBasicInfoId) ?? new TreeBasicInfo();
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == caseEntity.UserId) ?? new User();
            var adminUser = await _context.AdminUser.FirstOrDefaultAsync(x => x.Id == caseEntity.AdminUserId) ?? new AdminUser();

            var result = new CaseResponse
            {
                ApplicationDate = caseEntity.ApplicationDate,
                AdminUserId = caseEntity.AdminUserId,
                AdminUserName = adminUser.Name,
                UserId = caseEntity.UserId,
                UserName = user.Name,
                TreeBasicInfoId = caseEntity.TreeBasicInfoId,
                TreeBasicInfoName = treeBasicInfoId.Name,
                ScientificName = treeBasicInfoId.ScientificName,
                CaseNumber = caseEntity.CaseNumber,
                ForestSectionLocation = caseEntity.ForestSectionLocation,
                CaseAddress = caseEntity.DamageTreeAddress,
                District = caseEntity.District,
                County = caseEntity.County,
                Address = caseEntity.Address,
                BaseCondition = caseEntity.BaseCondition,
                DamagedCount = caseEntity.DamagedCount,
                PlantedCount = caseEntity.PlantedCount,
                AffiliatedUnit = caseEntity.AffiliatedUnit,
                DamagedArea = caseEntity.DamagedArea,
                DamageDescription = caseEntity.DamageDescription,
                DamagedPart = caseEntity.DamagedPart,
                Email = caseEntity.Email,
                Fax = caseEntity.Fax,
                FirstDiscoveryDate = caseEntity.FirstDiscoveryDate,
                ForestSection = caseEntity.ForestSection,
                ForestSubsection = caseEntity.ForestSubsection,
                LocalPlantingTime = caseEntity.LocalPlantingTime,
                TreeDiameter = caseEntity.TreeDiameter,
                LocationType = caseEntity.LocationType,
                Others = caseEntity.Others,
                Phone = caseEntity.Phone,
                Photo = fileList,
                PlantedArea = caseEntity.PlantedArea,
                TreeHeight = caseEntity.TreeHeight,
                UnitName = caseEntity.UnitName,
                LatitudeGoogle = caseEntity.LatitudeGoogle,
                LatitudeTgos = caseEntity.LatitudeTgos,
                LongitudeGoogle = caseEntity.LongitudeGoogle,
                LongitudeTgos = caseEntity.LongitudeTgos,
                CaseStatus = caseEntity.CaseStatus,
            };

            return result;
        }

        public async Task<PagedResult<CaseResponse>> Get(GetCaseDto dto)
        {
            var result = new List<CaseResponse>();
            await using var _context = await _contextFactory.CreateDbContextAsync();
            IQueryable<Case> caseEntity = _context.Case;

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

            if (dto.StartTime.HasValue && dto.EndDate.HasValue)
            {
                caseEntity = caseEntity.Where(x => x.ApplicationDate >= dto.StartTime.Value && x.ApplicationDate < dto.EndDate.Value);
            }

            if (dto.CaseStatus.HasValue)
            {
                caseEntity = caseEntity.Where(x => x.CaseStatus == dto.CaseStatus.Value);
            }

            foreach (var item in caseEntity)
            {
                //處理上傳檔案
                var fileList = new List<CaseFileDto>();
                if (!string.IsNullOrEmpty(item.Photo))
                {
                    var fileUpload = JsonSerializer.Deserialize<List<CaseFileDto>>(item.Photo);
                    if (fileUpload != null)
                    {
                        fileList.AddRange(fileUpload.Select(x => new CaseFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File, "image") }));
                    }
                }

                var treeBasicInfoId = await _context.TreeBasicInfo.FirstOrDefaultAsync(x => x.Id == item.TreeBasicInfoId) ?? new TreeBasicInfo();
                var user = await _context.User.FirstOrDefaultAsync(x => x.Id == item.UserId) ?? new User();
                var adminUser = await _context.AdminUser.FirstOrDefaultAsync(x => x.Id == item.AdminUserId) ?? new AdminUser();

                result.Add(new CaseResponse
                {
                    Id = item.Id,
                    CreateTime = item.CreateTime,
                    UpdateTime = item.UpdateTime,
                    ApplicationDate = item.ApplicationDate,
                    AdminUserId = item.AdminUserId,
                    AdminUserName = adminUser.Name,
                    UserId = item.UserId,
                    UserName = user.Name,
                    ForestSectionLocation = item.ForestSectionLocation,
                    TreeBasicInfoId = item.TreeBasicInfoId,
                    TreeBasicInfoName = treeBasicInfoId.Name,
                    ScientificName = treeBasicInfoId.ScientificName,
                    CaseNumber = item.CaseNumber,
                    CaseAddress = item.DamageTreeAddress,
                    District = item.District,
                    County = item.County,
                    Address = item.Address,
                    BaseCondition = item.BaseCondition,
                    DamagedCount = item.DamagedCount,
                    PlantedCount = item.PlantedCount,
                    AffiliatedUnit = item.AffiliatedUnit,
                    DamagedArea = item.DamagedArea,
                    DamageDescription = item.DamageDescription,
                    DamagedPart = item.DamagedPart,
                    Email = item.Email,
                    Fax = item.Fax,
                    FirstDiscoveryDate = item.FirstDiscoveryDate,
                    ForestSection = item.ForestSection,
                    ForestSubsection = item.ForestSubsection,
                    LocalPlantingTime = item.LocalPlantingTime,
                    TreeDiameter = item.TreeDiameter,
                    LocationType = item.LocationType,
                    Others = item.Others,
                    Phone = item.Phone,
                    Photo = fileList,
                    PlantedArea = item.PlantedArea,
                    TreeHeight = item.TreeHeight,
                    UnitName = item.UnitName,
                    LatitudeGoogle = item.LatitudeGoogle,
                    LatitudeTgos = item.LatitudeTgos,
                    LongitudeGoogle = item.LongitudeGoogle,
                    LongitudeTgos = item.LongitudeTgos,
                    CaseStatus = item.CaseStatus,
                });
            }

            return result.GetPaged(dto.Page!);
        }
        public async Task<CaseResponse> Add(AddCaseDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            //編案件編號
            var maxCaseNumber = await _context.Case
                .MaxAsync(x => (int?)x.CaseNumber) ?? 0;

            if (maxCaseNumber == 0)
            {
                var today = DateTime.Now;
                string datePrefix = today.ToString("yyyyMMdd");
                maxCaseNumber = int.Parse(datePrefix + "01");
            }
            else maxCaseNumber = maxCaseNumber + 1;

            //處理上傳檔案
            var fileUploadList = new List<CaseFileDto>();
            var id = 0;
            foreach (var file in dto.Photo)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file!.FileName)}";
                var fileUploadDto = await _fileService.Value.UploadFile(fileName, file);
                fileUploadList.Add(new CaseFileDto { Id = ++id, File = fileName });
            }
            var jsonResult = JsonSerializer.Serialize(fileUploadList);

            var caseEntity = new Case
            {
                ApplicationDate = dto.ApplicationDate,
                UserId = dto.UserId,
                TreeBasicInfoId = dto.TreeBasicInfoId,
                CaseNumber = maxCaseNumber,
                //CaseAddress = dto.CaseAddress,
                District = dto.City,
                ForestSectionLocation = dto.ForestSectionLocation,
                BaseCondition = dto.BaseCondition,
                DamagedCount = dto.DamagedCount,
                PlantedCount = dto.PlantedCount,
                Address = dto.Address,
                AffiliatedUnit = dto.AffiliatedUnit,
                DamagedArea = dto.DamagedArea,
                DamageDescription = dto.DamageDescription,
                DamagedPart = dto.DamagedPart,
                Email = dto.Email,
                Fax = dto.Fax,
                FirstDiscoveryDate = dto.FirstDiscoveryDate,
                ForestSection = dto.ForestSection,
                ForestSubsection = dto.ForestSubsection,
                LocalPlantingTime = dto.LocalPlantingTime,
                TreeDiameter = dto.TreeDiameter,
                LocationType = dto.LocationType,
                Others = dto.Others,
                Phone = dto.Phone,
                Photo = jsonResult,
                PlantedArea = dto.PlantedArea,
                TreeHeight = dto.TreeHeight,
                UnitName = dto.UnitName,
                LatitudeGoogle = dto.LatitudeGoogle,
                LatitudeTgos = dto.LatitudeTgos,
                LongitudeGoogle = dto.LongitudeGoogle,
                LongitudeTgos = dto.LongitudeTgos,
                CaseStatus = CaseStatusEnum.Pending_Assignment,
            };

            await _context.Case.AddAsync(caseEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<CaseResponse>(caseEntity);
        }

        public async Task<CaseResponse> Update(int Id, UpdateCaseDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var caseEntity = await _context.Case.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (caseEntity == null)
            {
                throw new ApiException($"找不到此案件資料 - {Id}");
            }

            // 更新資料
            if (dto.UserId.HasValue)
                caseEntity.UserId = dto.UserId.Value;

            if (dto.ApplicationDate.HasValue)
                caseEntity.ApplicationDate = dto.ApplicationDate.Value;

            if (!string.IsNullOrEmpty(dto.UnitName))
                caseEntity.UnitName = dto.UnitName;

            if (!string.IsNullOrEmpty(dto.City))
                caseEntity.District = dto.City;

            if (!string.IsNullOrEmpty(dto.Address))
                caseEntity.Address = dto.Address;

            if (!string.IsNullOrEmpty(dto.Phone))
                caseEntity.Phone = dto.Phone;

            if (!string.IsNullOrEmpty(dto.Fax))
                caseEntity.Fax = dto.Fax;

            if (!string.IsNullOrEmpty(dto.Email))
                caseEntity.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.CaseAddress))
                //caseEntity.CaseAddress = dto.CaseAddress;

            if (!string.IsNullOrEmpty(dto.AffiliatedUnit))
                caseEntity.AffiliatedUnit = dto.AffiliatedUnit;

            if (!string.IsNullOrEmpty(dto.ForestSection))
                caseEntity.ForestSection = dto.ForestSection;

            if (!string.IsNullOrEmpty(dto.ForestSubsection))
                caseEntity.ForestSubsection = dto.ForestSubsection;

            if (!string.IsNullOrEmpty(dto.LatitudeTgos))
                caseEntity.LatitudeTgos = dto.LatitudeTgos;

            if (!string.IsNullOrEmpty(dto.LatitudeGoogle))
                caseEntity.LatitudeGoogle = dto.LatitudeGoogle;

            if (!string.IsNullOrEmpty(dto.LongitudeTgos))
                caseEntity.LongitudeTgos = dto.LongitudeTgos;

            if (!string.IsNullOrEmpty(dto.LongitudeGoogle))
                caseEntity.LongitudeGoogle = dto.LongitudeGoogle;

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

            if (dto.TreeHeight.HasValue)
                caseEntity.TreeHeight = dto.TreeHeight.Value;

            if (dto.TreeDiameter.HasValue)
                caseEntity.TreeDiameter = dto.TreeDiameter.Value;

            if (dto.LocalPlantingTime.HasValue)
                caseEntity.LocalPlantingTime = dto.LocalPlantingTime.Value;

            if (dto.FirstDiscoveryDate.HasValue)
                caseEntity.FirstDiscoveryDate = dto.FirstDiscoveryDate.Value;

            if (!string.IsNullOrEmpty(dto.DamageDescription))
                caseEntity.DamageDescription = dto.DamageDescription;

            if (dto.LocationType != null && dto.LocationType.Count > 0)
                caseEntity.LocationType = dto.LocationType;

            if (dto.BaseCondition != null && dto.BaseCondition.Count > 0)
                caseEntity.BaseCondition = dto.BaseCondition;

            if (dto.CaseStatus.HasValue)
                caseEntity.CaseStatus = dto.CaseStatus.Value;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.Case.Update(caseEntity);

            // 新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
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

            var caseEntity = await _context.Case.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (caseEntity == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.Case.Remove(caseEntity);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
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
