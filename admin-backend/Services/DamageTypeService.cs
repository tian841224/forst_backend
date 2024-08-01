using AutoMapper;
using CommonLibrary.Data;
using CommonLibrary.DTOs.DamageClass;
using CommonLibrary.DTOs.DamageType;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class DamageTypeService
    {
        private readonly ILogger<DamageTypeService> _log;
        private readonly IMapper _mapper;
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;
        public DamageTypeService(ILogger<DamageTypeService> log, MysqlDbContext context, OperationLogService operationLogService,IMapper mapper)
        {
            _log = log;
            _context = context;
            _operationLogService = operationLogService;
            _mapper = mapper;
        }

        public async Task<List<DamageTypeResponse>> Get(int? Id = null)
        {
            var damageTypes = new List<DamageType>();

            if (Id.HasValue)
                damageTypes = await _context.DamageType.Where(x => x.Id == Id).ToListAsync();
            else
                damageTypes = await _context.DamageType.ToListAsync();

            return _mapper.Map<List<DamageTypeResponse>>(damageTypes);
        }

        public async Task<DamageTypeResponse> Add(AddDamageTypeDto dto)
        {
            var damageType = new DamageType
            {
                Name = dto.Name,
                Status = dto.Status,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.DamageType.AddAsync(damageType);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
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
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改危害類型：{damageType.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<DamageTypeResponse>(damageType);
        }

        public async Task<DamageTypeResponse> Delete(int Id)
        {

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
                await _operationLogService.Add(new AddOperationLogDto
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
