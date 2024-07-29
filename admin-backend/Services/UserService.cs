using CommonLibrary.Data;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.DTOs.User;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace admin_backend.Services
{
    public class UserService
    {
        private readonly MysqlDbContext _context;
        private readonly OperationLogService _operationLogService;
        private readonly ILogger<UserService> _log;

        public UserService(MysqlDbContext context,OperationLogService operationLogService, ILogger<UserService> log)
        {
            _context = context;
            _operationLogService = operationLogService;
            _log = log;
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
                throw new ApiException($"此帳號已註冊-{dto.Account}");
            }

            user = new User
            {
                Account = dto.Account,
                Name = dto.Name,
                Status = StatusEnum.Open,
            };

            await _context.User.AddAsync(user);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增會員：{user.Id}/{user.Name}",
                });
            }

            return user;
        }

        public async Task<User> Update(UpdateUserDto dto)
        {
            var user = await _context.User.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ApiException($"無此使用者-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                user.Name = dto.Name;

            if (dto.Status.HasValue)
                user.Status = (StatusEnum)dto.Status;

            _context.User.Update(user);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改會員：{user.Id}/{user.Name}",
                });
            }

            return user;
        }
    }
}
