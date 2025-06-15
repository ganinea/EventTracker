using Microsoft.EntityFrameworkCore;

namespace EventTracker.Infrastructure.Storage;

public class InMemoryEventTrackerContext(DbContextOptions options)
    : EventTrackerContext(options);
