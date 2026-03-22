namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class PlayReceiveStats
{
    public Guid PlayPlayerStatId { get; set; }
    public short? Yards { get; set; }
    public bool? Target { get; set; }
    public bool? Reception { get; set; }
    public bool? Dropped { get; set; }
    public bool? Catchable { get; set; }
    public bool? Firstdown { get; set; }
    public short? BrokenTackles { get; set; }
    public short? YardsAfterCatch { get; set; }
    public short? YardsAfterContact { get; set; }
    public bool? Goaltogo { get; set; }
    public bool? Inside20 { get; set; }
    public bool? Td { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayPlayerStats PlayPlayerStats { get; set; } = null!;
}
