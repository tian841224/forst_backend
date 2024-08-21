using admin_backend.Data;
using admin_backend.DTOs.AdminUser;
using admin_backend.DTOs.OperationLog;
using admin_backend.DTOs.User;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace admin_backend.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _log;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IOperationLogService> _operationLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailConfigService _mailConfigService;
        private readonly IEmailService _emailService;

        public UserService(ILogger<UserService> log, IMapper mapper, IDbContextFactory<MysqlDbContext> contextFactory, Lazy<IOperationLogService> operationLogService, IHttpContextAccessor httpContextAccessor, IMailConfigService mailConfigService, IEmailService emailService)
        {
            _log = log;
            _mapper = mapper;
            _contextFactory = contextFactory;
            _operationLogService = operationLogService;
            _httpContextAccessor = httpContextAccessor;
            _mailConfigService = mailConfigService;
            _emailService = emailService;
        }


        public async Task<UserResponse> Get(int Id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == Id);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<PagedResult<UserResponse>> Get(GetUserDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<User> user = _context.User;

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

            var userResponse = _mapper.Map<List<UserResponse>>(user);
            return userResponse.GetPaged(dto.Page!);
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
                Password = dto.PKey,
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

            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == Id);

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

            if (!string.IsNullOrEmpty(dto.OldKey) && !string.IsNullOrEmpty(dto.pKey))
            {

                //取得IP
                var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress ?? new System.Net.IPAddress(0);

                if (user.Password != dto.OldKey)
                {
                    //新增操作紀錄
                    await _context.OperationLog.AddAsync(new OperationLog
                    {
                        UserId = user.Id,
                        Type = ChangeTypeEnum.Edit,
                        Content = $"修改前台帳號密碼-輸入錯誤:{user.Name}/{user.Account}",
                        Ip = ipAddress.ToString(),
                    });
                    throw new ApiException($"原密碼輸入錯誤-{Id}");
                }

                user.Password = dto.pKey;
            }

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

        public async Task ResetPassword(ResetPasswordDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var user = await _context.User.Where(x => x.Account == dto.Email).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ApiException($"無此使用者-{dto.Email}");
            }

            //修改密碼
            char[] AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            var password = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                var buffer = new byte[8];

                rng.GetBytes(buffer);

                for (int i = 0; i < 8; i++)
                {
                    var index = buffer[i] % AllowedChars.Length;
                    password.Append(AllowedChars[index]);
                }
            }

            await Update(user.Id, new UpdateUserDto
            {
                OldKey = user.Password,
                pKey = password.ToString()
            });

            //取得寄件設定
            var emailConfig = await _mailConfigService.Get();

            //發送新密碼
            await _emailService.SendEmail(new SendEmailDto
            {
                Host = emailConfig.Host,
                Port = emailConfig.Port,
                Account = emailConfig.Account,
                Password = emailConfig.Pkey,
                EnableSsl = emailConfig.Encrypted == EncryptedEnum.SSL,
                MailMessage = new MailMessage
                {
                    From = new MailAddress(emailConfig.Account, emailConfig.Name),
                    Subject = "重置密碼信件",
                    Body = $"新密碼：{password.ToString()}，請妥善保管",
                    IsBodyHtml = true,
                },
                Recipient = dto.Email,
            });
        }
    }
}