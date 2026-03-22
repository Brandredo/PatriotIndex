using PatriotIndex.Domain.Entities.Organization;
using PatriotIndex.Domain.Entities.People;

namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class PlayPlayerStats
{
    public Guid Id { get; set; }
    public Guid PlayId { get; set; }
    public Guid PlayerId { get; set; }
    public Guid TeamId { get; set; }
    public string StatType { get; set; } = null!;
    public string? Category { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public GamePlay Play { get; set; } = null!;
    public Player Player { get; set; } = null!;
    public Team Team { get; set; } = null!;
    public PlayPassStats? PassStats { get; set; }
    public PlayReceiveStats? ReceiveStats { get; set; }
    public PlayRushStats? RushStats { get; set; }
    public PlayDefenseStats? DefenseStats { get; set; }
    public PlayKickStats? KickStats { get; set; }
    public PlayReturnStats? ReturnStats { get; set; }
}
