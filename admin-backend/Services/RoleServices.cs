using CommonLibrary.Data;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.DTOs.Role;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace admin_backend.Services
{
    public class RoleServices
    {
        private readonly ILogger<RoleServices> _log;
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;
        private readonly RolePermissionService _rolePermissionService;

        public RoleServices(MysqlDbContext context, OperationLogService operationLogService, ILogger<RoleServices> log, RolePermissionService rolePermissionService)
        {
            _context = context;
            _operationLogService = operationLogService;
            _log = log;
            _rolePermissionService = rolePermissionService;
        }

        public async Task<List<Role>> Get(GetRoleDto dto)
        {
            IQueryable<Role> query = _context.Role.AsQueryable();

            if (dto.Id.HasValue)
            {
                query = query.Where(x => x.Id == dto.Id);
            }

            if (!string.IsNullOrEmpty(dto.Name))
            {
                query = query.Where(x => x.Name == dto.Name);
            }

            return await query.ToListAsync();
        }

        public async Task<Role> Add(AddRoleDto dto)
        {
            var role = await _context.Role.Where(x => x.Name == dto.Name).FirstOrDefaultAsync();

            if (role != null)
            {
                return role;
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
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改角色：{role.Id}/{role.Name}",
                });
            }

            ////新增身分權限
            //dto.RolePermission.ForEach(item => item.RoleId = role.Id);
            //await _rolePermissionService.Add(dto.RolePermission);

            scope.Complete();

            return role;
        }

        public async Task<Role> Update(UpdateRoleDto dto)
        {
            var role = await _context.Role.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new ApiException($"無此資料-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                role.Name = dto.Name;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.Role.Update(role);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改角色：{role.Id}/{role.Name}",
                });
            }

            ////修改身分權限
            //dto.RolePermission.ForEach(item => item.RoleId = role.Id);
            //await _rolePermissionService.Update(dto.RolePermission);

            scope.Complete();

            return role;
        }

        public async Task<Role> Delete(DeleteRoleDto dto)
        {
            var role = await _context.Role.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new ApiException($"無此資料-{dto.Id}");
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.Role.Remove(role);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Delete,
                    Content = $"刪除角色：{role.Id}/{role.Name}",
                });
            }

            ////移除身分權限
            //await _rolePermissionService.Delete(dto.RolePermission);

            scope.Complete();
            return role;
        }
    }
}
