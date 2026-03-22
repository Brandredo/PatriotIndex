using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerGame;

public class PlayerGameStatsConfiguration : IEntityTypeConfiguration<PlayerGameStats>
{
    public void Configure(EntityTypeBuilder<PlayerGameStats> entity)
    {
        entity.ToTable("player_game_stats");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        entity.Property(e => e.PositionPlayed).HasMaxLength(10);
        entity.HasOne(e => e.Player)
            .WithMany()
            .HasForeignKey(e => e.PlayerId);
        entity.HasOne(e => e.Game)
            .WithMany()
            .HasForeignKey(e => e.GameId);
        entity.HasOne(e => e.Team)
            .WithMany()
            .HasForeignKey(e => e.TeamId);
        entity.HasIndex(e => new { e.PlayerId, e.GameId }).IsUnique();
        entity.HasIndex(e => e.PlayerId).HasDatabaseName("idx_player_game_player");
        entity.HasIndex(e => e.GameId).HasDatabaseName("idx_player_game_game");
        entity.HasIndex(e => e.TeamId).HasDatabaseName("idx_player_game_team");
    }
}
