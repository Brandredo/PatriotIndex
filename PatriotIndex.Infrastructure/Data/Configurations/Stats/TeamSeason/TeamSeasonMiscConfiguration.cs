using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamSeason;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamSeason;

public class TeamSeasonMiscConfiguration : IEntityTypeConfiguration<TeamSeasonMisc>
{
    public void Configure(EntityTypeBuilder<TeamSeasonMisc> entity)
    {
        entity.ToTable("team_season_misc");
        entity.HasKey(e => e.TeamSeasonId);
        entity.Property(e => e.RedzonePct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.GoaltogoPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.ThirddownPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.FourthdownPct).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.TeamSeasonStats)
            .WithOne(h => h.Misc)
            .HasForeignKey<TeamSeasonMisc>(e => e.TeamSeasonId);
    }
}
