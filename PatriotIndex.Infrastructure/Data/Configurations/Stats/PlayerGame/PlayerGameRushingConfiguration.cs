using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerGame;

public class PlayerGameRushingConfiguration : IEntityTypeConfiguration<PlayerGameRushing>
{
    public void Configure(EntityTypeBuilder<PlayerGameRushing> entity)
    {
        entity.ToTable("player_game_rushing");
        entity.HasKey(e => e.PlayerGameId);
        entity.Property(e => e.AvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.YardsAfterContact).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.PlayerGameStats)
            .WithOne(h => h.Rushing)
            .HasForeignKey<PlayerGameRushing>(e => e.PlayerGameId);
    }
}
