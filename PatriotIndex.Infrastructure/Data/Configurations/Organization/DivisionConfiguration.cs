using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.Organization;

namespace PatriotIndex.Infrastructure.Data.Configurations.Organization;

public class DivisionConfiguration : IEntityTypeConfiguration<Division>
{
    public void Configure(EntityTypeBuilder<Division> entity)
    {
        entity.ToTable("divisions");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(20).IsRequired();
        entity.HasIndex(e => e.Name).IsUnique();
        entity.HasOne(e => e.Conference)
            .WithMany(c => c.Divisions)
            .HasForeignKey(e => e.ConferenceId);
    }
}
