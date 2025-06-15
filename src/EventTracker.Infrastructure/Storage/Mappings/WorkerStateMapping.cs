using EventTracker.Domain;
using EventTracker.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventTracker.Infrastructure.Storage.Mappings;

internal class WorkerStateMapping : IEntityTypeConfiguration<WorkerState>
{
    public void Configure(EntityTypeBuilder<WorkerState> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
    }
}