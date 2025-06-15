using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventTracker.Infrastructure.Storage;

public class DatabaseMigrator(StorageConfiguration storageConfiguration,
    ILogger<DatabaseMigrator> logger,
    EventTrackerContext context)
{
    public void Migrate()
    {
        if (!storageConfiguration.ApplyMigrationsOnStartup)
        {
            logger.LogInformation("Migrating database skipped");
            return;
        }

        logger.LogInformation("Migrating database started");
        context.Database.Migrate();
        logger.LogInformation("Migrating database finished");
    }
}