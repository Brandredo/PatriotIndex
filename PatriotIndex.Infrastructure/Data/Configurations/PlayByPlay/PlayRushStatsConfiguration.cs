using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class PlayRushStatsConfiguration : IEntityTypeConfiguration<PlayRushStats>
{
    public void Configure(EntityTypeBuilder<PlayRushStats> entity)
    {
        entity.ToTable("play_rush_stats");
        entity.HasKey(e => e.PlayPlayerStatId);
        entity.HasOne(e => e.PlayPlayerStats)
            .WithOne(h => h.RushStats)
            .HasForeignKey<PlayRushStats>(e => e.PlayPlayerStatId);
    }
}
