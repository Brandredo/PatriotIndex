using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamGame;

public class TeamGameMiscConfiguration : IEntityTypeConfiguration<TeamGameMisc>
{
    public void Configure(EntityTypeBuilder<TeamGameMisc> entity)
    {
        entity.ToTable("team_game_misc");
        entity.HasKey(e => e.TeamGameId);
        entity.Property(e => e.RedzonePct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.GoaltogoPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.ThirddownPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.FourthdownPct).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.TeamGameStats)
            .WithOne(h => h.Misc)
            .HasForeignKey<TeamGameMisc>(e => e.TeamGameId);
    }
}
