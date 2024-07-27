using CommonLibrary.Data;
using CommonLibrary.DTOs;
using CommonLibrary.DTOs.Login;
using CommonLibrary.Service;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Data;
using System.Security.Claims;

namespace admin_backend.Services
{
    public class LoginServices
    {
        private readonly ILogger<LoginServices> _log;
        private readonly JwtConfig _jwtConfig;
        private readonly MysqlDbContext _context;
        private readonly RedisService _redisService;
        private readonly IdentityService _identityService;
        private const string ADMIN_USER_REFRESH_TOKEN_KEY_PRE = "ADMIN_USER_REFRESH_TOKEN_KEY_PRE:";
        private const string ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE = "ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE:";
        private const string CAPTCHA_CODE_PRE = "CAPTCHA_CODE_PRE:";
        public IDatabase redisDb => _redisService.redisDb;


        public LoginServices(RedisService redisService, MysqlDbContext context, IOptions<JwtConfig> jwtConfig, ILogger<LoginServices> log, IdentityService identityService)
        {
            _jwtConfig = jwtConfig.Value;
            _log = log;
            _context = context;
            _redisService = redisService;
            _identityService = identityService;
        }

        public async Task<IdentityResultDto> Login(LoginDto dto)
        {
            var adminUser = await _context.AdminUser.Where(x => x.Account == dto.Account && x.Password == dto.Password).FirstOrDefaultAsync();

            if (adminUser == null)
            {
                throw new Exception("帳號密碼錯誤");
            }

            //取得身分
            var role = await _context.Role.Where(x => x.Id == adminUser.RoleId).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new Exception("請先設定此帳號身分");
            }

            //驗證驗證碼
            await _identityService.VerifyCaptchaAsync(dto.CaptchaCode,dto.CaptchaUserInput);

            var refreshToken = Guid.NewGuid().ToString();

            //取得Token
            var token = await _identityService.GenerateToken(new GenerateTokenDto
            {
                Id = adminUser.Id,
                RefreshToken = refreshToken,
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Role,role.Name),
                    new Claim(JwtClaimTypes.Name,adminUser.Name),
                    new Claim(JwtClaimTypes.Email,adminUser.Email),
                    new Claim(JwtClaimTypes.ReferenceTokenId,refreshToken),
                    new Claim("Account",adminUser.Account),
                    new Claim("Id",adminUser.Id.ToString()),
                }
            });

            //更新登入時間
            adminUser.LoginTime = DateTime.UtcNow;
            _context.AdminUser.Update(adminUser);
            await _context.SaveChangesAsync();

            return new IdentityResultDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                Expires = (new DateTimeOffset(_jwtConfig.Expiration)).ToUnixTimeSeconds(),
                RoleId = role.Id,
                Account = adminUser.Account,
            };
        }


        public async Task<CaptchaDto> GetCaptchaAsync()
        {
            return await _identityService.GetCaptchaAsync();
        }

        public async Task<string> RefreshAdminUserTokenAsync(RefreshTokenDto dto)
        {

            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                throw new Exception("請輸入RefreshTokenn");
            }

            var adminKey = ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE + dto.RefreshToken;

            var id = await redisDb.StringGetAsync(adminKey);

            if (id == RedisValue.Null)
            {
                throw new Exception("更新Token不合法");
            }

            var adminUser = await _context.AdminUser.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (adminUser == null)
            {
                throw new Exception("帳號不存在");
            }

            //取得身分
            var role = await _context.Role.Where(x => x.Id == adminUser.RoleId).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new Exception("請先設定此帳號身分");
            }

            var refreshToken = Guid.NewGuid().ToString();


            //取得Token
            var token = await _identityService.GenerateToken(new GenerateTokenDto
            {
                Id = adminUser.Id,
                RefreshToken = refreshToken,
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Role,role.Name),
                    new Claim(JwtClaimTypes.Name,adminUser.Name),
                    new Claim(JwtClaimTypes.Email,adminUser.Email),
                    new Claim(JwtClaimTypes.ReferenceTokenId,refreshToken),
                    new Claim("Account",adminUser.Account),
                    new Claim("Id",adminUser.Id.ToString()),
                }
            });

            return token;
        }
    }
}