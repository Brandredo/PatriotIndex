namespace PatriotIndex.Domain.Entities;

public class TeamSeasonStats
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string? SeasonSrId { get; set; }
    public int SeasonYear { get; set; }
    public string SeasonType { get; set; } = "";
    public int GamesPlayed { get; set; }

    // Stored as JSONB columns — see DbContext configuration
    public TeamStatBlock Record { get; set; } = new();
    public TeamStatBlock Opponents { get; set; } = new();

    public Team? Team { get; set; }
}
