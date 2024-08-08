using admin_backend.Data;
using admin_backend.DTOs.DamageType;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
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

        public async Task<PagedResult<DamageTypeResponse>> Get(int? Id = null, PagedOperationDto? dto = null)
        {
            if (dto == null) dto = new PagedOperationDto();
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<DamageType> damageTypes = _context.DamageType;

            if (Id.HasValue)
                damageTypes = _context.DamageType.Where(x => x.Id == Id);

            var damageTypesResponse = _mapper.Map<List<DamageTypeResponse>>(damageTypes);
            //分頁處理
            return damageTypesResponse.GetPaged(dto!);
        }

        public async Task<PagedResult<DamageTypeResponse>> Get(GetDamageTypeDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<DamageType> damageTypes = _context.DamageType;

            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                string keyword = dto.Keyword.ToLower();
                damageTypes = damageTypes.Where(x =>
                    x.Name.ToLower().Contains(keyword) ||
                    x.Id.ToString().Contains(keyword)
                );
            }

            if (dto.Status.HasValue)
            {
                damageTypes = damageTypes.Where(x => x.Status == dto.Status);
            }

            var damageTypesResponse = _mapper.Map<List<DamageTypeResponse>>(damageTypes);
            //分頁處理
            return damageTypesResponse.GetPaged(dto.Page!);
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

        public async Task<DamageTypeResponse> Update(int Id, UpdateDamageTypeDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var damageType = await _context.DamageType.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (damageType == null)
            {
                throw new ApiException($"無此資料-{Id}");
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
