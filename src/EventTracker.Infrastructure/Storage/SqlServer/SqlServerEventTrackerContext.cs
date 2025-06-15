using Microsoft.EntityFrameworkCore;

namespace EventTracker.Infrastructure.Storage.SqlServer;

public class SqlServerEventTrackerContext(DbContextOptions options)
    : EventTrackerContext(options);