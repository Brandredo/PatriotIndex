using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class PlayReturnStatsConfiguration : IEntityTypeConfiguration<PlayReturnStats>
{
    public void Configure(EntityTypeBuilder<PlayReturnStats> entity)
    {
        entity.ToTable("play_return_stats");
        entity.HasKey(e => e.PlayPlayerStatId);
        entity.HasOne(e => e.PlayPlayerStats)
            .WithOne(h => h.ReturnStats)
            .HasForeignKey<PlayReturnStats>(e => e.PlayPlayerStatId);
    }
}
