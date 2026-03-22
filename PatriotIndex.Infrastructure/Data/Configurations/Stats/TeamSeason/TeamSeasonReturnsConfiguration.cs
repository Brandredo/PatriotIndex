using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamSeason;

public class TeamSeasonReturnsConfiguration : IEntityTypeConfiguration<TeamSeasonReturns>
{
    public void Configure(EntityTypeBuilder<TeamSeasonReturns> entity)
    {
        entity.ToTable("team_season_returns");
        entity.HasKey(e => e.TeamSeasonId);
        entity.Property(e => e.KrAvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.PrAvgYards).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.TeamSeasonStats)
            .WithOne(h => h.Returns)
            .HasForeignKey<TeamSeasonReturns>(e => e.TeamSeasonId);
    }
}
