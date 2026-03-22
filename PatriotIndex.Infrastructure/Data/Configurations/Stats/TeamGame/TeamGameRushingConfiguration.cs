using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamGame;

public class TeamGameRushingConfiguration : IEntityTypeConfiguration<TeamGameRushing>
{
    public void Configure(EntityTypeBuilder<TeamGameRushing> entity)
    {
        entity.ToTable("team_game_rushing");
        entity.HasKey(e => e.TeamGameId);
        entity.Property(e => e.AvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.YardsAfterContact).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.TeamGameStats)
            .WithOne(h => h.Rushing)
            .HasForeignKey<TeamGameRushing>(e => e.TeamGameId);
    }
}
