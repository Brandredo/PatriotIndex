namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class PlayReturnStats
{
    public Guid PlayPlayerStatId { get; set; }
    public short? Yards { get; set; }
    public bool? Returned { get; set; }
    public short? BrokenTackles { get; set; }
    public bool? FairCatch { get; set; }
    public short? YardsAfterCatch { get; set; }
    public bool? Td { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayPlayerStats PlayPlayerStats { get; set; } = null!;
}
