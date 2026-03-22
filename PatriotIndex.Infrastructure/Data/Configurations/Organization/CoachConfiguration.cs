using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Organization;

namespace PatriotIndex.Infrastructure.Data.Configurations.Organization;

public class CoachConfiguration : IEntityTypeConfiguration<Coach>
{
    public void Configure(EntityTypeBuilder<Coach> entity)
    {
        entity.ToTable("coaches");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.FullName).HasMaxLength(100).IsRequired();
        entity.Property(e => e.FirstName).HasMaxLength(50);
        entity.Property(e => e.LastName).HasMaxLength(50);
        entity.Property(e => e.Position).HasMaxLength(60);
        entity.HasOne(e => e.Team)
            .WithMany(t => t.Coaches)
            .HasForeignKey(e => e.TeamId);
    }
}
