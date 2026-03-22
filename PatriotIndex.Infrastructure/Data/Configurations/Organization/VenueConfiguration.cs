using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Organization;

namespace PatriotIndex.Infrastructure.Data.Configurations.Organization;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> entity)
    {
        entity.ToTable("venues");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        entity.Property(e => e.City).HasMaxLength(50);
        entity.Property(e => e.State).HasMaxLength(30);
        entity.Property(e => e.Country).HasMaxLength(30).HasDefaultValue("USA");
        entity.Property(e => e.Address).HasMaxLength(150);
        entity.Property(e => e.Zip).HasMaxLength(10);
        entity.Property(e => e.Surface).HasMaxLength(20);
        entity.Property(e => e.RoofType).HasMaxLength(20);
        entity.Property(e => e.Lat).HasColumnType("numeric(10,7)");
        entity.Property(e => e.Lng).HasColumnType("numeric(10,7)");
    }
}
