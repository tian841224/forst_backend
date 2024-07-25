using CommonLibrary.Data;
using CommonLibrary.DTOs.Role;
using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace admin_backend.Services
{
    public class RoleServices
    {
        private readonly MysqlDbContext _context;
        public RoleServices(MysqlDbContext context)
        {
            _context = context;
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
                throw new Exception($"此身分已存在-{dto.Name}");
            }

            role = new Role
            {
                Name = dto.Name,
            };

            await _context.Role.AddAsync(role);

            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<Role> Update(UpdateRoleDto dto)
        {
            var role = await _context.Role.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new Exception($"無此資料-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                role.Name = dto.Name;

            _context.Role.Update(role);

            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<Role> Delete(DeleteRoleDto dto)
        {
            var role = await _context.Role.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new Exception($"無此資料-{dto.Id}");
            }

            _context.Role.Remove(role);

            await _context.SaveChangesAsync();

            return role;
        }
    }
}
