using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.Data;
using CommonLibrary.DTOs.File;
using CommonLibrary.DTOs.ForestDiseasePublications;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using CommonLibrary.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Transactions;

namespace admin_backend.Services
{
    public class ForestDiseasePublicationsService : IForestDiseasePublicationsService
    {
        private readonly ILogger<ForestDiseasePublicationsService> _log;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly Lazy<IFileService> _fileService;

        public ForestDiseasePublicationsService(ILogger<ForestDiseasePublicationsService> log, IMapper mapper, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService, Lazy<IFileService> fileService)
        {
            _log = log;
            _mapper = mapper;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
            _fileService = fileService;
        }

        public async Task<List<ForestDiseasePublicationsResponse>> Get(int? Id = null)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var list = new List<ForestDiseasePublications>();

            if (Id.HasValue)
            {
                list = await _context.ForestDiseasePublications.Where(x => x.Id == Id.Value).ToListAsync();
            }
            else
            {
                list = await _context.ForestDiseasePublications.ToListAsync();
            }

            foreach (var item in list)
            {
                var fileDto = JsonSerializer.Deserialize<FileUploadDto>(item.File);
                var file = string.Empty;
                if (fileDto != null)
                    file = await _fileService.Value.FileToBase64(fileDto.FilePath);
            }

            return _mapper.Map<List<ForestDiseasePublicationsResponse>>(list);
        }

        public async Task<ForestDiseasePublicationsResponse> Add(AddForestDiseasePublicationsDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var forestDiseasePublications = await _context.ForestDiseasePublications.Where(x => x.Name == dto.Name).FirstOrDefaultAsync();

            if (forestDiseasePublications != null)
            {
                throw new ApiException($"此名稱重複-{dto.Name}");
            }

            if (dto.Type == 1)
            {
#pragma warning disable CS8602 // 可能 null 參考的取值 (dereference)。
                if (string.IsNullOrEmpty(dto.Unit) || dto.File.Length == 0)
                {
                    throw new ApiException($"請輸入出版單位及檔案");
                }
#pragma warning restore CS8602 // 可能 null 參考的取值 (dereference)。

                //上傳檔案
                var fileUploadDto = await _fileService.Value.UploadFile(dto.File);
                var file = JsonSerializer.Serialize(fileUploadDto);

                forestDiseasePublications = new ForestDiseasePublications
                {
                    Name = dto.Name,
                    Unit = dto.Unit,
                    File = file,
                    Type = dto.Type,
                    Status = dto.Status,
                };
            }
            else
            {
                if (dto.Authors!.Count == 0 || !dto.Date.HasValue || string.IsNullOrEmpty(dto.Link))
                {
                    throw new ApiException($"請輸入出版單位及檔案");
                }

                string Authors = JsonSerializer.Serialize(dto.Authors);

                forestDiseasePublications = new ForestDiseasePublications
                {
                    Name = dto.Name,
                    Link = dto.Link,
                    Date = dto.Date.Value,
                    Author = Authors,
                    Type = dto.Type,
                    Status = dto.Status,
                };
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.ForestDiseasePublications.AddAsync(forestDiseasePublications);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增出版品{forestDiseasePublications.Name}",
                });
            }
            scope.Complete();
            return _mapper.Map<ForestDiseasePublicationsResponse>(forestDiseasePublications);
        }

        public async Task<ForestDiseasePublicationsResponse> Update(int Id, UpdateForestDiseasePublicationsDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var forestDiseasePublications = await _context.ForestDiseasePublications.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (forestDiseasePublications == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            forestDiseasePublications.Type = dto.Type;

            if (!string.IsNullOrEmpty(dto.Name))
                forestDiseasePublications.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Unit))
                forestDiseasePublications.Unit = dto.Unit;

            if (dto.Authors!.Count > 0)
            {
                string Authors = JsonSerializer.Serialize(dto.Authors);
                forestDiseasePublications.Author = Authors;
            }

            if (!string.IsNullOrEmpty(dto.Link))
                forestDiseasePublications.Link = dto.Link;

            if (dto.Date.HasValue)
                forestDiseasePublications.Date = dto.Date.Value;

            forestDiseasePublications.Status = dto.Status;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.ForestDiseasePublications.Update(forestDiseasePublications);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改出版品{forestDiseasePublications.Name}",
                });
            }
            scope.Complete();
            return _mapper.Map<ForestDiseasePublicationsResponse>(forestDiseasePublications);
        }

        public async Task<ForestDiseasePublicationsResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var forestDiseasePublications = await _context.ForestDiseasePublications.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (forestDiseasePublications == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.ForestDiseasePublications.Remove(forestDiseasePublications);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除出版品{forestDiseasePublications.Name}",
                });
            }
            scope.Complete();
            return _mapper.Map<ForestDiseasePublicationsResponse>(forestDiseasePublications);
        }

    }
}
