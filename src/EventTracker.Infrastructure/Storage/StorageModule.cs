using EventTracker.Common;
using EventTracker.Domain.Data;
using EventTracker.Infrastructure.Common;
using EventTracker.Infrastructure.Storage.Services;
using EventTracker.Infrastructure.Storage.SqlServer;
using EventTracker.Infrastructure.Storage.SqlServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventTracker.Infrastructure.Storage;

public class StorageModule(IConfiguration configuration) : IModule
{
    public void AddServices(IServiceCollection services)
    {
        services.AddScoped<DatabaseMigrator>();
        services.AddCustomConfiguration<StorageConfiguration>(configuration);

        services.AddDbContext<SqlServerEventTrackerContext>((sp, contextOptions) =>
        {
            var options = sp.GetRequiredService<StorageConfiguration>();
            if (options.Provider != WellKnownStorageProvider.SqlServer)
                return;
            contextOptions.UseSqlServer(options.ConnectionString);
        });

        services.AddScoped<EventTrackerContext>(sp =>
        {
            var options = sp.GetRequiredService<StorageConfiguration>();

            return options.Provider switch
            {
                WellKnownStorageProvider.SqlServer =>
                    sp.GetRequiredService<SqlServerEventTrackerContext>(),
                _ => throw new NotSupportedException()
            };
        });

        services.AddScoped<IWorkerStateDataService, WorkerStateDataService>();
        services.AddScoped<SqlServerParcelDataService>();
        services.AddScoped<IParcelDataService>(sp =>
        {
            var options = sp.GetRequiredService<StorageConfiguration>();

            return options.Provider switch
            {
                WellKnownStorageProvider.SqlServer =>
                    sp.GetRequiredService<SqlServerParcelDataService>(),
                _ => throw new NotSupportedException()
            };
        });
    }
}