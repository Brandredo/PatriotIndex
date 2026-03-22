namespace PatriotIndex.Domain.Entities.Stats.TeamGame;

public class TeamGameRushing
{
    public Guid TeamGameId { get; set; }
    public int? Attempts { get; set; }
    public int? Yards { get; set; }
    public decimal? AvgYards { get; set; }
    public short? Touchdowns { get; set; }
    public short? Longest { get; set; }
    public short? LongestTd { get; set; }
    public short? BrokenTackles { get; set; }
    public short? FirstDowns { get; set; }
    public short? Inside20 { get; set; }
    public short? Goaltogo { get; set; }
    public decimal? YardsAfterContact { get; set; }
    public short? Scrambles { get; set; }
    public short? KneelDowns { get; set; }
    public short? LostFumbles { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public TeamGameStats TeamGameStats { get; set; } = null!;
}
