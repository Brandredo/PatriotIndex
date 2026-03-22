using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamSeason;

public class TeamSeasonPuntsConfiguration : IEntityTypeConfiguration<TeamSeasonPunts>
{
    public void Configure(EntityTypeBuilder<TeamSeasonPunts> entity)
    {
        entity.ToTable("team_season_punts");
        entity.HasKey(e => e.TeamSeasonId);
        entity.Property(e => e.GrossAvg).HasColumnType("numeric(5,2)");
        entity.Property(e => e.NetAvg).HasColumnType("numeric(5,2)");
        entity.Property(e => e.HangTime).HasColumnType("numeric(4,2)");
        entity.HasOne(e => e.TeamSeasonStats)
            .WithOne(h => h.Punts)
            .HasForeignKey<TeamSeasonPunts>(e => e.TeamSeasonId);
    }
}
