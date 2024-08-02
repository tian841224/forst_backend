using admin_backend.Data;
using admin_backend.DTOs.OperationLog;
using admin_backend.DTOs.User;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Transactions;

namespace admin_backend.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _log;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IOperationLogService> _operationLogService;

        public UserService(ILogger<UserService> log, IMapper mapper, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService)
        {
            _log = log;
            _mapper = mapper;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
        }

        public async Task<List<UserResponse>> Get(GetUserDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<User> user = _context.User;

            if (dto.Id.HasValue)
            {
                user = user.Where(x => x.Id == dto.Id);
            }

            if (!string.IsNullOrEmpty(dto.Account))
            {
                user = user.Where(x => x.Account == dto.Account);
            }

            if (!string.IsNullOrEmpty(dto.Name))
            {
                user = user.Where(x => x.Name == dto.Name);
            }

            if (dto.Status.HasValue)
            {
                user = user.Where(x => x.Status == dto.Status);
            }

            if (dto.LoginTime.HasValue)
            {
                user = user.Where(x => x.LoginTime == dto.LoginTime);
            }

            var pagedResult = await user.GetPagedAsync(dto);

            return _mapper.Map<List<UserResponse>>(pagedResult.Items.OrderBy(x => dto.OrderBy));
        }

        public async Task<UserResponse> Add(AddUserDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

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

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await _context.User.AddAsync(user);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Add,
                    Content = $"新增會員：{user.Id}/{user.Name}",
                });
            }
            scope.Complete();
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> Update(int Id, UpdateUserDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var user = await _context.User.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ApiException($"無此使用者-{Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                user.Name = dto.Name;

            if (dto.Status.HasValue)
                user.Status = (StatusEnum)dto.Status;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _context.User.Update(user);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                await _operationLogService.Value.Add(new AddOperationLogDto
                {
                    Type = ChangeTypeEnum.Edit,
                    Content = $"修改會員：{user.Id}/{user.Name}",
                });
            }
            scope.Complete();
            return _mapper.Map<UserResponse>(user);
        }
    }
}