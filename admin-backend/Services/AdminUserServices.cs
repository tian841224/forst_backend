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
using StackExchange.Redis;
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

            //���o�v���W��
            var role = (await _roleService.Get(new GetRoleDto { Id = adminUser.RoleId })).Items.FirstOrDefault() ?? new RoleResponse();
            adminUser.RoleName = role.Name;

            return adminUser;
        }

        public async Task<PagedResult<AdminUserResponse>> Get(GetAdminUserDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var query = from adminUser in _context.AdminUser
                        join role in _context.Role on adminUser.RoleId equals role.Id into roleGroup
                        from role in roleGroup.DefaultIfEmpty()
                        select new
                        {
                            adminUser,
                            role,
                        };


            if (dto.Id.HasValue)
            {
                query = query.Where(x => x.adminUser.Id == dto.Id);
            }

            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                string keyword = dto.Keyword.ToLower();
                query = query.Where(x =>
                    x.role.Name.ToLower().Contains(keyword) ||
                    x.adminUser.Account.ToLower().Contains(keyword) ||
                    x.adminUser.Email.ToLower().Contains(keyword) ||
                    x.adminUser.Name.ToLower().Contains(keyword)
                );
            }

            if (dto.RoleId.HasValue)
            {
                query = query.Where(x => x.adminUser.RoleId == dto.RoleId);
            }

            if (dto.Status.HasValue)
            {
                query = query.Where(x => x.adminUser.Status == dto.Status);
            }

            var adminUserList = await query.ToListAsync();
            var adminUserResponse = _mapper.Map<List<AdminUserResponse>>(adminUserList.Select(x => x.adminUser));

            var tasks = adminUserResponse.Select(async x =>
            {
                // �Ӥ��B�z
                if (!string.IsNullOrEmpty(x.Photo))
                {
                    x.Photo = _fileService.Value.GetFile(x.Photo, "image");
                }

                // ���o�v���W��
                var role = (await _roleService.Get(new GetRoleDto { Id = x.RoleId })).Items.FirstOrDefault() ?? new RoleResponse();
                x.RoleName = role.Name;

            });

            // ���ݩҦ����ȧ���
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
                throw new ApiException($"���b���w���U-{dto.Name}");
            }

            var role = await _context.Role.Where(x => x.Id == dto.RoleId).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new ApiException($"�������v�����s�b-{dto.RoleId}");
            }

            var photo = string.Empty;
            if (dto.Photo != null)
            {
                photo = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
                //�W���ɮ�
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

                //�s�W�ާ@����
                if (await _context.SaveChangesAsync() > 0)
                {
                    //���oIP
                    var ipAddress = _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!;

                    //�s�W�ާ@����
                    await _context.OperationLog.AddAsync(new OperationLog
                    {
                        AdminUserId = adminUser.Id,
                        Type = ChangeTypeEnum.Add,
                        Content = $"�s�W��x�b��:{adminUser.Name}/{adminUser.Account}",
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
                throw new ApiException($"�L�����-{Id}");
            }

            if (dto.RoleId.HasValue)
            {
                var role = await _context.Role.Where(x => x.Id == dto.RoleId).FirstOrDefaultAsync();

                if (role == null)
                {
                    throw new ApiException($"�������v�����s�b-{dto.RoleId}");
                }

                adminUser.RoleId = dto.RoleId.Value;
            }

            //���oIP
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
                    //�s�W�ާ@����
                    await _context.OperationLog.AddAsync(new OperationLog
                    {
                        AdminUserId = adminUser.Id,
                        Type = ChangeTypeEnum.Edit,
                        Content = $"�ק��x�b���K�X-��J���~:{adminUser.Name}/{adminUser.Account}",
                        Ip = ipAddress.ToString(),
                    });
                    throw new ApiException($"��K�X��J���~");
                }

                adminUser.Password = dto.pKey;
            }

            if (dto.Photo != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";

                //�W���ɮ�
                var fileUploadDto = await _fileService.Value.UploadFile(fileName, dto.Photo);
                adminUser.Photo = fileName;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.AdminUser.Update(adminUser);

                //�s�W�ާ@����
                if (await _context.SaveChangesAsync() > 0)
                {
                    //�s�W�ާ@����
                    await _context.OperationLog.AddAsync(new OperationLog
                    {
                        AdminUserId = adminUser.Id,
                        Type = ChangeTypeEnum.Edit,
                        Content = $"�ק��x�b��:{adminUser.Name}/{adminUser.Account}",
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
                throw new ApiException("�d�L���H�c�A�Э��s��J");
            }

            //�ק�K�X
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

            //���o�H��]�w
            var emailConfig = await _mailConfigService.Get();

            //�o�e�s�K�X
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
                    Subject = "���m�K�X�H��",
                    Body = $"�s�K�X�G{password.ToString()}�A�Ч����O��",
                    IsBodyHtml = true,
                },
                Recipient = adminUser.Email,
            });
        }

    }
}
