using CommonLibrary.Data;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.DTOs.TreeBasicInfo;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class TreeBasicInfoService
    {
        private readonly ILogger<TreeBasicInfoService> _log;
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;

        public TreeBasicInfoService(ILogger<TreeBasicInfoService> log, MysqlDbContext context, OperationLogService operationLogService)
        {
            _log = log;
            _context = context;
            _operationLogService = operationLogService;
        }

        public async Task<List<TreeBasicInfo>> Get()
        {
            var treeBasicInfos = await _context.TreeBasicInfo.ToListAsync();
            return treeBasicInfos;
        }

        public async Task<TreeBasicInfo> Add(AddTreeBasicInfoDto dto)
        {

            var treeBasicInfo = new TreeBasicInfo
            {
                ScientificName = dto.ScientificName,
                Name = dto.Name,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.TreeBasicInfo.AddAsync(treeBasicInfo);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增樹木基本資料：{treeBasicInfo.Name}",
                });
            };
            scope.Complete();
            return treeBasicInfo;
        }

        public async Task<TreeBasicInfo> Update(UpdateTreeBasicInfoDto dto)
        {

            var treeBasicInfo = await _context.TreeBasicInfo.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (treeBasicInfo == null)
            {
                throw new ApiException($"無此資料-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.ScientificName))
                treeBasicInfo.ScientificName = dto.ScientificName;

            if (!string.IsNullOrEmpty(dto.Name))
                treeBasicInfo.Name = dto.Name;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.TreeBasicInfo.AddAsync(treeBasicInfo);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改樹木基本資料：{treeBasicInfo.Name}",
                });
            };
            scope.Complete();
            return treeBasicInfo;
        }

        public async Task<TreeBasicInfo> Delete(DeleteTreeBasicInfoDto dto)
        {

            var treeBasicInfo = await _context.TreeBasicInfo.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (treeBasicInfo == null)
            {
                throw new ApiException($"無此資料-{dto.Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.TreeBasicInfo.Remove(treeBasicInfo);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除樹木基本資料：{treeBasicInfo.Name}",
                });
            };
            scope.Complete();
            return treeBasicInfo;
        }
    }
}
