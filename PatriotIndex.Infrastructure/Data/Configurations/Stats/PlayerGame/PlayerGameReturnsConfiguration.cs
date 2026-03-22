using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerGame;

public class PlayerGameReturnsConfiguration : IEntityTypeConfiguration<PlayerGameReturns>
{
    public void Configure(EntityTypeBuilder<PlayerGameReturns> entity)
    {
        entity.ToTable("player_game_returns");
        entity.HasKey(e => e.PlayerGameId);
        entity.Property(e => e.KrAvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.PrAvgYards).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.PlayerGameStats)
            .WithOne(h => h.Returns)
            .HasForeignKey<PlayerGameReturns>(e => e.PlayerGameId);
    }
}
