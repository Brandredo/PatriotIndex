using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamGame;

public class TeamGameReturnsConfiguration : IEntityTypeConfiguration<TeamGameReturns>
{
    public void Configure(EntityTypeBuilder<TeamGameReturns> entity)
    {
        entity.ToTable("team_game_returns");
        entity.HasKey(e => e.TeamGameId);
        entity.Property(e => e.KrAvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.PrAvgYards).HasColumnType("numeric(5,2)");
        entity.HasOne(e => e.TeamGameStats)
            .WithOne(h => h.Returns)
            .HasForeignKey<TeamGameReturns>(e => e.TeamGameId);
    }
}
