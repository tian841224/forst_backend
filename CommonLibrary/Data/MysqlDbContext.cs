using CommonLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommonLibrary.Data
{
    public class MysqlDbContext : DbContext
    {
        public MysqlDbContext(DbContextOptions<MysqlDbContext> options)
        : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<EpidemicSummary> EpidemicSummaries { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<AdSetting> AdSettings { get; set; }
        public DbSet<DamageClass> DamageClasses { get; set; }
        public DbSet<MailConfig> MailConfigs { get; set; }
        public DbSet<OperationLog> OperationLogs { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<AdminUser> Admin { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Documentation> Documentations { get; set; }
        public DbSet<CommonPest> CommonPests { get; set; }
        //public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<DamageType> DamageTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("data source=localhost;database=forest;user id=root;password=test123;pooling=true;charset=utf8;");
            }
        }

    }
}
