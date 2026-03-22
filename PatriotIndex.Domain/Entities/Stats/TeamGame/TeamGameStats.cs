using PatriotIndex.Domain.Entities.Organization;
using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Domain.Entities.Stats.TeamGame;

public class TeamGameStats
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid TeamId { get; set; }
    public bool IsHome { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Game Game { get; set; } = null!;
    public Team Team { get; set; } = null!;
    public TeamGamePassing? Passing { get; set; }
    public TeamGameRushing? Rushing { get; set; }
    public TeamGameReceiving? Receiving { get; set; }
    public TeamGameDefense? Defense { get; set; }
    public TeamGamePunts? Punts { get; set; }
    public TeamGameKicking? Kicking { get; set; }
    public TeamGameReturns? Returns { get; set; }
    public TeamGameMisc? Misc { get; set; }
}
