using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class PlayDefenseStatsConfiguration : IEntityTypeConfiguration<PlayDefenseStats>
{
    public void Configure(EntityTypeBuilder<PlayDefenseStats> entity)
    {
        entity.ToTable("play_defense_stats");
        entity.HasKey(e => e.PlayPlayerStatId);
        entity.Property(e => e.Sack).HasColumnType("numeric(3,1)");
        entity.HasOne(e => e.PlayPlayerStats)
            .WithOne(h => h.DefenseStats)
            .HasForeignKey<PlayDefenseStats>(e => e.PlayPlayerStatId);
    }
}
