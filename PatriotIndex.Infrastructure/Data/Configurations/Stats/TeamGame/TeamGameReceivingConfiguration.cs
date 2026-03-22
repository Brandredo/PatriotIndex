using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamGame;

public class TeamGameReceivingConfiguration : IEntityTypeConfiguration<TeamGameReceiving>
{
    public void Configure(EntityTypeBuilder<TeamGameReceiving> entity)
    {
        entity.ToTable("team_game_receiving");
        entity.HasKey(e => e.TeamGameId);
        entity.Property(e => e.AvgYards).HasColumnType("numeric(5,2)");
        entity.Property(e => e.YardsAfterCatch).HasColumnType("numeric(6,1)");
        entity.Property(e => e.YardsAfterContact).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.TeamGameStats)
            .WithOne(h => h.Receiving)
            .HasForeignKey<TeamGameReceiving>(e => e.TeamGameId);
    }
}
