using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Infrastructure.Data.Configurations.Schedule;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> entity)
    {
        entity.ToTable("games");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.ScheduledAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired();
        entity.Property(e => e.Status).HasMaxLength(20);
        entity.Property(e => e.GameType).HasMaxLength(20);
        entity.Property(e => e.Clock).HasMaxLength(10);
        entity.Property(e => e.Duration).HasMaxLength(10);
        entity.Property(e => e.Network).HasMaxLength(20);
        entity.HasOne(e => e.Season)
            .WithMany(s => s.Games)
            .HasForeignKey(e => e.SeasonId);
        entity.HasOne(e => e.Week)
            .WithMany(w => w.Games)
            .HasForeignKey(e => e.WeekId);
        entity.HasOne(e => e.Venue)
            .WithMany()
            .HasForeignKey(e => e.VenueId);
        entity.HasOne(e => e.HomeTeam)
            .WithMany()
            .HasForeignKey(e => e.HomeTeamId);
        entity.HasOne(e => e.AwayTeam)
            .WithMany()
            .HasForeignKey(e => e.AwayTeamId);
        entity.HasIndex(e => e.SeasonId).HasDatabaseName("idx_games_season");
        entity.HasIndex(e => e.WeekId).HasDatabaseName("idx_games_week");
        entity.HasIndex(e => e.HomeTeamId).HasDatabaseName("idx_games_home_team");
        entity.HasIndex(e => e.AwayTeamId).HasDatabaseName("idx_games_away_team");
        entity.HasIndex(e => e.VenueId).HasDatabaseName("idx_games_venue");
        entity.HasIndex(e => e.ScheduledAt).HasDatabaseName("idx_games_scheduled");
    }
}
