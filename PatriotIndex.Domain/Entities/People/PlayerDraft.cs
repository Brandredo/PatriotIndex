using PatriotIndex.Domain.Entities.Organization;

namespace PatriotIndex.Domain.Entities.People;

public class PlayerDraft
{
    public Guid PlayerId { get; set; }
    public Guid? DraftingTeamId { get; set; }
    public short? Year { get; set; }
    public short? Round { get; set; }
    public short? PickNumber { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Player Player { get; set; } = null!;
    public Team? DraftingTeam { get; set; }
}
