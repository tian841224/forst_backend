using admin_backend.Services;
using CommonLibrary.Data;
using CommonLibrary.Extensions;
using CommonLibrary.Infrastructure;
using CommonLibrary.Middleware;
using NLog;
using NLog.Web;
using System.Reflection;

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

    builder.Services.AddControllers();
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<RoleServices>();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
    var app = builder.Build();

    await builder.MigrateDbContextAsync<MysqlDbContext>(async (context, services) =>
    {
        await new SeedData().SeedAsync(context, services);
    });

    app.UseMiddleware<ExceptionHandling>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

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
