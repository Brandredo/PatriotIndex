using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerSeason;

public class PlayerSeasonDefenseConfiguration : IEntityTypeConfiguration<PlayerSeasonDefense>
{
    public void Configure(EntityTypeBuilder<PlayerSeasonDefense> entity)
    {
        entity.ToTable("player_season_defense");
        entity.HasKey(e => e.PlayerSeasonId);
        entity.Property(e => e.Sacks).HasColumnType("numeric(4,1)");
        entity.Property(e => e.SackYards).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.PlayerSeasonStats)
            .WithOne(h => h.Defense)
            .HasForeignKey<PlayerSeasonDefense>(e => e.PlayerSeasonId);
    }
}
