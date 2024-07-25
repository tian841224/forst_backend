using CommonLibrary.Data;
using CommonLibrary.DTOs.User;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using Microsoft.EntityFrameworkCore;

namespace admin_backend.Services
{
    public class UserService
    {
        private readonly MysqlDbContext _context;
        public UserService(MysqlDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> Get(GetUserDto dto)
        {
            IQueryable<User> query = _context.User.AsQueryable();

            if (dto.Id.HasValue)
            {
                query = query.Where(x => x.Id == dto.Id);
            }

            if (!string.IsNullOrEmpty(dto.Account))
            {
                query = query.Where(x => x.Account == dto.Account);
            }

            if (!string.IsNullOrEmpty(dto.Name))
            {
                query = query.Where(x => x.Name == dto.Name);
            }

            if (dto.Status.HasValue)
            {
                query = query.Where(x => x.Status == dto.Status);
            }

            if (dto.LoginTime.HasValue)
            {
                query = query.Where(x => x.LoginTime == dto.LoginTime);
            }

            return await query.ToListAsync();
        }

        public async Task<User> Add(AddUserDto dto)
        {
            var user = await _context.User.Where(x => x.Account == dto.Account).FirstOrDefaultAsync();

            if (user != null)
            {
                throw new Exception($"此帳號已註冊-{dto.Account}");
            }

            user = new User
            {
                Account = dto.Account,
                Name = dto.Name,
                Status = StatusEnum.Open,
            };

            await _context.User.AddAsync(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Update(UpdateUserDto dto)
        {
            var user = await _context.User.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception($"無此使用者-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                user.Name = dto.Name;

            if (dto.Status.HasValue)
                user.Status = (StatusEnum)dto.Status;

            _context.User.Update(user);

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
