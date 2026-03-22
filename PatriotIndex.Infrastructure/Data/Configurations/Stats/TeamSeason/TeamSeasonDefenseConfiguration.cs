using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamSeason;

public class TeamSeasonDefenseConfiguration : IEntityTypeConfiguration<TeamSeasonDefense>
{
    public void Configure(EntityTypeBuilder<TeamSeasonDefense> entity)
    {
        entity.ToTable("team_season_defense");
        entity.HasKey(e => e.TeamSeasonId);
        entity.Property(e => e.Sacks).HasColumnType("numeric(5,1)");
        entity.Property(e => e.SackYards).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.TeamSeasonStats)
            .WithOne(h => h.Defense)
            .HasForeignKey<TeamSeasonDefense>(e => e.TeamSeasonId);
    }
}
