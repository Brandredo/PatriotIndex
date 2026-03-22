using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class PlayKickStatsConfiguration : IEntityTypeConfiguration<PlayKickStats>
{
    public void Configure(EntityTypeBuilder<PlayKickStats> entity)
    {
        entity.ToTable("play_kick_stats");
        entity.HasKey(e => e.PlayPlayerStatId);
        entity.Property(e => e.HangTime).HasColumnType("numeric(4,2)");
        entity.HasOne(e => e.PlayPlayerStats)
            .WithOne(h => h.KickStats)
            .HasForeignKey<PlayKickStats>(e => e.PlayPlayerStatId);
    }
}
