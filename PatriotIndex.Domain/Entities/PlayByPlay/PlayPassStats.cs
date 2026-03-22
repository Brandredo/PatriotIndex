namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class PlayPassStats
{
    public Guid PlayPlayerStatId { get; set; }
    public short? Yards { get; set; }
    public bool? Attempt { get; set; }
    public bool? Complete { get; set; }
    public bool? Firstdown { get; set; }
    public decimal? PocketTime { get; set; }
    public bool? OnTargetThrow { get; set; }
    public bool? Goaltogo { get; set; }
    public bool? Inside20 { get; set; }
    public bool? Knockdown { get; set; }
    public bool? BattedPass { get; set; }
    public bool? Blitz { get; set; }
    public bool? Hurry { get; set; }
    public bool? Sack { get; set; }
    public bool? Interception { get; set; }
    public bool? Td { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayPlayerStats PlayPlayerStats { get; set; } = null!;
}
