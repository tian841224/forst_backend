using Microsoft.Extensions.DependencyInjection;

namespace CommonLibrary.Data
{
    public static class MySqlConfiguration
    {
        public static IServiceCollection AddMySqlDbContext(this IServiceCollection services)
        {
            services.AddDbContext<MysqlDbContext>(); // 使用 UseMySQL 方法
            return services;
        }
    }
}
