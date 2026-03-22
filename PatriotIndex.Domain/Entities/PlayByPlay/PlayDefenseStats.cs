namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class PlayDefenseStats
{
    public Guid PlayPlayerStatId { get; set; }
    public bool? Blitz { get; set; }
    public bool? Hurry { get; set; }
    public bool? Tackle { get; set; }
    public bool? Assist { get; set; }
    public decimal? Sack { get; set; }
    public bool? Interception { get; set; }
    public short? IntYards { get; set; }
    public bool? ForcedFumble { get; set; }
    public bool? FumbleRecovery { get; set; }
    public bool? DefComp { get; set; }
    public bool? Knockdown { get; set; }
    public bool? DefTarget { get; set; }
    public bool? BattedPass { get; set; }
    public bool? MissedTackle { get; set; }
    public bool? QbHit { get; set; }
    public bool? Td { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayPlayerStats PlayPlayerStats { get; set; } = null!;
}
