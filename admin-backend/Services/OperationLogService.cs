using admin_backend.Data;
using admin_backend.DTOs.AdminUser;
using admin_backend.DTOs.OperationLog;
using admin_backend.Entities;
using admin_backend.Interfaces;
using AutoMapper;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace admin_backend.Services
{
    public class OperationLogService : IOperationLogService
    {
        private readonly ILogger<OperationLogService> _log;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory;
        private readonly IAdminUserServices _adminUserServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Lazy<IIdentityService> _identityService;

        public OperationLogService(ILogger<OperationLogService> log, IMapper mapper, IDbContextFactory<MysqlDbContext> contextFactory, IAdminUserServices adminUserServices, IHttpContextAccessor httpContextAccessor, Lazy<IIdentityService> identityService)
        {
            _log = log;
            _mapper = mapper;
            _contextFactory = contextFactory;
            _adminUserServices = adminUserServices;
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }

        public async Task<PagedResult<OperationLogResponse>> Get(GetOperationLogDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var result = new List<OperationLogResponse>();
            IQueryable<OperationLog> operationLog = _context.OperationLog;

            if (dto.RoleId.HasValue)
            {
                var adminUser = await _adminUserServices.Get(new GetAdminUserDto
                {
                    RoleId = dto.RoleId.Value,
                });

                if (adminUser.Items.Any())
                {
                    var adminUserIds = adminUser.Items.Select(y => y.Id).ToList();
                    operationLog = _context.OperationLog.Where(x => adminUserIds.Contains(x.Id));
                }
            }

            if (dto.AdminUserId.HasValue)
            {
                operationLog = operationLog.Where(x => x.AdminUserId == dto.AdminUserId);
            }

            if (dto.Type.HasValue)
            {
                operationLog = operationLog.Where(x => x.Type == dto.Type);
            }

            if (dto.StartTime.HasValue)
            {
                operationLog = operationLog.Where(x => x.CreateTime >= dto.StartTime);
            }

            if (dto.EndTime.HasValue)
            {
                operationLog = operationLog.Where(x => x.CreateTime < dto.EndTime);
            }

            foreach (var x in operationLog)
            {
                var adminUser = (await _adminUserServices.Get(new GetAdminUserDto { Id = x.AdminUserId })).Items.FirstOrDefault();
                if (adminUser != null)
                {
                    var name = adminUser.Name;
                    result.Add(new OperationLogResponse
                    {
                        AdminUserId = x.AdminUserId,
                        AdminUserIdName = name,
                        Type = x.Type.GetDisplayName(),
                        Content = x.Content,
                        Ip = x.Ip,
                        Id = x.Id,
                    });
                }
            }
            return result.GetPaged(dto.Page);
        }

        public async Task Add(AddOperationLogDto dto)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            //取得IP
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;
            //取得Jwt Token資訊
            var JwtClaims = _httpContextAccessor.HttpContext?.User?.Claims.ToList();

            var claimsDto = _identityService.Value.GetUser();

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
