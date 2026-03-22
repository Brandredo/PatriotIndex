using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Organization;

namespace PatriotIndex.Infrastructure.Data.Configurations.Organization;

public class ConferenceConfiguration : IEntityTypeConfiguration<Conference>
{
    public void Configure(EntityTypeBuilder<Conference> entity)
    {
        entity.ToTable("conferences");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(3).IsRequired();
        entity.HasIndex(e => e.Name).IsUnique();
    }
}
