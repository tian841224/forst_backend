using CommonLibrary.Data;
using CommonLibrary.DTOs;
using CommonLibrary.DTOs.Login;
using CommonLibrary.Service;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkiaSharp;
using StackExchange.Redis;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace admin_backend.Services
{
    public class LoginServices
    {
        private readonly ILogger<LoginServices> _log;
        private readonly JwtConfig _jwtConfig;
        private readonly MysqlDbContext _context;
        private readonly RedisService _redisService;
        private const string ADMIN_USER_REFRESH_TOKEN_KEY_PRE = "ADMIN_USER_REFRESH_TOKEN_KEY_PRE:";
        private const string ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE = "ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE:";
        private const string CAPTCHA_CODE_PRE = "CAPTCHA_CODE_PRE:";
        public IDatabase redisDb => _redisService.redisDb;


        public LoginServices(RedisService redisService, MysqlDbContext context, IOptions<JwtConfig> jwtConfig, ILogger<LoginServices> log)
        {
            _jwtConfig = jwtConfig.Value;
            _log = log;
            _context = context;
            _redisService = redisService;
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

            var refreshToken = Guid.NewGuid().ToString();

            //取得Token
            var token = await GenerateToken(new GenerateTokenDto
            {
                Id = adminUser.Id,
                Account = adminUser.Account,
                Name = adminUser.Name,
                Email = adminUser.Email,
                Role = role.Name,
                RefreshToken = refreshToken,
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

        public async Task<string> GenerateToken(GenerateTokenDto dto)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Role,dto.Role),
                    new Claim(JwtClaimTypes.Name,dto.Name),
                    new Claim(JwtClaimTypes.Email,dto.Email),
                    new Claim(JwtClaimTypes.ReferenceTokenId,dto.RefreshToken),
                    new Claim("Account",dto.Account),
                    new Claim("Id",dto.Id.ToString()),
                };

                var securityToken = new JwtSecurityToken(
                    issuer: _jwtConfig.Issuer,
                    audience: _jwtConfig.Audience,
                    claims: claims,
                    notBefore: _jwtConfig.NotBefore,
                    expires: _jwtConfig.Expiration,
                    signingCredentials: _jwtConfig.SigningCredentials
                );

                var access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);

                //設定RefreshToken
                var adminKey = ADMIN_USER_REFRESH_TOKEN_KEY_PRE + dto.Id;
                await redisDb.SetAddAsync(adminKey, dto.RefreshToken);

                var adminUserIdKey = ADMIN_USER_ID_FROM_REFRESH_TOKEN_PRE + dto.RefreshToken;
                await redisDb.SetAddAsync(adminUserIdKey, dto.Id);

                return access_token;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        public async Task<CaptchaDto> GetCaptchaAsync()
        {
            // 生成驗證碼
            string code = GenerateCode();


            var Captcha = GenerateCaptcha();

            var result = new CaptchaDto
            {
                CaptchaCode = Guid.NewGuid().ToString("N").ToUpper(),
                ImageBase64 = "data:image/png;base64," + Captcha.CaptchaBase64Data
            };

            await redisDb.StringSetAsync(CAPTCHA_CODE_PRE + result.CaptchaCode, Captcha.CaptchaCode);

            return result;
        }

        public CaptchaResult GenerateCaptcha(int width = 100, int height = 36)
        {
            // 生成驗證碼
            string code = GenerateCode();

            var image = new Bitmap(width, height);
            var graphics = Graphics.FromImage(image);
            var _random = new Random();

            // 背景顏色
            int num5 = 180;
            int num6 = 255;
            int num7 = _random.Next(num5, num6);
            int num8 = _random.Next(num5, num6);
            int num9 = _random.Next(num5, num6);
            graphics.Clear(Color.FromArgb(num7, num8, num9));

            // 畫驗證碼字串
            var font = new Font("Arial", 15, FontStyle.Bold | FontStyle.Italic);
            var brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Gray, 1.2f, true);

            int margin = 10; // 邊距，確保文字不靠近邊緣

            // 計算單個字符的寬高
            SizeF textSize = graphics.MeasureString(code[0].ToString(), font);
            float charWidth = textSize.Width;
            float charHeight = textSize.Height;

            for (int i = 0; i < code.Length; i++)
            {
                // 隨機生成文字位置，確保文字不靠近圖片邊緣
                int x = _random.Next(margin, width - margin - (int)charWidth);
                int y = _random.Next(margin, height - margin - (int)charHeight);
                graphics.DrawString(code[i].ToString(), font, brush, x, y);
            }

            // 畫干擾點
            for (int i = 0; i < 100; i++)
            {
                int x = _random.Next(image.Width);
                int y = _random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(_random.Next()));
            }

            graphics.Dispose();

            using var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin); // 確保 MemoryStream 的指針在開頭

            return new CaptchaResult
            {
                CaptchaCode = code,
                CaptchaByteData = ms.ToArray(),
                Timestamp = DateTime.UtcNow
            };
        }

        private static string GenerateCode(int length = 4)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var code = new char[length];
            for (int i = 0; i < length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }
            return new string(code);
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

            var token = await GenerateToken(new GenerateTokenDto
            {
                Id = adminUser.Id,
                Account = adminUser.Account,
                Name = adminUser.Name,
                Email = adminUser.Email,
                Role = role.Name,
                RefreshToken = refreshToken,
            });

            return token;
        }
    }
}