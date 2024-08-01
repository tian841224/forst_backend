using AutoMapper;
using CommonLibrary.Data;
using CommonLibrary.DTOs.DamageClass;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class DamageClassService
    {
        private readonly ILogger<DamageClassService> _log;
        private readonly IMapper _mapper;
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;
        public DamageClassService(ILogger<DamageClassService> log, MysqlDbContext context, OperationLogService operationLogService, IMapper mapper)
        {
            _log = log;
            _context = context;
            _operationLogService = operationLogService;
            _mapper = mapper;
        }

        public async Task<List<DamageClassResponse>> Get(int? Id = null)
        {
            var damageClassList = new List<DamageClass>();

            if (Id.HasValue)
                damageClassList = await _context.DamageClass.Where(x => x.Id == Id).ToListAsync();
            else
                damageClassList = await _context.DamageClass.ToListAsync();

            return _mapper.Map<List<DamageClassResponse>>(damageClassList);
        }

        public async Task<List<DamageClassResponse>> Get(int TypeId)
        {
            var damageClassList = new List<DamageClass>();

            damageClassList = await _context.DamageClass.Where(x => x.DamageTypeId == TypeId).ToListAsync();

            return _mapper.Map<List<DamageClassResponse>>(damageClassList);
        }

        public async Task<DamageClassResponse> Add(AddDamageClassDto dto)
        {

            var damageClasses = new DamageClass 
            {
                Name = dto.Name,
                DamageTypeId = dto.DamageTypeId,
                Status = dto.Status,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.DamageClass.AddAsync(damageClasses);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增危害種類：{damageClasses.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<DamageClassResponse>(damageClasses);
        }

        public async Task<DamageClassResponse> Update(UpdateDamageClassDto dto)
        {

            var damageClasses = await _context.DamageClass.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (damageClasses == null)
            {
                throw new ApiException($"無此資料-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                damageClasses.Name = dto.Name;

            if (dto.DamageTypeId.HasValue)
                damageClasses.DamageTypeId = dto.DamageTypeId.Value;

            if (dto.Status.HasValue)
                damageClasses.Status = dto.Status.Value;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.DamageClass.Update(damageClasses);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改危害種類：{damageClasses.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<DamageClassResponse>(damageClasses);
        }

        public async Task<DamageClassResponse> Delete(int Id)
        {

            var damageClasses = await _context.DamageClass.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (damageClasses == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.DamageClass.Remove(damageClasses);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除危害種類：{damageClasses.Name}",
                });
            };
            scope.Complete();
            return _mapper.Map<DamageClassResponse>(damageClasses);
        }
    }
}
