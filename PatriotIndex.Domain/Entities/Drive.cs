namespace PatriotIndex.Domain.Entities;

public class Drive
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public int? PeriodNumber { get; set; }
    public int? Sequence { get; set; }
    public int? TeamSequence { get; set; }
    public string? StartReason { get; set; }
    public string? EndReason { get; set; }
    public int? PlayCount { get; set; }
    public string? Duration { get; set; }
    public int? FirstDowns { get; set; }
    public int? Gain { get; set; }
    public int? PenaltyYards { get; set; }
    public int? NetYards { get; set; }
    public string? StartClock { get; set; }
    public string? EndClock { get; set; }
    public Guid? OffensiveTeamId { get; set; }
    public Guid? DefensiveTeamId { get; set; }
    public int OffensivePoints { get; set; }
    public int DefensivePoints { get; set; }

    public Game? Game { get; set; }
    public Team? OffensiveTeam { get; set; }
    public Team? DefensiveTeam { get; set; }
    public ICollection<Play> Plays { get; set; } = [];
}
