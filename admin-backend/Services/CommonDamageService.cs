using admin_backend.Data;
using admin_backend.DTOs.CommonDamage;
using admin_backend.DTOs.DamageType;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Transactions;

namespace admin_backend.Services
{
    public class CommonDamageService : ICommonDamageService
    {
        private readonly ILogger<CommonDamageService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IFileService> _fileService;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly IDamageClassService _damageClassService;
        private readonly IDamageTypeService _damageTypeService;

        public CommonDamageService(ILogger<CommonDamageService> log, IDbContextFactory<MysqlDbContext> contextFactory, IMapper mapper, Lazy<IFileService> fileService, Lazy<IOperationLogService> operationLogService, IDamageClassService damageClassService, IDamageTypeService damageTypeService)
        {
            _log = log;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _fileService = fileService;
            _operationLogService = operationLogService;
            _damageClassService = damageClassService;
            _damageTypeService = damageTypeService;
        }

        public async Task<PagedResult<CommonDamageResponse>> Get(int? Id = null, PagedOperationDto? dto = null)
        {
            if (dto == null) dto = new PagedOperationDto();
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<CommonDamage> commonDamages = _context.CommonDamage;

            if (Id.HasValue)
                commonDamages = _context.CommonDamage.Where(x => x.Id == Id);

            var commonDamageList = await commonDamages.ToListAsync();

            var commonDamageResponse = new List<CommonDamageResponse>();

            foreach (var commonDamage in commonDamageList)
            {
                var photo = new List<CommonDamagePhotoResponse>();
                if (!string.IsNullOrEmpty(commonDamage.Photo))
                    photo = JsonSerializer.Deserialize<List<CommonDamagePhotoResponse>>(commonDamage.Photo);

                photo = photo!.Select(x => new CommonDamagePhotoResponse { File = _fileService.Value.GetFile(x.File, "image"), Sort = x.Sort }).ToList();

                var damageTypeName = await _context.DamageType.Where(x => x.Id == commonDamage.DamageTypeId).Select(x => x.Name).FirstOrDefaultAsync();
                if (damageTypeName == null)
                    throw new ApiException($"此危害類型不存在-{commonDamage.DamageTypeId}");

                var damageClassName = await _context.DamageClass.Where(x => x.Id == commonDamage.DamageClassId).Select(x => x.Name).FirstOrDefaultAsync();
                if (damageClassName == null)
                    throw new ApiException($"此危害種類不存在-{commonDamage.DamageClassId}");

                commonDamageResponse.Add(new CommonDamageResponse
                {
                    Id = commonDamage.Id,
                    CreateTime = commonDamage.CreateTime,
                    UpdateTime = commonDamage.UpdateTime,
                    DamageClassId = commonDamage.DamageClassId,
                    DamageClassName = damageClassName,
                    DamageTypeId = commonDamage.DamageTypeId,
                    DamageTypeName = damageTypeName,
                    Name = commonDamage.Name,
                    DamagePart = commonDamage.DamagePart.Select(x => x.GetDescription()).ToList(),
                    DamageFeatures = commonDamage.DamageFeatures,
                    Suggestions = commonDamage.Suggestions,
                    Photo = photo!,
                    Status = commonDamage.Status
                });
            }
            //分頁處理
            return commonDamageResponse.GetPaged(dto!);
        }

        public async Task<PagedResult<CommonDamageResponse>> Get(GetCommonDamageDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var commonDamageList = await _context.CommonDamage.ToListAsync();

            var commonDamageResponse = new List<CommonDamageResponse>();

            foreach (var value in commonDamageList)
            {
                var photo = new List<CommonDamagePhotoResponse>();
                if (!string.IsNullOrEmpty(value.Photo))
                {
                    photo = JsonSerializer.Deserialize<List<CommonDamagePhotoResponse>>(value.Photo);
                    photo = photo!.Select(x => new CommonDamagePhotoResponse { File = _fileService.Value.GetFile(x.File, "image"), Sort = x.Sort }).ToList();
                }

                var damageTypeName = await _context.DamageType.Where(x => x.Id == value.DamageTypeId).Select(x => x.Name).FirstOrDefaultAsync();
                if (damageTypeName == null)
                    throw new ApiException($"此危害類型不存在-{value.DamageTypeId}");

                var damageClassName = await _context.DamageClass.Where(x => x.Id == value.DamageClassId).Select(x => x.Name).FirstOrDefaultAsync();
                if (damageClassName == null)
                    throw new ApiException($"此危害種類不存在-{value.DamageClassId}");

                commonDamageResponse.Add(new CommonDamageResponse
                {
                    Id = value.Id,
                    CreateTime = value.CreateTime,
                    UpdateTime = value.UpdateTime,
                    DamageClassId = value.DamageClassId,
                    DamageClassName = damageClassName,
                    DamageTypeId = value.DamageTypeId,
                    DamageTypeName = damageTypeName,
                    Name = value.Name,
                    DamagePart = value.DamagePart.Select(x => x.GetDescription()).ToList(),
                    DamageFeatures = value.DamageFeatures,
                    Suggestions = value.Suggestions,
                    Photo = photo!,
                    Status = value.Status
                });

                if (!string.IsNullOrEmpty(dto.Keyword))
                {
                    string keyword = dto.Keyword.ToLower();
                    commonDamageResponse = commonDamageResponse.Where(x =>
                        x.Name.ToLower().Contains(keyword) ||
                        x.Id.ToString().Contains(keyword) ||
                        x.Suggestions.Contains(keyword) ||
                          x.DamageClassName.ToLower().Contains(keyword) ||
                        x.DamageTypeName.ToString().Contains(keyword)
                    ).ToList();
                }
            }

            if (dto.Status.HasValue)
            {
                commonDamageResponse = commonDamageResponse.Where(x => x.Status == dto.Status).ToList();
            }

            //分頁處理
            return commonDamageResponse.GetPaged(dto.Page!);
        }

