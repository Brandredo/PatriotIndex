using PatriotIndex.Domain.Enums;

namespace PatriotIndex.Domain.Entities;

public class PbpDriveEvent
{
    // Enums
    public EventType EventType { get; set;}
    public PlayType DriveType { get; set;}
    public enum Direction { Left, Right, Center }
    public enum HuddleType { Yes, No }

    // Core Event
    public EventType Type { get; set; }
    public Guid Id { get; set; }
    public long Sequence { get; set; }
    public string Clock { get; set; }
    public string WallClock { get; set; }
    public string Description { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    // Play Identification
    public PlayType? PlayType { get; set; }
    public string PassRoute { get; set; }

    // Pre-Snap / Formation
    public string? QbSnap { get; set; }
    public HuddleType? Huddle { get; set; }
    public int? MenInBox { get; set; }
    public int? LeftTightEnds { get; set; }
    public int? RightTightEnds { get; set; }
    public string? HashMark { get; set; }
    public int? PlayersRushed { get; set; }

    // Play Attributes
    public Direction? PlayDirection { get; set; }
    public string? PocketLocation { get; set; }
    public bool FakePunt { get; set; }
    public bool FakeFieldGoal { get; set; }
    public bool ScreenPass { get; set; }
    public bool Blitz { get; set; }
    public bool PlayAction { get; set; }
    public bool RunPassOption { get; set; }

    // Clock / Down / Distance — Start
    public string StartClock { get; set; }
    public int StartDown { get; set; }
    public int StartYardsToGain { get; set; }
    public int StartLocationYardLine { get; set; }
    public Guid StartPossessionTeamId { get; set; }

    // Clock / Down / Distance — End
    public string EndClock { get; set; }
    public int EndDown { get; set; }
    public int EndYardsToGain { get; set; }
    public int EndLocationYardLine { get; set; }
    public Guid EndPossessionTeamId { get; set; }
}