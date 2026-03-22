using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Infrastructure.Data.Configurations.Schedule;

public class WeekConfiguration : IEntityTypeConfiguration<Week>
{
    public void Configure(EntityTypeBuilder<Week> entity)
    {
        entity.ToTable("weeks");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Title).HasMaxLength(20);
        entity.HasOne(e => e.Season)
            .WithMany(s => s.Weeks)
            .HasForeignKey(e => e.SeasonId);
        entity.HasIndex(e => e.SeasonId).HasDatabaseName("idx_weeks_season");
        entity.HasIndex(e => new { e.SeasonId, e.Sequence }).IsUnique();
    }
}
