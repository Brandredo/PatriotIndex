using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.People;

namespace PatriotIndex.Infrastructure.Data.Configurations.People;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> entity)
    {
        entity.ToTable("players");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        entity.Property(e => e.FirstName).HasMaxLength(50);
        entity.Property(e => e.LastName).HasMaxLength(50);
        entity.Property(e => e.AbbrName).HasMaxLength(20);
        entity.Property(e => e.Jersey).HasMaxLength(3);
        entity.Property(e => e.Position).HasMaxLength(10).IsRequired();
        entity.Property(e => e.Status).HasMaxLength(5);
        entity.Property(e => e.Weight).HasColumnType("numeric(5,1)");
        entity.Property(e => e.BirthPlace).HasMaxLength(100);
        entity.Property(e => e.College).HasMaxLength(80);
        entity.Property(e => e.CollegeConf).HasMaxLength(30);
        entity.Property(e => e.HighSchool).HasMaxLength(100);
        entity.HasOne(e => e.Team)
            .WithMany()
            .HasForeignKey(e => e.TeamId);
        entity.HasIndex(e => e.TeamId).HasDatabaseName("idx_players_team");
        entity.HasIndex(e => e.Position).HasDatabaseName("idx_players_position");
    }
}
