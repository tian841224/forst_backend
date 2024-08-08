using admin_backend.Data;
using admin_backend.Infrastructure;
using admin_backend.Interfaces;
using admin_backend.Services;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using CommonLibrary.Interfaces;
using CommonLibrary.Middlewares;
using CommonLibrary.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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

    var env = builder.Environment;
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
    builder.Configuration.AddEnvironmentVariables();

    //加入NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // 加入 DbContext 服務
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (connectionString != null)
    {
        builder.Services.AddDbContextFactory<MysqlDbContext>(options =>
        options.UseMySQL(connectionString));
    }
    else
    {
        throw new Exception("未設定DefaultConnection");
    }

    var jwtConfigSection = builder.Configuration.GetSection(nameof(JwtConfig));
    builder.Services.Configure<JwtConfig>(jwtConfigSection);
    builder.Services.AddControllers();
    builder.Services.AddAutoMapper(typeof(Program).Assembly);

    #region 注入
    builder.Services.AddScoped<IAdminUserServices, AdminUserServices>();
    builder.Services.AddScoped<IDamageClassService, DamageClassService>();
    builder.Services.AddScoped<IDamageTypeService, DamageTypeService>();
    builder.Services.AddScoped<ICommonDamageService, CommonDamageService>();
    builder.Services.AddScoped<IDocumentationService, DocumentationService>();
    builder.Services.AddScoped<IEpidemicSummaryService, EpidemicSummaryService>();
    builder.Services.AddScoped<IFAQService, FAQService>();
    builder.Services.AddScoped<IFileService, FileService>();
    builder.Services.AddScoped<IForestCompartmentLocationService, ForestCompartmentLocationService>();
    builder.Services.AddScoped<ILoginServices, LoginServices>();
    builder.Services.AddScoped<IMailConfigService, MailConfigService>();
    builder.Services.AddScoped<IOperationLogService, OperationLogService>();
    //延遲載入
    builder.Services.AddScoped(provider =>
        new Lazy<IOperationLogService>(() => provider.GetRequiredService<IOperationLogService>()));
    builder.Services.AddScoped<IRoleService, RoleService>();
    builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
    builder.Services.AddScoped<ITreeBasicInfoService, TreeBasicInfoService>();
    builder.Services.AddScoped<IForestDiseasePublicationsService, ForestDiseasePublicationsService>();
    builder.Services.AddScoped<IUserService, UserService>();


    /*----- 類別庫服務 -----*/

    //身分認證服務
    builder.Services.AddScoped<IIdentityService, IdentityService>();
    //延遲載入
    builder.Services.AddScoped(provider =>
        new Lazy<IIdentityService>(() => provider.GetRequiredService<IIdentityService>()));

    //檔案處理服務
    builder.Services.AddScoped<IFileService, FileService>();
    //延遲載入
    builder.Services.AddScoped(provider =>
        new Lazy<IFileService>(() => provider.GetRequiredService<IFileService>()));

    //Email
    builder.Services.AddScoped<IEmailService, EmailService>();
    //延遲載入
    builder.Services.AddScoped(provider =>
        new Lazy<IEmailService>(() => provider.GetRequiredService<IEmailService>()));
    #endregion

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    #region Swagger 
    builder.Services.AddSwaggerGen(options =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);

        // 啟用註釋
        options.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);


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

        var expiredHours = config.GetSection("JwtConfig")["Expired"];
        var jwtSecret = config.GetSection("JwtConfig")["SecretKey"];

        if (string.IsNullOrEmpty(jwtSecret) || string.IsNullOrEmpty(expiredHours))
        {
            throw new InvalidOperationException("JwtConfig is missing or empty in appsettings.json");
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
            ClockSkew = TimeSpan.FromHours(int.Parse(expiredHours))
        };
    });
    #endregion

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddCors(options =>
    {
        //options.AddDefaultPolicy(
        //    policy =>
        //    {
        //        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        //    });
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost:8080",
                                "http://localhost:3000",
                                "https://forest-admin-hyyvhv2yda-de.a.run.app",
                                "https://forest-client-hyyvhv2yda-de.a.run.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
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
    app.UseCors();
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
