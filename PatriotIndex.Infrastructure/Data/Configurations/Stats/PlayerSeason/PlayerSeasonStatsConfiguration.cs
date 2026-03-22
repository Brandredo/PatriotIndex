using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.PlayerSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.PlayerSeason;

public class PlayerSeasonStatsConfiguration : IEntityTypeConfiguration<PlayerSeasonStats>
{
    public void Configure(EntityTypeBuilder<PlayerSeasonStats> entity)
    {
        entity.ToTable("player_season_stats");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        entity.HasOne(e => e.Player)
            .WithMany()
            .HasForeignKey(e => e.PlayerId);
        entity.HasOne(e => e.Season)
            .WithMany()
            .HasForeignKey(e => e.SeasonId);
        entity.HasOne(e => e.Team)
            .WithMany()
            .HasForeignKey(e => e.TeamId);
        entity.HasIndex(e => new { e.PlayerId, e.SeasonId, e.TeamId }).IsUnique();
        entity.HasIndex(e => e.PlayerId).HasDatabaseName("idx_player_season_player");
        entity.HasIndex(e => e.SeasonId).HasDatabaseName("idx_player_season_season");
        entity.HasIndex(e => e.TeamId).HasDatabaseName("idx_player_season_team");
    }
}
