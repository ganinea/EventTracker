using EventTracker.Common;
using EventTracker.Domain;
using EventTracker.Infrastructure.Common;

namespace EventTracker.Infrastructure.Storage;

public class StorageConfiguration : ICustomConfiguration
{
    public string Provider { get; set; } = WellKnownStorageProvider.InMemory;
    public bool ApplyMigrationsOnStartup { get; set; } = true;
    public string? ConnectionString { get; set; }
}