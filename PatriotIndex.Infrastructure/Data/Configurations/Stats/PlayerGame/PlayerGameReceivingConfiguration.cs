using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerGame;

public class PlayerGameReceivingConfiguration : IEntityTypeConfiguration<PlayerGameReceiving>
{
    public void Configure(EntityTypeBuilder<PlayerGameReceiving> entity)
    {
        entity.ToTable("player_game_receiving");
        entity.HasKey(e => e.PlayerGameId);
        entity.Property(e => e.AvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.YardsAfterCatch).HasColumnType("numeric(6,1)");
        entity.Property(e => e.YardsAfterContact).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.PlayerGameStats)
            .WithOne(h => h.Receiving)
            .HasForeignKey<PlayerGameReceiving>(e => e.PlayerGameId);
    }
}
