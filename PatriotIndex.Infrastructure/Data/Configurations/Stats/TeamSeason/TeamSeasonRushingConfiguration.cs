using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamSeason;

public class TeamSeasonRushingConfiguration : IEntityTypeConfiguration<TeamSeasonRushing>
{
    public void Configure(EntityTypeBuilder<TeamSeasonRushing> entity)
    {
        entity.ToTable("team_season_rushing");
        entity.HasKey(e => e.TeamSeasonId);
        entity.Property(e => e.AvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.YardsAfterContact).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.TeamSeasonStats)
            .WithOne(h => h.Rushing)
            .HasForeignKey<TeamSeasonRushing>(e => e.TeamSeasonId);
    }
}
