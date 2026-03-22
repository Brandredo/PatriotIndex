using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamGame;

public class TeamGameKickingConfiguration : IEntityTypeConfiguration<TeamGameKicking>
{
    public void Configure(EntityTypeBuilder<TeamGameKicking> entity)
    {
        entity.ToTable("team_game_kicking");
        entity.HasKey(e => e.TeamGameId);
        entity.Property(e => e.FgPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.XpPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.KoAvgYards).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.TeamGameStats)
            .WithOne(h => h.Kicking)
            .HasForeignKey<TeamGameKicking>(e => e.TeamGameId);
    }
}
