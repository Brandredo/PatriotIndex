using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamGame;

public class TeamGamePassingConfiguration : IEntityTypeConfiguration<TeamGamePassing>
{
    public void Configure(EntityTypeBuilder<TeamGamePassing> entity)
    {
        entity.ToTable("team_game_passing");
        entity.HasKey(e => e.TeamGameId);
        entity.Property(e => e.CmpPct).HasColumnType("numeric(5,2)");
        entity.Property(e => e.Rating).HasColumnType("numeric(6,2)");
        entity.Property(e => e.Sacks).HasColumnType("numeric(6,1)");
        entity.Property(e => e.SackYards).HasColumnType("numeric(6,1)");
        entity.Property(e => e.PocketTime).HasColumnType("numeric(6,2)");
        entity.HasOne(e => e.TeamGameStats)
            .WithOne(h => h.Passing)
            .HasForeignKey<TeamGamePassing>(e => e.TeamGameId);
    }
}
