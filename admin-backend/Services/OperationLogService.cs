using CommonLibrary.Data;
using CommonLibrary.DTOs.AdminUser;
using CommonLibrary.DTOs.OperationLog;
using CommonLibrary.Entities;
using CommonLibrary.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace admin_backend.Services
{
    public class OperationLogService
    {
        private readonly ILogger<OperationLogService> _log;
        private readonly MysqlDbContext _context;
        private readonly AdminUserServices _adminUserServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityService _identityService;

        public OperationLogService(ILogger<OperationLogService> log, MysqlDbContext context, AdminUserServices adminUserServices, IHttpContextAccessor httpContextAccessor, IdentityService identityService)
        {
            _log = log;
            _context = context;
            _adminUserServices = adminUserServices;
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }

        public async Task<List<GetOperationLogResponseDto>> Get(GetOperationLogDto dto)
        {
            IQueryable<OperationLog> query = _context.OperationLog.AsQueryable();
            var result = new List<GetOperationLogResponseDto>();

            if (dto.RoleId.HasValue)
            {
                var adminUser = await _adminUserServices.Get(new GetAdminUserDto
                {
                    RoleId = dto.RoleId.Value,
                });

                if (adminUser.Any())
                {
                    var adminUserIds = adminUser.Select(y => y.Id).ToList();
                    var operationLogs = _context.OperationLog.Where(x => adminUserIds.Contains(x.Id));
                    query = operationLogs;
                }
            }

            if (dto.AdminUserId.HasValue)
            {
                query = query.Where(x => x.AdminUserId == dto.AdminUserId);
            }

            if (dto.Type.HasValue)
            {
                query = query.Where(x => x.Type == dto.Type);
            }

            if (dto.StartTime.HasValue)
            {
                query = query.Where(x => x.CreateTime >= dto.StartTime);
            }

            if (dto.EndTime.HasValue)
            {
                query = query.Where(x => x.CreateTime < dto.EndTime);
            }

            var operationLog = await query.ToListAsync();
            foreach (var x in operationLog)
            {
                var adminUser = (await _adminUserServices.Get(new GetAdminUserDto { Id = x.AdminUserId })).FirstOrDefault();
                if (adminUser != null)
                {
                    var name = adminUser.Name;
                    result.Add(new GetOperationLogResponseDto
                    {
                        Id = x.Id,
                        AdminUserId = x.AdminUserId,
                        AdminUserIdName = name,
                        Type = x.Type.GetDisplayName(),
                        Content = x.Content,
                        Ip = x.Ip,
                        CreateTime = x.CreateTime,
                    });
                }
            }

            return result;
        }

        public async Task Add(AddOperationLogDto dto)
        {
            //取得IP
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;
            //取得Jwt Token資訊
            var JwtClaims = _httpContextAccessor.HttpContext?.User?.Claims.ToList();

            var claimsDto = _identityService.GetUser();

            int.TryParse(claimsDto.UserId, out int AdminUserId);

            //新增操作紀錄
            await _context.OperationLog.AddAsync(new OperationLog
            {
                AdminUserId = AdminUserId,
                Type = dto.Type,
                Content = dto.Content,
                Ip = ipAddress!.ToString() ?? string.Empty,
            });

            await _context.SaveChangesAsync();
        }
    }
}
