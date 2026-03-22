using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerSeason;

public class PlayerSeasonReceivingConfiguration : IEntityTypeConfiguration<PlayerSeasonReceiving>
{
    public void Configure(EntityTypeBuilder<PlayerSeasonReceiving> entity)
    {
        entity.ToTable("player_season_receiving");
        entity.HasKey(e => e.PlayerSeasonId);
        entity.Property(e => e.AvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.YardsAfterCatch).HasColumnType("numeric(6,1)");
        entity.Property(e => e.YardsAfterContact).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.PlayerSeasonStats)
            .WithOne(h => h.Receiving)
            .HasForeignKey<PlayerSeasonReceiving>(e => e.PlayerSeasonId);
    }
}
