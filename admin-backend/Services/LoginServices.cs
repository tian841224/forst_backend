﻿using admin_backend.Data;
using admin_backend.DTOs.AdminUser;
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
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

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
        private readonly IEmailService _emailService;
        private readonly IMailConfigService _mailConfigService;

        public LoginServices(IDbContextFactory<MysqlDbContext> contextFactory, IOptions<JwtConfig> jwtConfig, ILogger<LoginServices> log, Lazy<IIdentityService> identityService, IFileService fileService, IAdminUserServices adminUserServices, IEmailService emailService, IMailConfigService mailConfigService)
        {
            _jwtConfig = jwtConfig.Value;
            _log = log;
            _contextFactory = contextFactory;
            _identityService = identityService;
            _fileService = fileService;
            _adminUserServices = adminUserServices;
            _emailService = emailService;
            _mailConfigService = mailConfigService;
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

                // 照片處理
                if (!string.IsNullOrEmpty(adminUser.Photo))
                {
                    adminUser.Photo = _fileService.GetFile(adminUser.Photo, "image");
                }

                return new IdentityResultDto
                {
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

        public async Task ResetPassword(ResetPasswordDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var adminUser = await _context.AdminUser.Where(x => x.Email == dto.Email).FirstOrDefaultAsync();

            if (adminUser == null)
            {
                throw new ApiException("帳號錯誤");
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

            await _adminUserServices.Update(adminUser.Id, new UpdateAdminUserDto
            {
                OldKey = adminUser.Password,
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
                Recipient = adminUser.Email,
            });
        }
    }
}