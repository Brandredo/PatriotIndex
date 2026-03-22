namespace PatriotIndex.Domain.Entities.Stats.TeamSeason;

public class TeamSeasonPunts
{
    public Guid TeamSeasonId { get; set; }
    public short? Attempts { get; set; }
    public int? Yards { get; set; }
    public decimal? GrossAvg { get; set; }
    public decimal? NetAvg { get; set; }
    public short? Longest { get; set; }
    public short? Inside20 { get; set; }
    public short? Touchbacks { get; set; }
    public short? OutOfBounds { get; set; }
    public short? FairCatches { get; set; }
    public short? Blocked { get; set; }
    public decimal? HangTime { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public TeamSeasonStats TeamSeasonStats { get; set; } = null!;
}
