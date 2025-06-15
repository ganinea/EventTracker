using EventTracker.Domain.Model;
using EventTracker.Infrastructure.Storage.Mappings;
using Microsoft.EntityFrameworkCore;

namespace EventTracker.Infrastructure.Storage;

public abstract class EventTrackerContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<WorkerState> WorkerStates => Set<WorkerState>();
    public DbSet<Parcel> Parcels => Set<Parcel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParcelMapping).Assembly);
    }
}