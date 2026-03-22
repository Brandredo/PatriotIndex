using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerSeason;

public class PlayerSeasonPassingConfiguration : IEntityTypeConfiguration<PlayerSeasonPassing>
{
    public void Configure(EntityTypeBuilder<PlayerSeasonPassing> entity)
    {
        entity.ToTable("player_season_passing");
        entity.HasKey(e => e.PlayerSeasonId);
        entity.Property(e => e.CmpPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.Rating).HasColumnType("numeric(6,2)");
        entity.Property(e => e.Sacks).HasColumnType("numeric(6,1)");
        entity.Property(e => e.SackYards).HasColumnType("numeric(6,1)");
        entity.Property(e => e.PocketTime).HasColumnType("numeric(6,2)");
        entity.HasOne(e => e.PlayerSeasonStats)
            .WithOne(h => h.Passing)
            .HasForeignKey<PlayerSeasonPassing>(e => e.PlayerSeasonId);
    }
}
