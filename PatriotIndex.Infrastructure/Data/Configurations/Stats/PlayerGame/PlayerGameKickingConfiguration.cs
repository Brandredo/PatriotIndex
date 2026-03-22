using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerGame;

public class PlayerGameKickingConfiguration : IEntityTypeConfiguration<PlayerGameKicking>
{
    public void Configure(EntityTypeBuilder<PlayerGameKicking> entity)
    {
        entity.ToTable("player_game_kicking");
        entity.HasKey(e => e.PlayerGameId);
        entity.Property(e => e.FgPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.XpPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.KoAvgYards).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.PlayerGameStats)
            .WithOne(h => h.Kicking)
            .HasForeignKey<PlayerGameKicking>(e => e.PlayerGameId);
    }
}
