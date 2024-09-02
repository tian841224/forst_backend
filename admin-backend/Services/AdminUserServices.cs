using admin_backend.Data;
using admin_backend.DTOs.AdminUser;
using admin_backend.DTOs.Role;
using admin_backend.Entities;
using admin_backend.Enums;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces;
using CommonLibrary.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace admin_backend.Services
{
    public class AdminUserServices : IAdminUserServices
    {
        private readonly ILogger<AdminUserServices> _log;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly Lazy<IIdentityService> _identityService;
        private readonly Lazy<IFileService> _fileService;
        private readonly IRoleService _roleService;
        private readonly IMailConfigService _mailConfigService;
        private readonly IEmailService _emailService;

        public AdminUserServices(IDbContextFactory<MysqlDbContext> contextFactory, IHttpContextAccessor httpContextAccessor, ILogger<AdminUserServices> log, Lazy<IIdentityService> identityService, IMapper mapper, Lazy<IFileService> fileService, IRoleService roleService, IMailConfigService mailConfigService, IEmailService emailService)
        {
            _contextFactory = contextFactory;
            _httpContextAccessor = httpContextAccessor;
            _log = log;
            _identityService = identityService;
            _mapper = mapper;
            _fileService = fileService;
            _roleService = roleService;
            _mailConfigService = mailConfigService;
            _emailService = emailService;
        }

        public async Task<AdminUserResponse> Get()
        {
            var claimsDto = _identityService.Value.GetUser();

            int.TryParse(claimsDto.UserId, out int AdminUserId);

            var adminUser = (await Get(new GetAdminUserDto { Id = AdminUserId })).Items.FirstOrDefault() ?? new AdminUserResponse();

            //取得權限名稱
            var role = (await _roleService.Get(new GetRoleDto { Id = adminUser.RoleId })).Items.FirstOrDefault() ?? new RoleResponse();
            adminUser.RoleName = role.Name;

            return adminUser;
        }

        public async Task<PagedResult<AdminUserResponse>> Get(GetAdminUserDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            IQueryable<AdminUser> query = _context.AdminUser;

            if (dto.Id.HasValue)
            {
                query = query.Where(x => x.Id == dto.Id);
            }

            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                string keyword = dto.Keyword.ToLower();
                query = query.Where(x =>
                    x.Account.ToLower().Contains(keyword) ||
                    x.Password.ToLower().Contains(keyword) ||
                    x.Email.ToLower().Contains(keyword) ||
                    x.Name.ToLower().Contains(keyword)
                );
            }

            if (dto.RoleId.HasValue)
            {
                query = query.Where(x => x.RoleId == dto.RoleId);
            }

            if (dto.Status.HasValue)
            {
                query = query.Where(x => x.Status == dto.Status);
            }

            var adminUserResponse = _mapper.Map<List<AdminUserResponse>>(query);

            var tasks = adminUserResponse.Select(async x =>
            {
                // 照片處理
                if (!string.IsNullOrEmpty(x.Photo))
                {
                    x.Photo = _fileService.Value.GetFile(x.Photo, "image");
                }

                // 取得權限名稱
                var role = (await _roleService.Get(new GetRoleDto { Id = x.RoleId })).Items.FirstOrDefault() ?? new RoleResponse();
                x.RoleName = role.Name;

            });

            // 等待所有任務完成
            await Task.WhenAll(tasks);

            var pagedResult = adminUserResponse.GetPaged(dto.Page!);

            return pagedResult;
        }

        public async Task<AdminUserResponse> Add(AddAdminUserDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var adminUser = await _context.AdminUser.Where(x => x.Account == dto.Account).FirstOrDefaultAsync();

            if (adminUser != null)
            {
                throw new ApiException($"此帳號已註冊-{dto.Name}");
            }

            var role = await _context.Role.Where(x => x.Id == dto.RoleId).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new ApiException($"此身分權限不存在-{dto.RoleId}");
            }

            var photo = string.Empty;
            if (dto.Photo != null)
            {
                photo = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
                //上傳檔案
                var fileUploadDto = await _fileService.Value.UploadFile(photo, dto.Photo);
            }

            adminUser = new AdminUser
            {
                Name = dto.Name,
                Account = dto.Account,
                Password = dto.Password,
                Email = dto.Email,
                RoleId = dto.RoleId,
                Status = dto.Status,
                Photo = photo,
            };

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.AdminUser.AddAsync(adminUser);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    //取得IP
                    var ipAddress = _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!;

                    //新增操作紀錄
                    await _context.OperationLog.AddAsync(new OperationLog
                    {
                        AdminUserId = adminUser.Id,
                        Type = ChangeTypeEnum.Add,
                        Content = $"新增後台帳號:{adminUser.Name}/{adminUser.Account}",
                        Ip = ipAddress.ToString(),
                    });

                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
                adminUser.Photo = _fileService.Value.GetFile(photo, "image");
                return _mapper.Map<AdminUserResponse>(adminUser);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _log.LogError(ex.Message);
                throw;
            }
        }
        public async Task<AdminUserResponse> Update(int Id, UpdateAdminUserDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var adminUser = await _context.AdminUser.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (adminUser == null)
            {
                throw new ApiException($"無此資料-{Id}");
            }

            if (dto.RoleId.HasValue)
            {
                var role = await _context.Role.Where(x => x.Id == dto.RoleId).FirstOrDefaultAsync();

                if (role == null)
                {
                    throw new ApiException($"此身分權限不存在-{dto.RoleId}");
                }

                adminUser.RoleId = dto.RoleId.Value;
            }

            //取得IP
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress ?? new System.Net.IPAddress(0);

            if (!string.IsNullOrEmpty(dto.Name))
                adminUser.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Email))
                adminUser.Email = dto.Email;

            if (dto.Status.HasValue)
                adminUser.Status = (StatusEnum)dto.Status;

            if (!string.IsNullOrEmpty(dto.OldKey) && !string.IsNullOrEmpty(dto.pKey))
            {
                if (adminUser.Password != dto.OldKey)
                {
                    //新增操作紀錄
                    await _context.OperationLog.AddAsync(new OperationLog
                    {
                        AdminUserId = adminUser.Id,
                        Type = ChangeTypeEnum.Edit,
                        Content = $"修改後台帳號密碼-輸入錯誤:{adminUser.Name}/{adminUser.Account}",
                        Ip = ipAddress.ToString(),
                    });
                    throw new ApiException($"原密碼輸入錯誤-{Id}");
                }

                adminUser.Password = dto.pKey;
            }

            if (dto.Photo != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";

                //上傳檔案
                var fileUploadDto = await _fileService.Value.UploadFile(fileName, dto.Photo);
                adminUser.Photo = fileName;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.AdminUser.Update(adminUser);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    //新增操作紀錄
                    await _context.OperationLog.AddAsync(new OperationLog
                    {
                        AdminUserId = adminUser.Id,
                        Type = ChangeTypeEnum.Edit,
                        Content = $"修改後台帳號:{adminUser.Name}/{adminUser.Account}",
                        Ip = ipAddress.ToString(),
                    });

                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();

                return _mapper.Map<AdminUserResponse>(adminUser);

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
                throw new ApiException("查無此信箱，請重新輸入");
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

            await Update(adminUser.Id, new UpdateAdminUserDto
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
