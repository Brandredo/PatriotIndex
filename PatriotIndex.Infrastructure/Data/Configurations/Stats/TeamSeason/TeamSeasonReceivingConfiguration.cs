using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamSeason;

public class TeamSeasonReceivingConfiguration : IEntityTypeConfiguration<TeamSeasonReceiving>
{
    public void Configure(EntityTypeBuilder<TeamSeasonReceiving> entity)
    {
        entity.ToTable("team_season_receiving");
        entity.HasKey(e => e.TeamSeasonId);
        entity.Property(e => e.AvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.YardsAfterCatch).HasColumnType("numeric(6,1)");
        entity.Property(e => e.YardsAfterContact).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.TeamSeasonStats)
            .WithOne(h => h.Receiving)
            .HasForeignKey<TeamSeasonReceiving>(e => e.TeamSeasonId);
    }
}
