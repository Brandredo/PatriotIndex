using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamSeason;

public class TeamSeasonPassingConfiguration : IEntityTypeConfiguration<TeamSeasonPassing>
{
    public void Configure(EntityTypeBuilder<TeamSeasonPassing> entity)
    {
        entity.ToTable("team_season_passing");
        entity.HasKey(e => e.TeamSeasonId);
        entity.Property(e => e.CmpPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.Rating).HasColumnType("numeric(6,2)");
        entity.Property(e => e.Sacks).HasColumnType("numeric(6,1)");
        entity.Property(e => e.SackYards).HasColumnType("numeric(6,1)");
        entity.Property(e => e.PocketTime).HasColumnType("numeric(6,2)");
        entity.HasOne(e => e.TeamSeasonStats)
            .WithOne(h => h.Passing)
            .HasForeignKey<TeamSeasonPassing>(e => e.TeamSeasonId);
    }
}
