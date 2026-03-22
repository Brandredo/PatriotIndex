using PatriotIndex.Domain.Entities.Organization;
using PatriotIndex.Domain.Entities.People;
using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Domain.Entities.Stats.PlayerGame;

public class PlayerGameStats
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid GameId { get; set; }
    public Guid TeamId { get; set; }
    public string? PositionPlayed { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Player Player { get; set; } = null!;
    public Game Game { get; set; } = null!;
    public Team Team { get; set; } = null!;
    public PlayerGamePassing? Passing { get; set; }
    public PlayerGameRushing? Rushing { get; set; }
    public PlayerGameReceiving? Receiving { get; set; }
    public PlayerGameDefense? Defense { get; set; }
    public PlayerGamePunts? Punts { get; set; }
    public PlayerGameKicking? Kicking { get; set; }
    public PlayerGameReturns? Returns { get; set; }
}
