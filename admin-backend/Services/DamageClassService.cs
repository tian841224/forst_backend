using admin_backend.Data;
using admin_backend.DTOs.DamageClass;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;


namespace admin_backend.Services
{
    public class DamageClassService : IDamageClassService
    {
        private readonly ILogger<DamageClassService> _log;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly IOperationLogService _operationLogService;
        private readonly IFileService _fileService;

        public DamageClassService(ILogger<DamageClassService> log, IDbContextFactory<MysqlDbContext> contextFactory, IOperationLogService operationLogService, IMapper mapper, IFileService fileService)
        {
            _log = log;
            _contextFactory = contextFactory;
            _mapper = mapper;
            _operationLogService = operationLogService;
            _fileService = fileService;
        }

        public async Task<PagedResult<DamageClassResponse>> Get(int? Id = null, PagedOperationDto? dto = null)
        {
            if (dto == null) dto = new PagedOperationDto();
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<DamageClass> damageClassList = _context.DamageClass;

            if (Id.HasValue)
                damageClassList = _context.DamageClass.Where(x => x.Id == Id);

            var damageClassListResponse = _mapper.Map<List<DamageClassResponse>>(damageClassList);
            //分頁處理
            return damageClassListResponse.GetPaged(dto!);
        }

        public async Task<PagedResult<DamageClassResponse>> Get(GetDamageClassDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<DamageClass> damageClassList = _context.DamageClass;

            damageClassList = _context.DamageClass.Where(x => x.DamageTypeId == dto.TypeId);

            var damageClassResponse = _mapper.Map<List<DamageClassResponse>>(damageClassList);

            //分頁處理
            return damageClassResponse.GetPaged(dto.Page!);
        }

        public async Task<DamageClassResponse> Add(AddDamageClassDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var damageClasses = await _context.DamageClass.Where(x => x.Name == dto.Name).FirstOrDefaultAsync();

            if (damageClasses != null)
            {
                throw new ApiException($"此名稱已存在-{dto.Name}");
            }

            damageClasses = new DamageClass
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

        public async Task<DamageClassResponse> Update(int Id, UpdateDamageClassDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var damageClasses = await _context.DamageClass.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (damageClasses == null)
            {
                throw new ApiException($"無此資料-{Id}");
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
            await using var _context = await _contextFactory.CreateDbContextAsync();

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
