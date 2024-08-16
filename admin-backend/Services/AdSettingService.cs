using admin_backend.Data;
using admin_backend.DTOs.AdSetting;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class AdSettingService : IAdSettingService
    {
        private readonly ILogger<AdSettingService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IFileService> _fileService;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly IAdminUserServices _adminUserServices;

        public AdSettingService(ILogger<AdSettingService> log, IDbContextFactory<MysqlDbContext> contextFactory, IMapper mapper, Lazy<IFileService> fileService, Lazy<IOperationLogService> operationLogService, IAdminUserServices adminUserServices)
        {
            _log = log;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _fileService = fileService;
            _operationLogService = operationLogService;
            _adminUserServices = adminUserServices;
        }

        public async Task<PagedResult<AdSettingResponse>> Get(int? Id = null, PagedOperationDto? dto = null)
        {
            if (dto == null) dto = new PagedOperationDto();
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<AdSetting> adSettings = _context.AdSetting;

            if (Id.HasValue)
                adSettings = _context.AdSetting.Where(x => x.Id == Id);

            //var adSettingResponses = _mapper.Map<List<AdSettingResponse>>(adSettings);

            var adSettingResponses = (await adSettings.ToListAsync()).Select(x => new AdSettingResponse
            {
                Id = x.Id,
                Name = x.Name,
                AdminUserId = x.AdminUserId,
                Status = x.Status,
                Website = x.Website.Select(e => e.GetDescription()).ToList(),
                PhotoPc = x.PhotoPc,
                PhotoMobile = x.PhotoMobile,
                CreateTime = x.CreateTime,
                UpdateTime = x.UpdateTime,
            }).ToList();

            foreach (var adSetting in adSettingResponses)
            {
                adSetting.AdminUserName = await _context.AdminUser
                    .Where(y => y.Id == adSetting.Id)
                    .Select(y => y.Name)
                    .FirstOrDefaultAsync() ?? string.Empty;

                if (!string.IsNullOrEmpty(adSetting.PhotoPc))
                {
                    adSetting.PhotoPc = _fileService.Value.GetFile(adSetting.PhotoPc, "image");
                }

                if (!string.IsNullOrEmpty(adSetting.PhotoMobile))
                {
                    adSetting.PhotoMobile = _fileService.Value.GetFile(adSetting.PhotoMobile, "image");
                }
            }

            //分頁處理
            return adSettingResponses.GetPaged(dto!);
        }

        public async Task<PagedResult<AdSettingResponse>> Get(GetdSettingDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<AdSetting> adSettings = _context.AdSetting;

            if (!string.IsNullOrEmpty(dto.Name))
                adSettings = _context.AdSetting.Where(x => x.Name.Contains(dto.Name));

            //var adSettingResponses = _mapper.Map<List<AdSettingResponse>>(adSettings);

            var adSettingResponses = (await adSettings.ToListAsync()).Select(x => new AdSettingResponse
            {
                Id = x.Id,
                Name = x.Name,
                AdminUserId = x.AdminUserId,
                Status = x.Status,
                Website = x.Website.Select(e => e.GetDescription()).ToList(),
                PhotoPc = x.PhotoPc,
                PhotoMobile = x.PhotoMobile,
                CreateTime = x.CreateTime,
                UpdateTime = x.UpdateTime,
            }).ToList();

            foreach (var adSetting in adSettingResponses)
            {
                adSetting.AdminUserName = await _context.AdminUser
                    .Where(y => y.Id == adSetting.AdminUserId)
                    .Select(y => y.Name)
                    .FirstOrDefaultAsync() ?? string.Empty;

                if (!string.IsNullOrEmpty(adSetting.PhotoPc))
                {
                    adSetting.PhotoPc = _fileService.Value.GetFile(adSetting.PhotoPc, "image");
                }

                if (!string.IsNullOrEmpty(adSetting.PhotoMobile))
                {
                    adSetting.PhotoMobile = _fileService.Value.GetFile(adSetting.PhotoMobile, "image");
                }
            }


            //分頁處理
            return adSettingResponses.GetPaged(dto.Page!);
        }

        public async Task<AdSettingResponse> Add(AddAdSettingDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            //取得當前使用者身分
            var adminUser = await _adminUserServices.Get();
            if (adminUser == null)
            { throw new ApiException($"無法取得當前使用者身分"); }


            var adSettings = new AdSetting
            {
                AdminUserId = adminUser.Id,
                Name = dto.Name,
                Website = dto.Website,
                Status = dto.Status,
            };

            if (dto.PhotoMobile != null && dto.PhotoMobile.Length > 0)
            {
                //上傳檔案
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.PhotoMobile.FileName)}";
                await _fileService.Value.UploadFile(fileName, dto.PhotoMobile);
                adSettings.PhotoMobile = fileName;
            }

            if (dto.PhotoPc != null && dto.PhotoPc.Length > 0)
            {
                //上傳檔案
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.PhotoPc.FileName)}";
                await _fileService.Value.UploadFile(fileName, dto.PhotoPc);
                adSettings.PhotoPc = fileName;
            }
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.AdSetting.AddAsync(adSettings);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增官網廣告版位：{adSettings.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<AdSettingResponse>(adSettings);
        }

        public async Task<AdSettingResponse> Update(int Id, UpdateAdSettingDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var adSettings = await _context.AdSetting.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (adSettings == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                adSettings.Name = dto.Name;

            if (dto.Website != null && dto.Website.Count > 0)
                adSettings.Website = dto.Website;

            if (dto.Status.HasValue)
                adSettings.Status = dto.Status.Value;

            if (dto.PhotoMobile != null && dto.PhotoMobile.Length > 0)
            {
                //上傳檔案
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.PhotoMobile.FileName)}";
                await _fileService.Value.UploadFile(fileName, dto.PhotoMobile);
                adSettings.PhotoMobile = fileName;
            }

            if (dto.PhotoPc != null && dto.PhotoPc.Length > 0)
            {
                //上傳檔案
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.PhotoPc.FileName)}";
                await _fileService.Value.UploadFile(fileName, dto.PhotoPc);
                adSettings.PhotoPc = fileName;
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.AdSetting.Update(adSettings);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改官網廣告版位：{adSettings.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<AdSettingResponse>(adSettings);
        }

        public async Task UploadFile(int Id, AdSettingPhotoDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var adSettings = await _context.AdSetting.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (adSettings == null)
                throw new ApiException($"找不到此資料-{Id}");

            if (dto.PhotoMobile != null && dto.PhotoMobile.Length > 0)
            {
                //上傳檔案
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.PhotoMobile.FileName)}";
                await _fileService.Value.UploadFile(fileName, dto.PhotoMobile);
                adSettings.PhotoMobile = fileName;
            }

            if (dto.PhotoPc != null && dto.PhotoPc.Length > 0)
            {
                //上傳檔案
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.PhotoPc.FileName)}";
                await _fileService.Value.UploadFile(fileName, dto.PhotoPc);
                adSettings.PhotoPc = fileName;
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.AdSetting.Update(adSettings);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"上傳官網廣告版位圖片-{adSettings.Name}",
                });
            }
            scope.Complete();
            return;
        }

        public async Task<AdSettingResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var adSetting = await _context.AdSetting.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (adSetting == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.AdSetting.Remove(adSetting);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除官網廣告版位：{adSetting.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<AdSettingResponse>(adSetting);
        }
    }
}
