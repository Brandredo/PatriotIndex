using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerGame;

public class PlayerGamePuntsConfiguration : IEntityTypeConfiguration<PlayerGamePunts>
{
    public void Configure(EntityTypeBuilder<PlayerGamePunts> entity)
    {
        entity.ToTable("player_game_punts");
        entity.HasKey(e => e.PlayerGameId);
        entity.Property(e => e.GrossAvg).HasColumnType("numeric(5,2)");
        entity.Property(e => e.NetAvg).HasColumnType("numeric(5,2)");
        entity.Property(e => e.HangTime).HasColumnType("numeric(4,2)");
        entity.HasOne(e => e.PlayerGameStats)
            .WithOne(h => h.Punts)
            .HasForeignKey<PlayerGamePunts>(e => e.PlayerGameId);
    }
}
