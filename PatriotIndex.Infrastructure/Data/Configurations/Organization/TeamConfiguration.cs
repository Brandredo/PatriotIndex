using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Organization;

namespace PatriotIndex.Infrastructure.Data.Configurations.Organization;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> entity)
    {
        entity.ToTable("teams");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(60).IsRequired();
        entity.Property(e => e.Alias).HasMaxLength(4).IsRequired();
        entity.Property(e => e.Market).HasMaxLength(40);
        entity.Property(e => e.Mascot).HasMaxLength(40);
        entity.Property(e => e.Owner).HasMaxLength(100);
        entity.HasOne(e => e.Division)
            .WithMany(d => d.Teams)
            .HasForeignKey(e => e.DivisionId);
        entity.HasOne(e => e.Venue)
            .WithMany(v => v.Teams)
            .HasForeignKey(e => e.VenueId);
        entity.HasIndex(e => e.DivisionId).HasDatabaseName("idx_teams_division");
        entity.HasIndex(e => e.VenueId).HasDatabaseName("idx_teams_venue");
    }
}
