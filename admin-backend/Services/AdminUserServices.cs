using CommonLibrary.Data;
using CommonLibrary.DTOs.AdminUser;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;

namespace admin_backend.Services
{
    public class AdminUserServices
    {
        private readonly MysqlDbContext _context;
        public AdminUserServices(MysqlDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdminUser>> Get(GetAdminUserDto dto)
        {
            IQueryable<AdminUser> query = _context.AdminUser.AsQueryable();


            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                string keyword = dto.Keyword.ToLower();
                query = query.Where(x =>
                    x.Account.ToLower().Contains(keyword) ||
                    x.Password.ToLower().Contains(keyword) ||
                    x.Email.ToLower().Contains(keyword) ||
                    x.Name.ToLower().Contains(keyword)
                );

            }

            if (dto.RoleId.HasValue)
            {
                query = query.Where(x => x.RoleId == dto.RoleId);
            }

            if (dto.Status.HasValue)
            {
                query = query.Where(x => x.Status == dto.Status);
            }

            return await query.ToListAsync();
        }

        public async Task<AdminUser> Add(AddAdminUserDto dto)
        {
            var adminUser = await _context.AdminUser.Where(x => x.Account == dto.Account).FirstOrDefaultAsync();

            if (adminUser != null)
            {
                throw new Exception($"此帳號已存在-{dto.Name}");
            }

            adminUser = new AdminUser
            {
                Name = dto.Name,
                Account = dto.Account,
                Password = dto.Password,
                Email = dto.Email,
                RoleId = dto.RoleId,
                Status = dto.Status,
                Photo = dto.Photo,
            };

            await _context.AdminUser.AddAsync(adminUser);
            await _context.SaveChangesAsync();

            return adminUser;
        }

        public async Task<AdminUser> Update(UpdateAdminUserDto dto)
        {
            var adminUser = await _context.AdminUser.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (adminUser == null)
            {
                throw new Exception($"無此資料-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                adminUser.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Email))
                adminUser.Email = dto.Email;

            if (dto.Status.HasValue)
                adminUser.Status = (StatusEnum)dto.Status;

            if (!string.IsNullOrEmpty(dto.Password))
                adminUser.Password = dto.Password;

            _context.AdminUser.Update(adminUser);

            await _context.SaveChangesAsync();

            return adminUser;
        }
    }
}
