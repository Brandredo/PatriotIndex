using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerSeason;

public class PlayerSeasonRushingConfiguration : IEntityTypeConfiguration<PlayerSeasonRushing>
{
    public void Configure(EntityTypeBuilder<PlayerSeasonRushing> entity)
    {
        entity.ToTable("player_season_rushing");
        entity.HasKey(e => e.PlayerSeasonId);
        entity.Property(e => e.AvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.YardsAfterContact).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.PlayerSeasonStats)
            .WithOne(h => h.Rushing)
            .HasForeignKey<PlayerSeasonRushing>(e => e.PlayerSeasonId);
    }
}
