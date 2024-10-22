﻿using admin_backend.Data;
using admin_backend.DTOs.ForestDiseasePublications;
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

        public async Task<PagedResult<ForestDiseasePublicationsResponse>> Get(int? Id = null, PagedOperationDto? dto = null)
        {
            if (dto == null) dto = new PagedOperationDto();
            var result = new List<ForestDiseasePublicationsResponse>();
            await using var _context = await _contextFactory.CreateDbContextAsync();
            IQueryable<ForestDiseasePublications> forestDiseasePublications = _context.ForestDiseasePublications;

            try
            {
                if (Id.HasValue)
                    forestDiseasePublications = _context.ForestDiseasePublications.Where(x => x.Id == Id.Value);

                foreach (var item in forestDiseasePublications)
                {
                    var fileList = new List<ForestDiseasePublicationsFileDto>();
                    if (!string.IsNullOrEmpty(item.File))
                    {
                        var fileUpload = JsonSerializer.Deserialize<List<ForestDiseasePublicationsFileDto>>(item.File);
                        if (fileUpload != null)
                        {
                            if (item.Type == 1)
                                fileList.AddRange(fileUpload.Select(x => new ForestDiseasePublicationsFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File, "image") }));
                            else
                                fileList.AddRange(fileUpload.Select(x => new ForestDiseasePublicationsFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File) }));
                        }
                    }
                    result.Add(new ForestDiseasePublicationsResponse
                    {
                        Id = item.Id,
                        Type = item.Type,
                        TypeName = item.Type == 1 ? "林業叢刊" : "相關摺頁",
                        Name = item.Name,
                        Authors = string.IsNullOrEmpty(item.Author) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(item.Author)!,
                        Date = item.Date,
                        Link = item.Link,
                        Sort = item.Sort,
                        Status = item.Status,
                        Unit = item.Unit,
                        File = fileList,
                        CreateTime = item.CreateTime,
                        UpdateTime = item.UpdateTime,
                    });
                }
                return result.GetPaged(dto!);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<PagedResult<ForestDiseasePublicationsResponse>> Get(GetForestDiseasePublicationsDto dto)
        {
            var result = new List<ForestDiseasePublicationsResponse>();
            await using var _context = await _contextFactory.CreateDbContextAsync();
            IQueryable<ForestDiseasePublications> forestDiseasePublications = _context.ForestDiseasePublications;

            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                string keyword = dto.Keyword.ToLower();
                forestDiseasePublications = forestDiseasePublications.Where(x =>
                    x.Unit.ToLower().Contains(keyword) ||
                    x.Name.ToLower().Contains(keyword) ||
                    x.Author.ToLower().Contains(keyword) ||
                    x.Id.ToString().Contains(keyword)
                );
            }

            if (dto.Type.HasValue)
            {
                forestDiseasePublications = forestDiseasePublications.Where(x => x.Type == dto.Type.Value);
            }

            if (dto.Status.HasValue)
            {
                forestDiseasePublications = forestDiseasePublications.Where(x => x.Status == dto.Status.Value);
            }

            foreach (var item in forestDiseasePublications)
            {
                var fileList = new List<ForestDiseasePublicationsFileDto>();
                if (!string.IsNullOrEmpty(item.File))
                {
                    var fileUpload = JsonSerializer.Deserialize<List<ForestDiseasePublicationsFileDto>>(item.File);
                    if (fileUpload != null)
                    {
                        if (item.Type == 1)
                            fileList.AddRange(fileUpload.Select(x => new ForestDiseasePublicationsFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File, "image") }));
                        else
                            fileList.AddRange(fileUpload.Select(x => new ForestDiseasePublicationsFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File) }));
                    }
                }

                result.Add(new ForestDiseasePublicationsResponse
                {
                    Id = item.Id,
                    Type = item.Type,
                    TypeName = item.Type == 1 ? "林業叢刊" : "相關摺頁",
                    Name = item.Name,
                    Authors = string.IsNullOrEmpty(item.Author) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(item.Author)!,
                    Date = item.Date,
                    Link = item.Link,
                    Sort = item.Sort,
                    Status = item.Status,
                    Unit = item.Unit,
                    File = fileList,
                    CreateTime = item.CreateTime,
                    UpdateTime = item.UpdateTime,
                });
            }

            return result.GetPaged(dto.Page!);
        }

        public async Task<ForestDiseasePublicationsResponse> Add(AddForestDiseasePublicationsDto dto)
        {
            var result = new ForestDiseasePublicationsResponse();

            await using var _context = await _contextFactory.CreateDbContextAsync();

            var forestDiseasePublications = await _context.ForestDiseasePublications.Where(x => x.Name == dto.Name && x.Type == dto.Type).FirstOrDefaultAsync();

            if (forestDiseasePublications != null)
            {
                throw new ApiException($"此名稱重複-{dto.Name}");
            }

            forestDiseasePublications = new ForestDiseasePublications();

            if (dto.File.Count == 0)
            {
                throw new ApiException($"請上傳檔案");
            }

            var fileUploadList = new List<ForestDiseasePublicationsFileDto>();
            var id = 0;
            foreach (var file in dto.File)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file!.FileName)}";
                var fileUploadDto = await _fileService.Value.UploadFile(fileName, file);
                fileUploadList.Add(new ForestDiseasePublicationsFileDto { Id = ++id, File = fileName });
            }

            var jsonResult = JsonSerializer.Serialize(fileUploadList);
            forestDiseasePublications.File = jsonResult;
            var date = DateTime.MinValue;

            if (dto.Type == 1)
            {
                if (dto.Authors!.Count == 0 || string.IsNullOrEmpty(dto.Date) || string.IsNullOrEmpty(dto.Link))
                {
                    throw new ApiException($"請輸入作者、日期、連結");
                }

                if (string.IsNullOrEmpty(dto.Date))
                    throw new ApiException($"若需設定排程，請輸入起始結束時間");

                if (!DateTime.TryParse(dto.Date, out date))
                {
                    throw new ArgumentException("Invalid date format", nameof(dto.Date));
                }

                forestDiseasePublications.Name = dto.Name;
                forestDiseasePublications.Link = dto.Link;
                forestDiseasePublications.Date = date;
                forestDiseasePublications.Author = JsonSerializer.Serialize(dto.Authors);
                forestDiseasePublications.Type = dto.Type;
                forestDiseasePublications.Status = dto.Status;
                forestDiseasePublications.Sort = dto.Sort;
            }
            else
            {
                if (string.IsNullOrEmpty(dto.Unit))
                {
                    throw new ApiException($"請輸入出版單位");
                }
                forestDiseasePublications.Name = dto.Name;
                forestDiseasePublications.Unit = dto.Unit;
                forestDiseasePublications.Type = dto.Type;
                forestDiseasePublications.Status = dto.Status;
                forestDiseasePublications.Sort = dto.Sort;
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

            result.Id = forestDiseasePublications.Id;
            result.CreateTime = forestDiseasePublications.CreateTime;
            result.UpdateTime = forestDiseasePublications.UpdateTime;
            result.Name = dto.Name;
            result.Unit = dto.Unit ?? string.Empty;
            result.Authors = dto.Authors!.Count == 0 ? new List<string>() : dto.Authors;
            result.Link = dto.Link ?? string.Empty;
            result.Date = date;
            result.Type = dto.Type;
            result.TypeName = dto.Type == 1 ? "林業叢刊" : "相關摺頁";
            result.Status = dto.Status;
            result.Sort = dto.Sort;
            //result.File = fileUploadList;

            return result;
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

            var date = DateTime.MinValue;

            if (string.IsNullOrEmpty(dto.Date))
                throw new ApiException($"若需設定排程，請輸入起始結束時間");

            if (!DateTime.TryParse(dto.Date, out date))
            {
                throw new ArgumentException("Invalid date format", nameof(dto.Date));
            }

            forestDiseasePublications.Date = date;

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

        public async Task<List<ForestDiseasePublicationsFileDto>> UploadFile(int Id, List<IFormFile> files)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var forestDiseasePublications = await _context.ForestDiseasePublications.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (forestDiseasePublications == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            var fileUploadList = new List<ForestDiseasePublicationsFileDto> { };
            var id = 0;

            if (!string.IsNullOrEmpty(forestDiseasePublications.File))
            {
                var oldFile = JsonSerializer.Deserialize<List<ForestDiseasePublicationsFileDto>>(forestDiseasePublications.File) ?? new List<ForestDiseasePublicationsFileDto>();
                fileUploadList.AddRange(oldFile);
                id = oldFile.Select(x => x.Id).Max();
            }

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file!.FileName)}";
                var fileUploadDto = await _fileService.Value.UploadFile(fileName, file);
                fileUploadList.Add(new ForestDiseasePublicationsFileDto { Id = ++id, File = fileName });
            }

            var jsonResult = JsonSerializer.Serialize(fileUploadList);
            forestDiseasePublications.File = jsonResult;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.ForestDiseasePublications.Update(forestDiseasePublications);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改出版品附件{forestDiseasePublications.Name}",
                });
            }
            scope.Complete();

            return fileUploadList.Select(x => new ForestDiseasePublicationsFileDto { Id = x.Id, File = _fileService.Value.GetFile(x.File) }).ToList();
        }

        public async Task<List<ForestDiseasePublicationsResponse>> UpdateSort(List<SortBasicDto> dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var result = new List<ForestDiseasePublicationsResponse>();

            foreach (var value in dto)
            {
                var forestDiseasePublications = await _context.ForestDiseasePublications.Where(x => x.Id == value.Id).FirstOrDefaultAsync();
                if (forestDiseasePublications == null) continue;

                if (value.Sort.HasValue)
                    forestDiseasePublications.Sort = value.Sort.Value;

                _context.ForestDiseasePublications.Update(forestDiseasePublications);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _operationLogService.Value.Add(new AddOperationLogDto
                    {
                        Type = ChangeTypeEnum.Edit,
                        Content = $"修改林班位置排序：{forestDiseasePublications.Id}/{forestDiseasePublications.Sort}",
                    });
                };

                result.Add(_mapper.Map<ForestDiseasePublicationsResponse>(forestDiseasePublications));
            }

            scope.Complete();
            return result;
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
                    Content = $"刪除出版品-{forestDiseasePublications.Id}/{forestDiseasePublications.Name}",
                });
            }
            scope.Complete();
            return _mapper.Map<ForestDiseasePublicationsResponse>(forestDiseasePublications);
        }

        public async Task DeleteFile(int Id, int fileId)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var forestDiseasePublications = await _context.ForestDiseasePublications.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (forestDiseasePublications == null)
            {
                throw new ApiException($"找不到此資料-{Id}");
            }

            var fileList = JsonSerializer.Deserialize<List<ForestDiseasePublicationsFileDto>>(forestDiseasePublications.File);
            if (fileList!.Where(x => x.Id == fileId).Any())
            {
                var removeFile = fileList!.Where(_x => _x.Id == fileId).FirstOrDefault();
                if (removeFile != null)
                    fileList!.Remove(removeFile!);
            }

            if (fileList.Any())
            {
                var jsonResult = JsonSerializer.Serialize(fileList);
                forestDiseasePublications.File = jsonResult;
            }
            else
            {
                forestDiseasePublications.File = string.Empty;
            }
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.ForestDiseasePublications.Update(forestDiseasePublications);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"刪除出版品附件-{forestDiseasePublications.Name}/{fileId}",
                });
            }
            scope.Complete();
        }

        //public async Task<string> GetFile(string FileName)
        //{
        //    await using var _context = await _contextFactory.CreateDbContextAsync();

        //    var forestDiseasePublications = await _context.ForestDiseasePublications.Where(x => x.File.Contains(FileName)).FirstOrDefaultAsync();

        //    if (forestDiseasePublications == null)
        //    {
        //        throw new ApiException($"找不到此資料-{FileName}");
        //    }

        //    var fileList = JsonSerializer.Deserialize<List<string>>(forestDiseasePublications.File);

        //    if (fileList == null)
        //    {
        //        throw new ApiException($"未上傳或找不到檔案-{FileName}");
        //    }

        //    var fileUploadPath = _fileService.Value.GetFileUploadPath();

        //    return $"{fileUploadPath}/{fileList.Where(x => x == FileName).FirstOrDefault()!}";
        //}
    }
}
