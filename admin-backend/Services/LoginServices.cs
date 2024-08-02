using admin_backend.Interfaces;
using CommonLibrary.Data;
using CommonLibrary.DTOs.Common;
using CommonLibrary.DTOs.Login;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using CommonLibrary.Interface;
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
        private const string ADMIN_USER_REFRESH_TOKEN_KEY_PRE = "ADMIN_USER_REFRESH_TOKEN_KEY_PRE:";
        private const string ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE = "ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE:";
        private const string CAPTCHA_CODE_PRE = "CAPTCHA_CODE_PRE:";


        public LoginServices(IDbContextFactory<MysqlDbContext> contextFactory, IOptions<JwtConfig> jwtConfig, ILogger<LoginServices> log, Lazy<IIdentityService> identityService)
        {
            _jwtConfig = jwtConfig.Value;
            _log = log;
            _contextFactory = contextFactory;
            _identityService = identityService;
        }

        public async Task<IdentityResultDto> Login(LoginDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var adminUser = await _context.AdminUser.Where(x => x.Account == dto.Account && x.Password == dto.pKey).FirstOrDefaultAsync();

            if (adminUser == null)
            {
                throw new ApiException("帳號密碼錯誤");
            }

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
                Id = adminUser.Id,
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
                adminUser.LoginTime = DateTime.UtcNow;
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

                return new IdentityResultDto
                {
                    AccessToken = token,
                    //RefreshToken = refreshToken,
                    Expires = (new DateTimeOffset(_jwtConfig.Expiration)).ToUnixTimeSeconds(),
                    RoleId = role.Id,
                    Account = adminUser.Account,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _log.LogError(ex.Message);
                throw;
            }
        }

        //public async Task ResetPassword(ResetPasswordDto dto)
        //{

        //}

        //public async Task<CaptchaDto> GetCaptchaAsync()
        //{
        //    return await _identityService.GetCaptchaAsync();
        //}
    }
}