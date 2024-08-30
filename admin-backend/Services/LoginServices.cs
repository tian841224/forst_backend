using admin_backend.Data;
using admin_backend.DTOs.Login;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace admin_backend.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly ILogger<LoginServices> _log;
        private readonly JwtConfig _jwtConfig;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly Lazy<IIdentityService> _identityService;
        private readonly IFileService _fileService;
        private readonly IAdminUserServices _adminUserServices;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IMailConfigService _mailConfigService;

        public LoginServices(IDbContextFactory<MysqlDbContext> contextFactory, IOptions<JwtConfig> jwtConfig, ILogger<LoginServices> log, Lazy<IIdentityService> identityService, IFileService fileService, IAdminUserServices adminUserServices, IEmailService emailService, IMailConfigService mailConfigService, IUserService userService)
        {
            _jwtConfig = jwtConfig.Value;
            _log = log;
            _contextFactory = contextFactory;
            _identityService = identityService;
            _fileService = fileService;
            _adminUserServices = adminUserServices;
            _userService = userService;
            _emailService = emailService;
            _mailConfigService = mailConfigService;
        }

        public async Task<IdentityResultDto> LoginBackEnd(LoginDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var adminUser = await _context.AdminUser.Where(x => x.Account == dto.Account && x.Password == dto.pKey).FirstOrDefaultAsync();

            if (adminUser == null)
            {
                throw new ApiException("帳號密碼錯誤");
            }

            if (adminUser.Status == StatusEnum.Close || adminUser.Status == StatusEnum.Stop)
                throw new ApiException("帳號已停用");

            //取得身分
            var role = await _context.Role.Where(x => x.Id == adminUser.RoleId).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new ApiException("請先設定此帳號身分");
            }

            //驗證驗證碼
            //await _identityService.VerifyCaptchaAsync(dto.CaptchaCode, dto.CaptchaUserInput);

            //var refreshToken = Guid.NewGuid().ToString();

            //取得Token
            var token = _identityService.Value.GenerateToken(new GenerateTokenDto
            {
                Id = adminUser.Id.ToString(),
                //RefreshToken = refreshToken,
                Claims = new ClaimDto
                {
                    RoleId = role.Id.ToString(),
                    RoleNane = role.Name,
                    UserNane = adminUser.Name,
                    Account = adminUser.Account,
                    Email = adminUser.Email,
                }
            });

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //更新登入時間
                adminUser.LoginTime = DateTime.Now;
                _context.AdminUser.Update(adminUser);
                await _context.SaveChangesAsync();

                //新增操作紀錄
                await _context.OperationLog.AddAsync(new OperationLog
                {
                    AdminUserId = adminUser.Id,
                    Type = ChangeTypeEnum.None,
                    Content = $"登入：{adminUser.Id}/{adminUser.Name}",
                });

                await transaction.CommitAsync();

                // 照片處理
                if (!string.IsNullOrEmpty(adminUser.Photo))
                {
                    adminUser.Photo = _fileService.GetFile(adminUser.Photo, "image");
                }

                return new IdentityResultDto
                {
                    Id = adminUser.Id,
                    AccessToken = token,
                    //RefreshToken = refreshToken,
                    Expires = (new DateTimeOffset(_jwtConfig.Expiration)).ToUnixTimeSeconds(),
                    RoleId = role.Id,
                    Account = adminUser.Account,
                    Photo = adminUser.Photo,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IdentityResultDto> LoginFrontEnd(LoginFrontEndDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var user = await _context.User.Where(x => x.Account == dto.Account && x.Password == dto.PKey).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ApiException("帳號密碼錯誤");
            }

            if(user.Status == StatusEnum.Close || user.Status == StatusEnum.Stop)
                throw new ApiException("會員已停用");

            //取得Token
            var token = _identityService.Value.GenerateToken(new GenerateTokenDto
            {
                Id = user.Id.ToString(),
                //RefreshToken = refreshToken,
                Claims = new ClaimDto
                {
                    UserNane = user.Name,
                    Account = user.Account,
                }
            });

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //更新登入時間
                user.LoginTime = DateTime.Now;
                _context.User.Update(user);
                await _context.SaveChangesAsync();

                //新增操作紀錄
                await _context.OperationLog.AddAsync(new OperationLog
                {
                    UserId = user.Id,
                    Type = ChangeTypeEnum.None,
                    Content = $"登入：{user.Id}/{user.Name}",
                });

                await transaction.CommitAsync();

                return new IdentityResultDto
                {
                    Id = user.Id,
                    AccessToken = token,
                    Expires = (new DateTimeOffset(_jwtConfig.Expiration)).ToUnixTimeSeconds(),
                    Account = user.Account,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _log.LogError(ex.Message);
                throw;
            }
        }

    }
}