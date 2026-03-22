using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class GameDriveConfiguration : IEntityTypeConfiguration<GameDrive>
{
    public void Configure(EntityTypeBuilder<GameDrive> entity)
    {
        entity.ToTable("game_drives");
        entity.HasKey(e => e.Id);
        entity.HasOne(e => e.Game)
            .WithMany()
            .HasForeignKey(e => e.GameId);
        entity.HasIndex(e => e.GameId).HasDatabaseName("idx_game_drives_game");
    }
}
