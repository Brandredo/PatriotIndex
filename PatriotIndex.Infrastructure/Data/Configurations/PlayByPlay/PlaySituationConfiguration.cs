using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.PlayByPlay;

namespace PatriotIndex.Infrastructure.Data.Configurations.PlayByPlay;

public class PlaySituationConfiguration : IEntityTypeConfiguration<PlaySituation>
{
    public void Configure(EntityTypeBuilder<PlaySituation> entity)
    {
        entity.ToTable("play_situations");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        entity.Property(e => e.SituationType).HasMaxLength(5).IsRequired();
        entity.Property(e => e.Clock).HasMaxLength(10);
        entity.HasOne(e => e.Play)
            .WithMany(p => p.Situations)
            .HasForeignKey(e => e.PlayId);
        entity.HasOne(e => e.LocationTeam)
            .WithMany()
            .HasForeignKey(e => e.LocationTeamId);
        entity.HasOne(e => e.PossessionTeam)
            .WithMany()
            .HasForeignKey(e => e.PossessionTeamId);
        entity.HasIndex(e => new { e.PlayId, e.SituationType }).IsUnique();
        entity.HasIndex(e => e.PlayId).HasDatabaseName("idx_play_situations_play");
    }
}
