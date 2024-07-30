using CommonLibrary.Data;
using CommonLibrary.DTOs.ForestCompartmentLocation;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class ForestCompartmentLocationService
    {
        private readonly ILogger<ForestCompartmentLocationService> _log;
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;

        public ForestCompartmentLocationService(ILogger<ForestCompartmentLocationService> log, MysqlDbContext context, OperationLogService operationLogService)
        {
            _log = log;
            _context = context;
            _operationLogService = operationLogService;
        }

        public async Task<List<ForestCompartmentLocation>> Get()
        {
            var forestCompartmentLocations = await _context.ForestCompartmentLocation.ToListAsync();
            return forestCompartmentLocations;
        }

        public async Task<ForestCompartmentLocation> Add(AddForestCompartmentLocationDto dto)
        {

            var forestCompartmentLocation = new ForestCompartmentLocation
            {
                Postion = dto.Postion,
                AffiliatedUnit = dto.AffiliatedUnit,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.ForestCompartmentLocation.AddAsync(forestCompartmentLocation);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增林班位置：{forestCompartmentLocation.AffiliatedUnit}",
                });
            };
            scope.Complete();
            return forestCompartmentLocation;
        }

        public async Task<ForestCompartmentLocation> Update(UpdateForestCompartmentLocationDto dto)
        {

            var forestCompartmentLocation = await _context.ForestCompartmentLocation.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (forestCompartmentLocation == null)
            {
                throw new ApiException($"無此資料-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.Postion))
                forestCompartmentLocation.Postion = dto.Postion;

            if (!string.IsNullOrEmpty(dto.AffiliatedUnit))
                forestCompartmentLocation.AffiliatedUnit = dto.AffiliatedUnit;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.ForestCompartmentLocation.AddAsync(forestCompartmentLocation);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"修改林班位置：{forestCompartmentLocation.AffiliatedUnit}",
                });
            };
            scope.Complete();
            return forestCompartmentLocation;
        }

        public async Task<ForestCompartmentLocation> Delete(DeleteForestCompartmentLocationDto dto)
        {

            var forestCompartmentLocation = await _context.ForestCompartmentLocation.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (forestCompartmentLocation == null)
            {
                throw new ApiException($"無此資料-{dto.Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.ForestCompartmentLocation.Remove(forestCompartmentLocation);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"移除林班位置：{forestCompartmentLocation.AffiliatedUnit}",
                });
            };
            scope.Complete();
            return forestCompartmentLocation;
        }
    }
}
