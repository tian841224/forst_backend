﻿using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommonLibrary.Data
{
    public class MysqlDbContext : DbContext
    {
        public MysqlDbContext(DbContextOptions<MysqlDbContext> options)
        : base(options)
        {
        }

        public DbSet<Role> Role { get; set; }
        public DbSet<EpidemicSummary> EpidemicSummary { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<AdSetting> AdSetting { get; set; }
        public DbSet<DamageClass> DamageClass { get; set; }
        public DbSet<MailConfig> MailConfig { get; set; }
        public DbSet<OperationLog> OperationLog { get; set; }
        public DbSet<FAQ> FAQ { get; set; }
        public DbSet<AdminUser> AdminUser { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Documentation> Documentation { get; set; }
        public DbSet<CommonPest> CommonPest { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<DamageType> DamageType { get; set; }
        public DbSet<Log> Log { get; set; }
    }
}
