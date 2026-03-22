using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerSeason;

public class PlayerSeasonPuntsConfiguration : IEntityTypeConfiguration<PlayerSeasonPunts>
{
    public void Configure(EntityTypeBuilder<PlayerSeasonPunts> entity)
    {
        entity.ToTable("player_season_punts");
        entity.HasKey(e => e.PlayerSeasonId);
        entity.Property(e => e.GrossAvg).HasColumnType("numeric(5,2)");
        entity.Property(e => e.NetAvg).HasColumnType("numeric(5,2)");
        entity.Property(e => e.HangTime).HasColumnType("numeric(4,2)");
        entity.HasOne(e => e.PlayerSeasonStats)
            .WithOne(h => h.Punts)
            .HasForeignKey<PlayerSeasonPunts>(e => e.PlayerSeasonId);
    }
}
