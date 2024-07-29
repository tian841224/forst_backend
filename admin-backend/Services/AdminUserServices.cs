using CommonLibrary.Data;
using CommonLibrary.DTOs.AdminUser;
using CommonLibrary.Entities;
using CommonLibrary.Enums;
using CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;

namespace admin_backend.Services
{
    public class AdminUserServices
    {
        private readonly MysqlDbContext _context;
        private readonly ILogger<AdminUserServices> _log;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminUserServices(MysqlDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<AdminUserServices> log)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _log = log;
        }

        public async Task<List<AdminUser>> Get(GetAdminUserDto dto)
        {
            IQueryable<AdminUser> query = _context.AdminUser.AsQueryable();


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

            return await query.ToListAsync();
        }

        public async Task<AdminUser> Add(AddAdminUserDto dto)
        {
            try
            {
                var adminUser = await _context.AdminUser.Where(x => x.Account == dto.Account).FirstOrDefaultAsync();

                if (adminUser != null)
                {
                    throw new ApiException($"此帳號已註冊-{dto.Name}");
                }

                adminUser = new AdminUser
                {
                    Name = dto.Name,
                    Account = dto.Account,
                    Password = dto.Password,
                    Email = dto.Email,
                    RoleId = dto.RoleId,
                    Status = dto.Status,
                    Photo = dto.Photo,
                };

                await _context.AdminUser.AddAsync(adminUser);

                //新增操作紀錄
                if (await _context.SaveChangesAsync() > 0)
                {
                    //取得IP
                    var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;

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

                return adminUser;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }
        public async Task<AdminUser> Update(UpdateAdminUserDto dto)
        {
            var adminUser = await _context.AdminUser.Where(x => x.Id == dto.Id).FirstOrDefaultAsync();

            if (adminUser == null)
            {
                throw new ApiException($"無此資料-{dto.Id}");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                adminUser.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Email))
                adminUser.Email = dto.Email;

            if (dto.Status.HasValue)
                adminUser.Status = (StatusEnum)dto.Status;

            if (!string.IsNullOrEmpty(dto.Password))
                adminUser.Password = dto.Password;

            _context.AdminUser.Update(adminUser);

            //新增操作紀錄
            if (await _context.SaveChangesAsync() > 0)
            {
                //取得IP
                var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;

                //新增操作紀錄
                await _context.OperationLog.AddAsync(new OperationLog
                {
                    AdminUserId = adminUser.Id,
                    Type = ChangeTypeEnum.Add,
                    Content = $"修改後台帳號:{adminUser.Name}/{adminUser.Account}",
                    Ip = ipAddress.ToString(),
                });

                await _context.SaveChangesAsync();
            }

            return adminUser;
        }
    }
}
