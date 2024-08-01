using admin_backend.Interfaces;
using AutoMapper;
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
    public class ForestCompartmentLocationService: IForestCompartmentLocationService
    {
        private readonly ILogger<ForestCompartmentLocationService> _log;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IOperationLogService> _operationLogService;

        public ForestCompartmentLocationService(ILogger<ForestCompartmentLocationService> log, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService, IMapper mapper)
        {
            _log = log;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
            _mapper = mapper;
        }

        public async Task<List<ForestCompartmentLocationResponse>> Get(int? Id = null)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var forestCompartmentLocation = new List<ForestCompartmentLocation>();
            if (Id.HasValue)
                forestCompartmentLocation = await _context.ForestCompartmentLocation.Where(x => x.Id == Id).ToListAsync();
            else
                forestCompartmentLocation = await _context.ForestCompartmentLocation.ToListAsync();

            return _mapper.Map<List<ForestCompartmentLocationResponse>>(forestCompartmentLocation);
        }

        public async Task<ForestCompartmentLocationResponse> Add(AddForestCompartmentLocationDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

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
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增林班位置：{forestCompartmentLocation.AffiliatedUnit}",
                });
            };
            scope.Complete();
            return _mapper.Map<ForestCompartmentLocationResponse>(forestCompartmentLocation);
        }

        public async Task<ForestCompartmentLocationResponse> Update(int Id, UpdateForestCompartmentLocationDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();


            var forestCompartmentLocation = await _context.ForestCompartmentLocation.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (forestCompartmentLocation == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            if (!string.IsNullOrEmpty(dto.Postion))
                forestCompartmentLocation.Postion = dto.Postion;

            if (!string.IsNullOrEmpty(dto.AffiliatedUnit))
                forestCompartmentLocation.AffiliatedUnit = dto.AffiliatedUnit;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.ForestCompartmentLocation.Update(forestCompartmentLocation);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改林班位置：{forestCompartmentLocation.AffiliatedUnit}",
                });
            };
            scope.Complete();
            return _mapper.Map<ForestCompartmentLocationResponse>(forestCompartmentLocation);
        }

        public async Task<ForestCompartmentLocationResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();


            var forestCompartmentLocation = await _context.ForestCompartmentLocation.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (forestCompartmentLocation == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.ForestCompartmentLocation.Remove(forestCompartmentLocation);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"移除林班位置：{forestCompartmentLocation.AffiliatedUnit}",
                });
            };
            scope.Complete();
            return _mapper.Map<ForestCompartmentLocationResponse>(forestCompartmentLocation);
        }
    }
}
