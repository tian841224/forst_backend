﻿using admin_backend.Data;
using admin_backend.Entities;
using admin_backend.Enums;

namespace admin_backend.Infrastructure
{
    public class SeedData
    {
        public async Task SeedAsync(MysqlDbContext context, IServiceProvider serviceProvider)
        {

            //新增預設角色
            if (!context.Set<Role>().Any(u => u.Name == "Admin"))
            {
                context.Role.Add(new Role { Name = "Admin" });
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
                    Name = "管理員帳號",
                    Account = "admin",
                    Password = "!Allpower123",
                    Email = "admin@allpower.in",
                    Photo = "",
                    Status = StatusEnum.Open,
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