        public async Task<CommonDamageResponse> Add(AddCommonDamageDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var damageClass = await _context.DamageClass.Where(x => x.Id == dto.DamageClassId).AnyAsync();
            if (!damageClass)
                throw new ApiException($"此危害種類不存在-{dto.DamageClassId}");

            var damageType = await _context.DamageType.Where(x => x.Id == dto.DamageTypeId).AnyAsync();
            if (!damageType)
                throw new ApiException($"此危害類型不存在-{dto.DamageTypeId}");

            ////上傳圖片
            //var fileUploadList = new List<string>();
            //foreach (var file in dto.File)
            //{
            //    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Photo.FileName)}";
            //    var fileUploadDto = await _fileService.Value.UploadFile(fileName, file.Photo);
            //    fileUploadList.Add(fileName);
            //}
            //var jsonResult = JsonSerializer.Serialize(fileUploadList);

            //上傳圖片
            if (dto.File == null)
            {
                throw new ApiException($"請上傳檔案");
            }

            var fileUploadList = new List<CommonDamagePhotoResponse>();
            var sort = 0;
            foreach (var file in dto.File)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fileUploadDto = await _fileService.Value.UploadFile(fileName, file);
                fileUploadList.Add(new CommonDamagePhotoResponse { Sort = ++sort, File = fileName });
            }

            var jsonResult = JsonSerializer.Serialize(fileUploadList);

