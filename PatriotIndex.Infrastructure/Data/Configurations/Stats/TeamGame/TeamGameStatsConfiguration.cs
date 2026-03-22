using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamGame;

public class TeamGameStatsConfiguration : IEntityTypeConfiguration<TeamGameStats>
{
    public void Configure(EntityTypeBuilder<TeamGameStats> entity)
    {
        entity.ToTable("team_game_stats");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        entity.HasOne(e => e.Game)
            .WithMany()
            .HasForeignKey(e => e.GameId);
        entity.HasOne(e => e.Team)
            .WithMany()
            .HasForeignKey(e => e.TeamId);
        entity.HasIndex(e => new { e.GameId, e.TeamId }).IsUnique();
        entity.HasIndex(e => e.GameId).HasDatabaseName("idx_team_game_game");
        entity.HasIndex(e => e.TeamId).HasDatabaseName("idx_team_game_team");
    }
}
