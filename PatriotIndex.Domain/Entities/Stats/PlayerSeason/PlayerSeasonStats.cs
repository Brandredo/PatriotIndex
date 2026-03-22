using PatriotIndex.Domain.Entities.Organization;
using PatriotIndex.Domain.Entities.People;
using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Domain.Entities.Stats.PlayerSeason;

public class PlayerSeasonStats
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid SeasonId { get; set; }
    public Guid TeamId { get; set; }
    public short? GamesPlayed { get; set; }
    public short? GamesStarted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Player Player { get; set; } = null!;
    public Season Season { get; set; } = null!;
    public Team Team { get; set; } = null!;
    public PlayerSeasonPassing? Passing { get; set; }
    public PlayerSeasonRushing? Rushing { get; set; }
    public PlayerSeasonReceiving? Receiving { get; set; }
    public PlayerSeasonDefense? Defense { get; set; }
    public PlayerSeasonPunts? Punts { get; set; }
    public PlayerSeasonKicking? Kicking { get; set; }
    public PlayerSeasonReturns? Returns { get; set; }
}
