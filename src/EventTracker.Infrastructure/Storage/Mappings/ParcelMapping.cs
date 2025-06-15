using EventTracker.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventTracker.Infrastructure.Storage.Mappings;

internal class ParcelMapping : IEntityTypeConfiguration<Parcel>
{
    public void Configure(EntityTypeBuilder<Parcel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(x => x.LastEvent, x =>
        {
            x.HasIndex(e => e.Id);
            x.Property(e => e.Type).HasMaxLength(50);
            x.Property(e => e.RunId).HasMaxLength(50);
            x.Property(e => e.StatusCode).HasMaxLength(50);
        });

    }
}