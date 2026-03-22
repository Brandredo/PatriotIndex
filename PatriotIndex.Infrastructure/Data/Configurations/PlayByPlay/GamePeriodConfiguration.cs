using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class GamePeriodConfiguration : IEntityTypeConfiguration<GamePeriod>
{
    public void Configure(EntityTypeBuilder<GamePeriod> entity)
    {
        entity.ToTable("game_periods");
        entity.HasKey(e => e.Id);
        entity.HasOne(e => e.Game)
            .WithMany()
            .HasForeignKey(e => e.GameId);
        entity.HasIndex(e => new { e.GameId, e.Number }).IsUnique();
        entity.HasIndex(e => e.GameId).HasDatabaseName("idx_game_periods_game");
    }
}
