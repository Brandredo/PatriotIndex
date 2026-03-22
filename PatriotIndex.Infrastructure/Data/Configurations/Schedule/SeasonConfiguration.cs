using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Infrastructure.Data.Configurations.Schedule;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> entity)
    {
        entity.ToTable("seasons");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Type).HasMaxLength(3).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(30);
        entity.HasIndex(e => new { e.Year, e.Type }).IsUnique();
    }
}
