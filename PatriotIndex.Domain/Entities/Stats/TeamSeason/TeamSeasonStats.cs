using PatriotIndex.Domain.Entities.Organization;
using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Domain.Entities.Stats.TeamSeason;

public class TeamSeasonStats
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid SeasonId { get; set; }
    public short? GamesPlayed { get; set; }
    public bool IsOpponentStats { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Team Team { get; set; } = null!;
    public Team Opponent { get; set; } = null!;
    public Season Season { get; set; } = null!;
    public TeamSeasonPassing? Passing { get; set; }
    public TeamSeasonRushing? Rushing { get; set; }
    public TeamSeasonReceiving? Receiving { get; set; }
    public TeamSeasonDefense? Defense { get; set; }
    public TeamSeasonPunts? Punts { get; set; }
    public TeamSeasonKicking? Kicking { get; set; }
    public TeamSeasonReturns? Returns { get; set; }
    public TeamSeasonMisc? Misc { get; set; }
}
