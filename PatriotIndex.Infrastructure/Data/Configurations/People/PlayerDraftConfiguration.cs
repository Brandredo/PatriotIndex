using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatriotIndex.Domain.Entities.People;

namespace PatriotIndex.Infrastructure.Data.Configurations.People;

public class PlayerDraftConfiguration : IEntityTypeConfiguration<PlayerDraft>
{
    public void Configure(EntityTypeBuilder<PlayerDraft> entity)
    {
        entity.ToTable("player_draft");
        entity.HasKey(e => e.PlayerId);
        entity.HasOne(e => e.Player)
            .WithOne(p => p.Draft)
            .HasForeignKey<PlayerDraft>(e => e.PlayerId);
        entity.HasOne(e => e.DraftingTeam)
            .WithMany()
            .HasForeignKey(e => e.DraftingTeamId);
    }
}
