using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerGame;

public class PlayerGamePassingConfiguration : IEntityTypeConfiguration<PlayerGamePassing>
{
    public void Configure(EntityTypeBuilder<PlayerGamePassing> entity)
    {
        entity.ToTable("player_game_passing");
        entity.HasKey(e => e.PlayerGameId);
        entity.Property(e => e.CmpPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.Rating).HasColumnType("numeric(6,2)");
        entity.Property(e => e.Sacks).HasColumnType("numeric(6,1)");
        entity.Property(e => e.SackYards).HasColumnType("numeric(6,1)");
        entity.Property(e => e.PocketTime).HasColumnType("numeric(6,2)");
        entity.HasOne(e => e.PlayerGameStats)
            .WithOne(h => h.Passing)
            .HasForeignKey<PlayerGamePassing>(e => e.PlayerGameId);
    }
}
