using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.Data;
using CommonLibrary.DTOs.ForestCompartmentLocation;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.DTOs.TreeBasicInfo;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using CommonLibrary.DTOs.Common;

namespace admin_backend.Services
{
    public class TreeBasicInfoService : ITreeBasicInfoService
    {
        private readonly ILogger<TreeBasicInfoService> _log;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IOperationLogService> _operationLogService;

        public TreeBasicInfoService(ILogger<TreeBasicInfoService> log, IMapper mapper, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService)
        {
            _log = log;
            _mapper = mapper;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
        }

        public async Task<List<TreeBasicInfoResponse>> Get(int? id = null)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var treeBasicInfo = new List<TreeBasicInfo>();

            if (id != null)
                treeBasicInfo = await _context.TreeBasicInfo.Where(x => x.Id == id).ToListAsync();

            else
                treeBasicInfo = await _context.TreeBasicInfo.ToListAsync();

            return _mapper.Map<List<TreeBasicInfoResponse>>(treeBasicInfo);

        }

        public async Task<List<TreeBasicInfoResponse>> Get(GetTreeBasicInfoDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            IQueryable<TreeBasicInfo> query = _context.TreeBasicInfo.AsQueryable();

            var treeBasicInfos = new List<TreeBasicInfo>();


            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                string keyword = dto.Keyword.ToLower();
                query = query.Where(x =>
                    x.ScientificName.ToLower().Contains(keyword) ||
                    x.Name.ToLower().Contains(keyword) ||
                    x.Id.ToString().Contains(keyword)
                );
            }

            treeBasicInfos = await query.ToListAsync();
            return _mapper.Map<List<TreeBasicInfoResponse>>(treeBasicInfos);
        }
        public async Task<TreeBasicInfoResponse> Add(AddTreeBasicInfoDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var treeBasicInfo = new TreeBasicInfo
            {
                ScientificName = dto.ScientificName,
                Name = dto.Name,
                Sort = dto.Sort,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.TreeBasicInfo.AddAsync(treeBasicInfo);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增樹木基本資料：{treeBasicInfo.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<TreeBasicInfoResponse>(treeBasicInfo);
        }

        public async Task<TreeBasicInfoResponse> Update(int Id, UpdateTreeBasicInfoDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var treeBasicInfo = await _context.TreeBasicInfo.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (treeBasicInfo == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            if (!string.IsNullOrEmpty(dto.ScientificName))
                treeBasicInfo.ScientificName = dto.ScientificName;

            if (!string.IsNullOrEmpty(dto.Name))
                treeBasicInfo.Name = dto.Name;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.TreeBasicInfo.Update(treeBasicInfo);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改樹木基本資料：{treeBasicInfo.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<TreeBasicInfoResponse>(treeBasicInfo);
        }

        public async Task<List<TreeBasicInfoResponse>> UpdateSort(List<SortBasicDto> dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var result = new List<TreeBasicInfoResponse>();

            foreach (var value in dto)
            {
                var treeBasicInfo = await _context.TreeBasicInfo.Where(x => x.Id == value.Id).FirstOrDefaultAsync();
                if (treeBasicInfo == null) continue;

                if (value.Sort.HasValue)
                    treeBasicInfo.Sort = value.Sort.Value;

                _context.TreeBasicInfo.Update(treeBasicInfo);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _operationLogService.Value.Add(new AddOperationLogDto
                    {
                        Type = ChangeTypeEnum.Edit,
                        Content = $"修改林班位置排序：{treeBasicInfo.Id}/{treeBasicInfo.Sort}",
                    });
                };

                result.Add(_mapper.Map<TreeBasicInfoResponse>(treeBasicInfo));
            }

            scope.Complete();
            return result;
        }

        public async Task<TreeBasicInfoResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();


            var treeBasicInfo = await _context.TreeBasicInfo.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (treeBasicInfo == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.TreeBasicInfo.Remove(treeBasicInfo);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除樹木基本資料：{treeBasicInfo.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<TreeBasicInfoResponse>(treeBasicInfo);
        }
    }
}
