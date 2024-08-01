using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.Data;
using CommonLibrary.DTOs.Common;
using CommonLibrary.DTOs.DamageType;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class DamageTypeService : IDamageTypeService
    {
        private readonly ILogger<DamageTypeService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IOperationLogService> _operationLogService;
        public DamageTypeService(ILogger<DamageTypeService> log, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService, IMapper mapper)
        {
            _log = log;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
            _mapper = mapper;
        }

        public async Task<List<DamageTypeResponse>> Get(int? Id = null)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var damageTypes = new List<DamageType>();

            if (Id.HasValue)
                damageTypes = await _context.DamageType.Where(x => x.Id == Id).ToListAsync();
            else
                damageTypes = await _context.DamageType.ToListAsync();

            return _mapper.Map<List<DamageTypeResponse>>(damageTypes);
        }

        public async Task<List<DamageTypeResponse>> Get(GetDamageTypeDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<DamageType> query = _context.DamageType.AsQueryable();

            var damageTypes = new List<DamageType>();


            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                string keyword = dto.Keyword.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(keyword) ||
                    x.Id.ToString().Contains(keyword)
                );
            }

            if (dto.Status.HasValue)
            {
                query = query.Where(x => x.Status == dto.Status);
            }


            damageTypes = await query.ToListAsync();

            return _mapper.Map<List<DamageTypeResponse>>(damageTypes);
        }

        public async Task<DamageTypeResponse> Add(AddDamageTypeDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var damageType = new DamageType
            {
                Name = dto.Name,
                Status = dto.Status,
                Sort = dto.Sort,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.DamageType.AddAsync(damageType);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增危害類型：{damageType.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<DamageTypeResponse>(damageType);
        }

        public async Task<DamageTypeResponse> Update(UpdateDamageTypeDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var damageType = await _context.DamageType.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (damageType == null)
            {
                throw new ApiException($"無此資料-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                damageType.Name = dto.Name;

            if (dto.Status.HasValue)
                damageType.Status = dto.Status.Value;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.DamageType.Update(damageType);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改危害類型：{damageType.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<DamageTypeResponse>(damageType);
        }

        public async Task<List<DamageTypeResponse>> UpdateSort(List<SortBasicDto> dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var result = new List<DamageTypeResponse>();

            foreach (var value in dto)
            {
                var damageType = await _context.DamageType.Where(x => x.Id == value.Id).FirstOrDefaultAsync();
                if (damageType == null) continue;

                if (value.Sort.HasValue)
                    damageType.Sort = value.Sort.Value;

                _context.DamageType.Update(damageType);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    await _operationLogService.Value.Add(new AddOperationLogDto
                    {
                        Type = ChangeTypeEnum.Edit,
                        Content = $"修改危害類型排序：{damageType.Name}/{damageType.Sort}",
                    });
                };

                result.Add(_mapper.Map<DamageTypeResponse>(damageType));
            }

            scope.Complete();
            return result;
        }

        public async Task<DamageTypeResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var damageType = await _context.DamageType.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (damageType == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.DamageType.Remove(damageType);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除危害類型：{damageType.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<DamageTypeResponse>(damageType);
        }
    }
}
