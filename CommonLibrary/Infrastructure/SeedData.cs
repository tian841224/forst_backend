using CommonLibrary.Data;
using CommonLibrary.Entities;

namespace CommonLibrary.Infrastructure
{
    public class SeedData
    {
        public async Task SeedAsync(MysqlDbContext context, IServiceProvider serviceProvider)
        {

            //新增預設角色
            if (!context.Set<Role>().Any(u => u.Name == "admin"))
            {
                context.Role.Add(new Role { Name = "admin" });
                await context.SaveChangesAsync();
            }

            var roleId = context.Role.Where(x => x.Name == "admin").Select(x => x.Id).FirstOrDefault();

            if (roleId == 0)
                throw new Exception("新增預設角色失敗");

            //新增預設帳號
            if (!context.Set<AdminUser>().Any(u => u.Account == "admin"))
            {
                context.AdminUser.Add(new AdminUser
                {
                    Account = "admin",
                    Password = "!Allpower123",
                    Email = "admin@allpower.in",
                    Photo = "",
                    RoleId = roleId
                });
                await context.SaveChangesAsync();
            }

            
            var adminUser = context.AdminUser.Where(x => x.Account == "admin").Select(x => x.Id).FirstOrDefault();

            if (adminUser == 0)
                throw new Exception("新增預設帳號失敗");
        }
    }

}
