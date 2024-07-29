using CommonLibrary.Data;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.DTOs.Role;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using CommonLibrary.Service;
using Microsoft.EntityFrameworkCore;

namespace admin_backend.Services
{
    public class RoleServices
    {
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;
        public RoleServices(MysqlDbContext context,OperationLogService operationLogService)
        {
            _context = context;
            _operationLogService = operationLogService;
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
                throw new ApiException($"此身分已存在-{dto.Name}");
            }

            role = new Role
            {
                Name = dto.Name,
            };

            await _context.Role.AddAsync(role);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增角色：{role.Id}/{role.Name}",
                });
            }

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

            return role;
        }

        public async Task<Role> Delete(DeleteRoleDto dto)
        {
            var role = await _context.Role.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new ApiException($"無此資料-{dto.Id}");
            }

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

            return role;
        }
    }
}
