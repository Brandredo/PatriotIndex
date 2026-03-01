namespace PatriotIndex.Domain.Entities;

public class DriveEvent
{
    public Guid DriveId { get; set; }

    public Guid PeriodId { get; set; }

    // Enums
    public string EventType { get; set; } // play, timeout, etc
    //public PlayType DriveType { get; set;}

    // Core Event
    public Guid Id { get; set; }
    public decimal Sequence { get; set; }
    public string Clock { get; set; }
    public string WallClock { get; set; }
    public string? Description { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    // Play Identification
    public string? PlayType { get; set; } // kickoff, punt, etc
    public string? PassRoute { get; set; }

    // Pre-Snap / Formation
    public string? QbSnap { get; set; }
    public string? Huddle { get; set; }
    public int? MenInBox { get; set; }
    public int? LeftTightEnds { get; set; }
    public int? RightTightEnds { get; set; }
    public string? HashMark { get; set; }
    public int? PlayersRushed { get; set; }

    // Play Attributes
    public string? PlayDirection { get; set; }
    public string? PocketLocation { get; set; }
    public bool? FakePunt { get; set; }
    public bool? FakeFieldGoal { get; set; }
    public bool? ScreenPass { get; set; }
    public bool? Blitz { get; set; }
    public bool? PlayAction { get; set; }
    public bool? RunPassOption { get; set; }

    // Clock / Down / Distance — Start
    public string? StartClock { get; set; }
    public int? StartDown { get; set; }
    public int? StartYardsToGain { get; set; }
    public int? StartLocationYardLine { get; set; }
    public Guid? StartPossessionTeamId { get; set; }

    // Clock / Down / Distance — End
    public string? EndClock { get; set; }
    public int? EndDown { get; set; }
    public int? EndYardsToGain { get; set; }
    public int? EndLocationYardLine { get; set; }
    public Guid? EndPossessionTeamId { get; set; }

    public Drive? Drive { get; set; }
    public IEnumerable<PbpEventStatistics> EventStats { get; set; } = [];
    public Period? Period { get; set; }
    public Team? StartTeam { get; set; }
    public Team? EndTeam { get; set; }
}