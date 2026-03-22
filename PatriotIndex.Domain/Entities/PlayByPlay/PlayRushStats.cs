namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class PlayRushStats
{
    public Guid PlayPlayerStatId { get; set; }
    public short? Yards { get; set; }
    public bool? Attempt { get; set; }
    public bool? Firstdown { get; set; }
    public short? BrokenTackles { get; set; }
    public short? YardsAfterContact { get; set; }
    public bool? Goaltogo { get; set; }
    public bool? Inside20 { get; set; }
    public bool? Td { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayPlayerStats PlayPlayerStats { get; set; } = null!;
}
