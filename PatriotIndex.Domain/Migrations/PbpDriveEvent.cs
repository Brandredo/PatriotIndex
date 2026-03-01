namespace PatriotIndex.Domain.Migrations;

public class PbpDriveEvent
{
    public Guid Id { get; set; }

    public Guid DriveId { get; set; }

    public string EventType { get; set; } = null!;

    public int DriveType { get; set; }

    public decimal Sequence { get; set; }

    public string Clock { get; set; } = null!;

    public string WallClock { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int HomeScore { get; set; }

    public int AwayScore { get; set; }

    public string? PlayType { get; set; }

    public string PassRoute { get; set; } = null!;

    public string? QbSnap { get; set; }

    public string? Huddle { get; set; }

    public int? MenInBox { get; set; }

    public int? LeftTightEnds { get; set; }

    public int? RightTightEnds { get; set; }

    public string? HashMark { get; set; }

    public int? PlayersRushed { get; set; }

    public string? PlayDirection { get; set; }

    public string? PocketLocation { get; set; }

    public bool FakePunt { get; set; }

    public bool FakeFieldGoal { get; set; }

    public bool ScreenPass { get; set; }

    public bool Blitz { get; set; }

    public bool PlayAction { get; set; }

    public bool RunPassOption { get; set; }

    public string? StartClock { get; set; }

    public int StartDown { get; set; }

    public int StartYardsToGain { get; set; }

    public int StartLocationYardLine { get; set; }

    public Guid StartPossessionTeamId { get; set; }

    public string? EndClock { get; set; }

    public int EndDown { get; set; }

    public int EndYardsToGain { get; set; }

    public int EndLocationYardLine { get; set; }

    public Guid EndPossessionTeamId { get; set; }

    public Guid PeriodId { get; set; }

    public virtual Drife Drive { get; set; } = null!;

    public virtual Team EndPossessionTeam { get; set; } = null!;

    public virtual IEnumerable<PbpEventStatistic> PbpEventStatisticDriveEvents { get; set; } =
        new List<PbpEventStatistic>();

    public virtual IEnumerable<PbpEventStatistic> PbpEventStatisticEvents { get; set; } = new List<PbpEventStatistic>();

    public virtual Period Period { get; set; } = null!;

    public virtual Team StartPossessionTeam { get; set; } = null!;
}