            var commonDamage = new CommonDamage
            {
                DamageTypeId = dto.DamageTypeId,
                DamageClassId = dto.DamageClassId,
                Name = dto.Name,
                DamagePart = dto.DamagePart,
                DamageFeatures = dto.DamageFeatures,
                Suggestions = dto.Suggestions,
                Photo = jsonResult,
                Status = dto.Status,
                Sort = dto.Sort,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.CommonDamage.AddAsync(commonDamage);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增常見病蟲害-{commonDamage.Name}",
                });
            }
            scope.Complete();

            return _mapper.Map<CommonDamageResponse>(commonDamage);
        }

        public async Task<CommonDamageResponse> Update(int Id, UpdateCommonDamageDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var commonDamage = await _context.CommonDamage.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (commonDamage == null)
                throw new ApiException($"找不到此資料-{Id}");

            if (dto.DamageClassId.HasValue)
            {
                var damageClass = await _context.DamageClass.Where(x => x.Id == dto.DamageClassId).AnyAsync();
                if (!damageClass)
                    throw new ApiException($"此危害種類不存在-{dto.DamageClassId}");

                commonDamage.DamageClassId = dto.DamageClassId.Value;
            }

            if (dto.DamageTypeId.HasValue)
            {
                var damageType = await _context.DamageType.Where(x => x.Id == dto.DamageTypeId).AnyAsync();
                if (!damageType)
                    throw new ApiException($"此危害類型不存在-{dto.DamageTypeId}");

                commonDamage.DamageTypeId = dto.DamageTypeId.Value;
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                commonDamage.Name = dto.Name;
            }

            if (dto.DamagePart != null && dto.DamagePart.Count() > 0)
            {
                commonDamage.DamagePart = dto.DamagePart;
            }

            if (!string.IsNullOrWhiteSpace(dto.DamageFeatures))
            {
                commonDamage.DamageFeatures = dto.DamageFeatures;
            }

            if (!string.IsNullOrWhiteSpace(dto.Suggestions))
            {
                commonDamage.Suggestions = dto.Suggestions;
            }

            if (dto.Status.HasValue)
            {
                commonDamage.Status = dto.Status.Value;
            }

            ////上傳圖片
            //if (dto.File == null)
            //{
            //    throw new ApiException($"請上傳檔案");
            //}

            //var fileUploadList = new List<CommonDamagePhotoResponse>();
            //var sort = 0;
            //foreach (var file in dto.File)
            //{
            //    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Photo.FileName)}";
            //    var fileUploadDto = await _fileService.Value.UploadFile(fileName, file.Photo);
            //    fileUploadList.Add(new CommonDamagePhotoResponse { Sort = ++sort, File = fileName });
            //}

            //var jsonResult = JsonSerializer.Serialize(fileUploadList);
            //commonDamage.Photo = jsonResult;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CommonDamage.Update(commonDamage);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改常見病蟲害-{commonDamage.Name}",
                });
            }
            scope.Complete();

            return _mapper.Map<CommonDamageResponse>(commonDamage);
        }

        public async Task<List<CommonDamageResponse>> UpdateSort(List<SortBasicDto> dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var result = new List<CommonDamageResponse>();

            foreach (var value in dto)
            {
                var commonDamage = await _context.CommonDamage.Where(x => x.Id == value.Id).FirstOrDefaultAsync();
                if (commonDamage == null) continue;

                if (value.Sort.HasValue)
                    commonDamage.Sort = value.Sort.Value;

                _context.CommonDamage.Update(commonDamage);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _operationLogService.Value.Add(new AddOperationLogDto
                    {
                        Type = ChangeTypeEnum.Edit,
                        Content = $"修改常見病蟲害排序：{commonDamage.Id}/{commonDamage.Sort}",
                    });
                };

                result.Add(_mapper.Map<CommonDamageResponse>(commonDamage));
            }

            scope.Complete();
            return result;
        }

        public async Task<List<CommonDamagePhotoResponse>> UpdateFileSort(int Id, List<UpdateFileSortDto> dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var commonDamage = await _context.CommonDamage.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (commonDamage == null)
                throw new ApiException($"找不到此資料-{Id}");

            var result = new List<CommonDamagePhotoResponse>();

            foreach (var value in dto)
            {
                if (commonDamage.Photo.Contains(value.Name))
                {
                    var commonDamagePhotoList = JsonSerializer.Deserialize<List<CommonDamagePhotoResponse>>(commonDamage.Photo);
                    var commonDamagePhoto = commonDamagePhotoList!.Where(x => x.File.Contains(value.Name)).FirstOrDefault();

                    commonDamagePhoto!.Sort = value.Sort;
                    commonDamagePhoto.File = commonDamagePhoto.File;

                    result.Add(commonDamagePhoto);
                }
            }

            commonDamage.Photo = JsonSerializer.Serialize(result);
            _context.CommonDamage.Update(commonDamage);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改常見病蟲害檔案排序：{commonDamage.Id}",
                });
            };

            scope.Complete();
            return result;
        }

        public async Task<List<CommonDamagePhotoResponse>> UploadFile(int Id, CommonDamagePhotoDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var commonDamage = await _context.CommonDamage.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (commonDamage == null)
                throw new ApiException($"找不到此資料-{Id}");

            var result = new List<CommonDamagePhotoResponse> { };

            if (!string.IsNullOrEmpty(commonDamage.Photo))
                result.AddRange(JsonSerializer.Deserialize<List<CommonDamagePhotoResponse>>(commonDamage.Photo)!);

            //上傳檔案
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto!.Photo.FileName)}";
            await _fileService.Value.UploadFile(fileName, dto.Photo);
            result.Add(new CommonDamagePhotoResponse { Sort = dto.Sort, File = fileName });

            var photo = JsonSerializer.Serialize(result);
            commonDamage.Photo = photo;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CommonDamage.Update(commonDamage);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"上傳常見病蟲害圖片-{commonDamage.Name}",
                });
            }
            scope.Complete();

            return result;
        }

        public async Task<CommonDamageResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var commonDamage = await _context.CommonDamage.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (commonDamage == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CommonDamage.Remove(commonDamage);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除常見病蟲害-{commonDamage.Id}/{commonDamage.Name}",
                });
            }
            scope.Complete();
            return _mapper.Map<CommonDamageResponse>(commonDamage);
        }

        public async Task DeleteFile(int Id, string fileId)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var commonDamage = await _context.CommonDamage.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (commonDamage == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            var fileList = JsonSerializer.Deserialize<List<CommonDamagePhotoResponse>>(commonDamage.Photo);
            if (fileList!.Where(x => x.File.Contains(fileId)).Any())
            {
                var removeFile = fileList!.Where(x => x.File.Contains(fileId)).FirstOrDefault();
                fileList!.Remove(removeFile!);
            }

            var jsonResult = JsonSerializer.Serialize(fileList);
            commonDamage.Photo = jsonResult;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.CommonDamage.Update(commonDamage);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"刪除常見病蟲害圖片-{commonDamage.Name}/{fileId}",
                });
            }
            scope.Complete();
        }
    }
}
