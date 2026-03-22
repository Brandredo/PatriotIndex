namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class PlayKickStats
{
    public Guid PlayPlayerStatId { get; set; }
    public short? Yards { get; set; }
    public short? NetYards { get; set; }
    public bool? Attempt { get; set; }
    public bool? Made { get; set; }
    public bool? SquibKick { get; set; }
    public bool? OnsideAttempt { get; set; }
    public bool? OnsideSuccess { get; set; }
    public bool? Touchback { get; set; }
    public bool? OutOfBounds { get; set; }
    public decimal? HangTime { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayPlayerStats PlayPlayerStats { get; set; } = null!;
}
