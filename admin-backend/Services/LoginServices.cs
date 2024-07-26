using CommonLibrary.Data;
using CommonLibrary.DTOs;
using CommonLibrary.DTOs.Login;
using CommonLibrary.Service;
using Google.Protobuf.WellKnownTypes;
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

            //驗證驗證碼
            VerifyCaptchaAsync(dto.CaptchaCode);

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

            await redisDb.StringSetAsync(CAPTCHA_CODE_PRE + result.CaptchaCode, Captcha.CaptchaCode,new TimeSpan(0,1,0));

            return result;
        }

        public CaptchaResult GenerateCaptcha(int width = 100, int height = 36)
        {
            try
            {
                string captchaText = GenerateCode();

                using (Bitmap bitmap = new Bitmap(width, height))
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.Clear(Color.White);

                    // 添加背景噪點
                    //AddNoise(graphics, width, height);

                    // 為每個字符創建單獨的路徑
                    for (int i = 0; i < captchaText.Length; i++)
                    {
                        using (GraphicsPath charPath = new GraphicsPath())
                        {
                            float fontSize = height * 0.7f; // 調整字體大小
                            using (Font font = new Font("Arial", fontSize, FontStyle.Bold))
                            {
                                float charWidth = width / captchaText.Length;
                                float x = i * charWidth + (charWidth - graphics.MeasureString(captchaText[i].ToString(), font).Width) / 2;
                                float y = (height - graphics.MeasureString(captchaText[i].ToString(), font).Height) / 2;

                                charPath.AddString(
                                    captchaText[i].ToString(),
                                    font.FontFamily,
                                    (int)font.Style,
                                    graphics.DpiY * fontSize / 72,
                                    new PointF(x, y),
                                    new StringFormat());

                                // 使用隨機顏色繪製字符
                                using (Brush brush = new SolidBrush(GetRandomColor()))
                                {
                                    graphics.FillPath(brush, charPath);
                                }
                            }
                        }
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        return new CaptchaResult
                        {
                            CaptchaCode = captchaText,
                            CaptchaByteData = ms.ToArray()
                        };
                    }
                }
            }
            catch (Exception ex) 
            {
                _log.LogError(ex.Message);
                throw;
            }
        }


        private static Color GetRandomColor()
        {
            Random random = new Random();
            return Color.FromArgb(random.Next(0, 100), random.Next(0, 100), random.Next(0, 100));
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

        public async Task VerifyCaptchaAsync(string captcha)
        {
            var cacheKey = CAPTCHA_CODE_PRE + captcha;
            var cacheValue = await redisDb.StringGetAsync(cacheKey);
            if (cacheValue == RedisValue.Null)
                throw new Exception("驗證碼已過期");

            var verifyResult = cacheValue.ToString().Equals(captcha, StringComparison.OrdinalIgnoreCase);

            await redisDb.StringGetDeleteAsync(cacheKey);

            if (verifyResult == false)
                throw new Exception("圖形密碼不正確");
        }

    }
}