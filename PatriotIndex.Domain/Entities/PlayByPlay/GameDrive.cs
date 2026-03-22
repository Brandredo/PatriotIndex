using PatriotIndex.Domain.Entities.Organization;
using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class GameDrive
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public short? Gain { get; set; }
    public string? StartReason { get; set; }
    public string? EndReason { get; set; }
    public short? PlayCount { get; set; }
    public string? Duration { get; set; }
    public short? FirstDowns { get; set; }
    public int? PenaltyYards { get; set; }
    public short? TeamSequence { get; set; }
    public string? StartClock { get; set; }
    public string? EndClock { get; set; }
    public short? FirstDriveYardline { get; set; }
    public short? LastDriveYardline { get; set; }
    public short? FarthestDriveYardline { get; set; }
    public int? NetYards { get; set; }
    public short? PatPointsAttempted { get; set; }
    public Guid? OffensiveTeamId { get; set; }
    public Guid? DefensiveTeamId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Game Game { get; set; } = null!;
    public Team? OffensiveTeam { get; set; }
    public Team? DefensiveTeam { get; set; }
    public ICollection<GamePlay> Plays { get; set; } = [];
}
