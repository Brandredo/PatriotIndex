namespace PatriotIndex.Domain.Entities;

public class PlayerSeasonStats
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid? TeamId { get; set; }
    public string? PlayerSrId { get; set; }
    public string? SeasonSrId { get; set; }
    public int SeasonYear { get; set; }
    public string SeasonType { get; set; } = "";
    public int GamesPlayed { get; set; }
    public int GamesStarted { get; set; }

    // Stored as a JSONB column — see DbContext configuration
    public PlayerStatBlock Stats { get; set; } = new();

    public Player? Player { get; set; }
    public Team? Team { get; set; }
}
