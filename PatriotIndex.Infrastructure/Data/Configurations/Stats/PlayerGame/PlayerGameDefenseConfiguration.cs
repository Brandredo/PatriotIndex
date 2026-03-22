using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerGame;

public class PlayerGameDefenseConfiguration : IEntityTypeConfiguration<PlayerGameDefense>
{
    public void Configure(EntityTypeBuilder<PlayerGameDefense> entity)
    {
        entity.ToTable("player_game_defense");
        entity.HasKey(e => e.PlayerGameId);
        entity.Property(e => e.Sacks).HasColumnType("numeric(4,1)");
        entity.Property(e => e.SackYards).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.PlayerGameStats)
            .WithOne(h => h.Defense)
            .HasForeignKey<PlayerGameDefense>(e => e.PlayerGameId);
    }
}
