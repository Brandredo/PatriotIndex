using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class GamePlayConfiguration : IEntityTypeConfiguration<GamePlay>
{
    public void Configure(EntityTypeBuilder<GamePlay> entity)
    {
        entity.ToTable("game_plays");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Type).HasMaxLength(20);
        entity.Property(e => e.Clock).HasMaxLength(10);
        entity.Property(e => e.Sequence).HasColumnType("numeric(12,4)");
        entity.Property(e => e.PlayType).HasMaxLength(30);
        entity.Property(e => e.Huddle).HasMaxLength(20);
        entity.Property(e => e.QbAtSnap).HasMaxLength(20);
        entity.Property(e => e.PassRoute).HasMaxLength(30);
        entity.Property(e => e.HashMark).HasMaxLength(10);
        entity.Property(e => e.WallClock).HasColumnType("timestamp with time zone");
        entity.HasOne(e => e.Drive)
            .WithMany(d => d.Plays)
            .HasForeignKey(e => e.DriveId);
        entity.HasOne(e => e.Game)
            .WithMany()
            .HasForeignKey(e => e.GameId);
        entity.HasOne(e => e.Period)
            .WithMany(p => p.Plays)
            .HasForeignKey(e => e.PeriodId);
        entity.HasIndex(e => e.DriveId).HasDatabaseName("idx_game_plays_drive");
        entity.HasIndex(e => e.GameId).HasDatabaseName("idx_game_plays_game");
        entity.HasIndex(e => e.PeriodId).HasDatabaseName("idx_game_plays_period");
        entity.HasIndex(e => e.PlayType).HasDatabaseName("idx_game_plays_type");
    }
}
