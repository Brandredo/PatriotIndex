using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamGame;

public class TeamGameDefenseConfiguration : IEntityTypeConfiguration<TeamGameDefense>
{
    public void Configure(EntityTypeBuilder<TeamGameDefense> entity)
    {
        entity.ToTable("team_game_defense");
        entity.HasKey(e => e.TeamGameId);
        entity.Property(e => e.Sacks).HasColumnType("numeric(5,1)");
        entity.Property(e => e.SackYards).HasColumnType("numeric(6,1)");
        entity.HasOne(e => e.TeamGameStats)
            .WithOne(h => h.Defense)
            .HasForeignKey<TeamGameDefense>(e => e.TeamGameId);
    }
}
