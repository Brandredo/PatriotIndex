namespace PatriotIndex.Domain.Entities.Stats.TeamSeason;

public class TeamSeasonReceiving
{
    public Guid TeamSeasonId { get; set; }
    public int? Targets { get; set; }
    public int? Receptions { get; set; }
    public int? Yards { get; set; }
    public decimal? AvgYards { get; set; }
    public short? Touchdowns { get; set; }
    public short? Longest { get; set; }
    public short? LongestTd { get; set; }
    public decimal? YardsAfterCatch { get; set; }
    public decimal? YardsAfterContact { get; set; }
    public short? BrokenTackles { get; set; }
    public short? Catchable { get; set; }
    public short? Dropped { get; set; }
    public short? FirstDowns { get; set; }
    public short? Inside20 { get; set; }
    public short? Goaltogo { get; set; }
    public short? LostFumbles { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public TeamSeasonStats TeamSeasonStats { get; set; } = null!;
}
