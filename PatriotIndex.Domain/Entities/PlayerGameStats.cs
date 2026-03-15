namespace PatriotIndex.Domain.Entities;

public class PlayerGameStats
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid GameId { get; set; }
    public Guid? TeamId { get; set; }
    public PlayerGameStatsBlock Stats { get; set; } = new();

    public Player? Player { get; set; }
    public Game? Game { get; set; }
    public Team? Team { get; set; }
}
