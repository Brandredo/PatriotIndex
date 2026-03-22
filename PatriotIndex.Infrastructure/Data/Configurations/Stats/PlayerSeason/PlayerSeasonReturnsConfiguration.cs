using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerSeason;

public class PlayerSeasonReturnsConfiguration : IEntityTypeConfiguration<PlayerSeasonReturns>
{
    public void Configure(EntityTypeBuilder<PlayerSeasonReturns> entity)
    {
        entity.ToTable("player_season_returns");
        entity.HasKey(e => e.PlayerSeasonId);
        entity.Property(e => e.KrAvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.PrAvgYards).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.PlayerSeasonStats)
            .WithOne(h => h.Returns)
            .HasForeignKey<PlayerSeasonReturns>(e => e.PlayerSeasonId);
    }
}
