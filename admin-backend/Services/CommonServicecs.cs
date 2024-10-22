using admin_backend.Data;
using admin_backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace admin_backend.Services
{
    public class CommonServicecs(IAdminUserServices adminUserServices, IDbContextFactory<MysqlDbContext> contextFactory) : ICommonServicecs
    {
        private readonly IAdminUserServices _adminUserServices = adminUserServices;
        private readonly IDbContextFactory<MysqlDbContext> _contextFactory = contextFactory;


        public async Task<string> GetAdminUserNameAsync(int AdminUserId)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();

            var adminUser = await _context.AdminUser.Where(x => x.Id == AdminUserId).FirstOrDefaultAsync();
            if (adminUser == null) return string.Empty;

            return adminUser.Name;
        }
    }
}
