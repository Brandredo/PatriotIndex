namespace PatriotIndex.Domain.Entities.Stats.TeamGame;

public class TeamGameMisc
{
    public Guid TeamGameId { get; set; }
    public short? Fumbles { get; set; }
    public short? LostFumbles { get; set; }
    public short? OwnRecoveries { get; set; }
    public short? OppRecoveries { get; set; }
    public short? Penalties { get; set; }
    public int? PenaltyYards { get; set; }
    public short? TdRush { get; set; }
    public short? TdPass { get; set; }
    public short? TdReturn { get; set; }
    public short? TdInterception { get; set; }
    public short? TdFumbleRec { get; set; }
    public short? TdTotal { get; set; }
    public short? FdPass { get; set; }
    public short? FdRush { get; set; }
    public short? FdPenalty { get; set; }
    public short? FdTotal { get; set; }
    public short? RedzoneAttempts { get; set; }
    public short? RedzoneSuccesses { get; set; }
    public decimal? RedzonePct { get; set; }
    public short? GoaltogoAttempts { get; set; }
    public short? GoaltogoSuccesses { get; set; }
    public decimal? GoaltogoPct { get; set; }
    public short? ThirddownAttempts { get; set; }
    public short? ThirddownSuccesses { get; set; }
    public decimal? ThirddownPct { get; set; }
    public short? FourthdownAttempts { get; set; }
    public short? FourthdownSuccesses { get; set; }
    public decimal? FourthdownPct { get; set; }
    public string? PossessionTime { get; set; }
    public decimal? AvgGain { get; set; }
    public short? Safeties { get; set; }
    public short? Turnovers { get; set; }
    public short? PlayCount { get; set; }
    public short? RushPlays { get; set; }
    public int? TotalYards { get; set; }
    public int? ReturnYards { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public TeamGameStats TeamGameStats { get; set; } = null!;
}
