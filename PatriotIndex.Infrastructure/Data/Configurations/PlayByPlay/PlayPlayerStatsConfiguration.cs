using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class PlayPlayerStatsConfiguration : IEntityTypeConfiguration<PlayPlayerStats>
{
    public void Configure(EntityTypeBuilder<PlayPlayerStats> entity)
    {
        entity.ToTable("play_player_stats");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        entity.Property(e => e.StatType).HasMaxLength(20).IsRequired();
        entity.Property(e => e.Category).HasMaxLength(40);
        entity.HasOne(e => e.Play)
            .WithMany(p => p.PlayerStats)
            .HasForeignKey(e => e.PlayId);
        entity.HasOne(e => e.Player)
            .WithMany()
            .HasForeignKey(e => e.PlayerId);
        entity.HasOne(e => e.Team)
            .WithMany()
            .HasForeignKey(e => e.TeamId);
        entity.HasIndex(e => e.PlayId).HasDatabaseName("idx_play_pstats_play");
        entity.HasIndex(e => e.PlayerId).HasDatabaseName("idx_play_pstats_player");
        entity.HasIndex(e => e.TeamId).HasDatabaseName("idx_play_pstats_team");
        entity.HasIndex(e => e.StatType).HasDatabaseName("idx_play_pstats_type");
    }
}
