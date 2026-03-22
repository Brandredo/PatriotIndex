using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamSeason;

public class TeamSeasonKickingConfiguration : IEntityTypeConfiguration<TeamSeasonKicking>
{
    public void Configure(EntityTypeBuilder<TeamSeasonKicking> entity)
    {
        entity.ToTable("team_season_kicking");
        entity.HasKey(e => e.TeamSeasonId);
        entity.Property(e => e.FgPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.XpPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.KoAvgYards).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.TeamSeasonStats)
            .WithOne(h => h.Kicking)
            .HasForeignKey<TeamSeasonKicking>(e => e.TeamSeasonId);
    }
}
