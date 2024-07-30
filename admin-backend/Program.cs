using admin_backend.Services;
using CommonLibrary.Data;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using CommonLibrary.Infrastructure;
using CommonLibrary.Middleware;
using CommonLibrary.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Reflection;
using System.Text;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("啟動程式");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //加入NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // 加入 DbContext 服務
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (connectionString != null)
    {
        builder.Services.AddMySqlDbContext(connectionString);
    }
    else throw new Exception("未設定DefaultConnection");

    var jwtConfigSection = builder.Configuration.GetSection(nameof(JwtConfig));
    builder.Services.Configure<JwtConfig>(jwtConfigSection);

    #region 注入
    builder.Services.AddControllers();
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<AdminUserServices>();
    builder.Services.AddScoped<RoleServices>();
    builder.Services.AddScoped<LoginServices>();
    builder.Services.AddScoped<IdentityService>();
    builder.Services.AddScoped<MailConfigService>();
    builder.Services.AddScoped<OperationLogService>();
    builder.Services.AddScoped<RolePermissionService>();
    builder.Services.AddScoped<TreeBasicInfoService>();
    #endregion

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    #region Swagger 
    builder.Services.AddSwaggerGen(options =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);

        options.AddSecurityDefinition("Bearer",
         new OpenApiSecurityScheme
         {
             Name = "Authorization",
             Type = SecuritySchemeType.ApiKey,
             Scheme = "Bearer",
             BearerFormat = "JWT",
             In = ParameterLocation.Header,
             Description = "Example: \"Bearer xxxxxxxxxxxxxxx\""
         });

        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
            });

    });
    #endregion

    #region Jwt
    // 設定Jwt驗證
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var jwtSecret = config.GetSection("JwtConfig")["SecretKey"];

        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new InvalidOperationException("JwtConfig:Secret is missing or empty in appsettings.json");
        }

        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = false,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
    #endregion

    builder.Services.AddHttpContextAccessor();

    var app = builder.Build();

    await builder.MigrateDbContextAsync<MysqlDbContext>(async (context, services) =>
    {
        await new SeedData().SeedAsync(context, services);
    });

    app.UseMiddleware<ExceptionHandling>();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{

    //}

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
