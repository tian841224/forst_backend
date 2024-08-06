using admin_backend.Data;
using admin_backend.DTOs.OperationLog;
using admin_backend.DTOs.Role;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _log;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly IRolePermissionService _rolePermissionService;

        public RoleService(Lazy<IOperationLogService> operationLogService, IMapper mapper, IDbContextFactory<MysqlDbContext> contextFactory, ILogger<RoleService> log, IRolePermissionService rolePermissionService)
        {
            _log = log;
            _mapper = mapper;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
            _rolePermissionService = rolePermissionService;
        }

        public async Task<List<RoleResponse>> Get(GetRoleDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<Role> query = _context.Role;

            if (dto.Id.HasValue)
            {
                query = query.Where(x => x.Id == dto.Id);
            }

            if (!string.IsNullOrEmpty(dto.Name))
            {
                query = query.Where(x => x.Name == dto.Name);
            }

            var pagedResult = query.GetPaged(dto);

            return _mapper.Map<List<RoleResponse>>(pagedResult.Items.OrderBy(x => dto.OrderBy));
        }

        public async Task<RoleResponse> Add(AddRoleDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var role = await _context.Role.Where(x => x.Name == dto.Name).FirstOrDefaultAsync();

            if (role != null)
            {
                throw new ApiException($"此角色已註冊-{dto.Name}");
            }

            role = new Role
            {
                Name = dto.Name,
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.Role.Add(role);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增角色：{role.Id}/{role.Name}",
                });
            }

            ////新增身分權限
            //dto.RolePermission.ForEach(item => item.RoleId = role.Id);
            //await _rolePermissionService.Add(dto.RolePermission);

            scope.Complete();

            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<RoleResponse> Update(int Id, UpdateRoleDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var role = await _context.Role.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                role.Name = dto.Name;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.Role.Update(role);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改角色：{role.Id}/{role.Name}",
                });
            }

            ////修改身分權限
            //dto.RolePermission.ForEach(item => item.RoleId = role.Id);
            //await _rolePermissionService.Update(dto.RolePermission);

            scope.Complete();

            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<RoleResponse> Delete(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var role = await _context.Role.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.Role.Remove(role);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除角色：{role.Id}/{role.Name}",
                });
            }

            ////移除身分權限
            //await _rolePermissionService.Delete(dto.RolePermission);

            scope.Complete();
            return _mapper.Map<RoleResponse>(role);
        }
    }
}
