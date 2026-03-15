namespace PatriotIndex.Domain.Entities;

public class TeamGameStats
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid TeamId { get; set; }
    public bool IsHome { get; set; }
    public TeamGameStatsBlock Stats { get; set; } = new();

    public Game? Game { get; set; }
    public Team? Team { get; set; }
}
