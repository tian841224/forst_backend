using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace admin_backend.Data
{
    public static class MySqlConfiguration
    {
        public static IServiceCollection AddMySqlDbContext(this IServiceCollection services, string connectionStrings)
        {
            services.AddDbContext<MysqlDbContext>(options =>
            {
                // 使用 UseMySQL 方法
                if (!options.IsConfigured)
                {
                    options.UseMySQL(connectionStrings);
                }
            });
            return services;
        }
    }
}
