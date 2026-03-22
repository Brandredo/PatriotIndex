using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Stats.TeamGame;

namespace PatriotIndex.Infrastructure.Data.Configurations.Stats.TeamGame;

public class TeamGamePuntsConfiguration : IEntityTypeConfiguration<TeamGamePunts>
{
    public void Configure(EntityTypeBuilder<TeamGamePunts> entity)
    {
        entity.ToTable("team_game_punts");
        entity.HasKey(e => e.TeamGameId);
        entity.Property(e => e.GrossAvg).HasColumnType("numeric(5,2)");
        entity.Property(e => e.NetAvg).HasColumnType("numeric(5,2)");
        entity.Property(e => e.HangTime).HasColumnType("numeric(4,2)");
        entity.HasOne(e => e.TeamGameStats)
            .WithOne(h => h.Punts)
            .HasForeignKey<TeamGamePunts>(e => e.TeamGameId);
    }
}
