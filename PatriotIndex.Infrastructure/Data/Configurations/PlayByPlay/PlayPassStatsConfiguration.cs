using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class PlayPassStatsConfiguration : IEntityTypeConfiguration<PlayPassStats>
{
    public void Configure(EntityTypeBuilder<PlayPassStats> entity)
    {
        entity.ToTable("play_pass_stats");
        entity.HasKey(e => e.PlayPlayerStatId);
        entity.Property(e => e.PocketTime).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.PlayPlayerStats)
            .WithOne(h => h.PassStats)
            .HasForeignKey<PlayPassStats>(e => e.PlayPlayerStatId);
    }
}
