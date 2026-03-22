using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerSeason;

public class PlayerSeasonKickingConfiguration : IEntityTypeConfiguration<PlayerSeasonKicking>
{
    public void Configure(EntityTypeBuilder<PlayerSeasonKicking> entity)
    {
        entity.ToTable("player_season_kicking");
        entity.HasKey(e => e.PlayerSeasonId);
        entity.Property(e => e.FgPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.XpPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.KoAvgYards).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.PlayerSeasonStats)
            .WithOne(h => h.Kicking)
            .HasForeignKey<PlayerSeasonKicking>(e => e.PlayerSeasonId);
    }
}
