using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class PlayReceiveStatsConfiguration : IEntityTypeConfiguration<PlayReceiveStats>
{
    public void Configure(EntityTypeBuilder<PlayReceiveStats> entity)
    {
        entity.ToTable("play_receive_stats");
        entity.HasKey(e => e.PlayPlayerStatId);
        entity.HasOne(e => e.PlayPlayerStats)
            .WithOne(h => h.ReceiveStats)
            .HasForeignKey<PlayReceiveStats>(e => e.PlayPlayerStatId);
    }
}
