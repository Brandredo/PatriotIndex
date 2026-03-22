using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class GamePlay
{
    public Guid Id { get; set; }
    public Guid DriveId { get; set; }
    public Guid GameId { get; set; }
    public Guid PeriodId { get; set; }
    public string? Type { get; set; }
    public string? Clock { get; set; }
    public decimal? Sequence { get; set; }
    public string? PlayType { get; set; }
    public string? Description { get; set; }
    public short? AwayPoints { get; set; }
    public short? HomePoints { get; set; }
    public bool? Blitz { get; set; }
    public string? Huddle { get; set; }
    public short? MenInBox { get; set; }
    public string? QbAtSnap { get; set; }
    public string? PassRoute { get; set; }
    public string? HashMark { get; set; }
    public bool? PlayAction { get; set; }
    public bool? ScreenPass { get; set; }
    public bool? RunPassOption { get; set; }
    public bool? FakePunt { get; set; }
    public bool? FakeFieldGoal { get; set; }
    public bool? Official { get; set; }
    public DateTimeOffset? WallClock { get; set; }
    public string? PlayDirection { get; set; }
    public short? LeftTightends { get; set; }
    public short? RightTightends { get; set; }
    public short? RunningLane { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public GameDrive Drive { get; set; } = null!;
    public Game Game { get; set; } = null!;
    public GamePeriod Period { get; set; } = null!;
    public ICollection<PlaySituation> Situations { get; set; } = [];
    public ICollection<PlayPlayerStats> PlayerStats { get; set; } = [];
    public ICollection<PlayDetail> Details { get; set; } = [];
}
