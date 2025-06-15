using EventTracker;
using EventTracker.Application;
using EventTracker.Infrastructure;
using EventTracker.Infrastructure.Common;
using EventTracker.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File("Logs/log.txt")
    .CreateBootstrapLogger();

try
{
    var builder = Host.CreateApplicationBuilder(args);
    ConfigureServices(builder.Services, builder.Configuration);

    var host = builder.Build();
    
    using (var scope = host.Services.CreateScope())
    {
        var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
        migrator.Migrate();
    }

    host.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application start-up failed");
    if (Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development")
        throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddHostedService<Worker>();
    services.AddSerilog(x =>
        x.ReadFrom.Configuration(configuration));
    services.AddModules(new InfrastructureModule(configuration),
        new ApplicationModule());
}




