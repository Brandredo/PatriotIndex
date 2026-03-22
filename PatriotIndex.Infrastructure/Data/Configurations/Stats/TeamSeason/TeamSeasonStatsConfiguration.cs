using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamSeason;

public class TeamSeasonStatsConfiguration : IEntityTypeConfiguration<TeamSeasonStats>
{
    public void Configure(EntityTypeBuilder<TeamSeasonStats> entity)
    {
        entity.ToTable("team_season_stats");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        entity.HasOne(e => e.Team)
            .WithMany()
            .HasForeignKey(e => e.TeamId);
        entity.HasOne(e => e.Season)
            .WithMany()
            .HasForeignKey(e => e.SeasonId);
        entity.HasIndex(e => new { e.TeamId, e.SeasonId }).IsUnique();
        entity.HasIndex(e => e.TeamId).HasDatabaseName("idx_team_season_team");
        entity.HasIndex(e => e.SeasonId).HasDatabaseName("idx_team_season_season");
    }
}
