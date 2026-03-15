using System.ComponentModel.DataAnnotations;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Entities;

public class Play : IGameEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid  GameId   { get; set; }
    public Guid  DriveId  { get; set; }
    public Guid? PeriodId { get; set; }

    // ── Searchable SQL columns ──────────────────────────────────────────────

    [Required]
    [MaxLength(50)]
    public string PlayType { get; set; } = string.Empty;

    public decimal Sequence { get; set; }

    [Required]
    [MaxLength(10)]
    public string Clock { get; set; } = string.Empty;

    public DateTimeOffset? WallClock { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public int HomePoints { get; set; }
    public int AwayPoints { get; set; }

    // ── Pre-snap formation (searchable SQL columns) ─────────────────────────

    [MaxLength(20)]
    public string? QbSnap { get; set; }

    [MaxLength(20)]
    public string? Huddle { get; set; }

    public int? MenInBox       { get; set; }
    public int? LeftTightEnds  { get; set; }
    public int? RightTightEnds { get; set; }

    [MaxLength(10)]
    public string? HashMark { get; set; }

    public bool? Blitz         { get; set; }
    public bool? PlayAction    { get; set; }
    public bool? RunPassOption { get; set; }
    public bool? ScreenPass    { get; set; }
    public bool? FakePunt      { get; set; }
    public bool? FakeFieldGoal { get; set; }

    [MaxLength(20)]
    public string? PlayDirection { get; set; }

    [MaxLength(50)]
    public string? PassRoute { get; set; }

    // ── Situation Value Objects (owned, flattened SQL columns) ──────────────
    public GameSituation StartSituation { get; set; } = new();
    public GameSituation EndSituation   { get; set; } = new();

    // ── JSONB collections ───────────────────────────────────────────────────
    public List<PlayStat>   Statistics { get; set; } = new();
    public List<PlayDetail> Details    { get; set; } = new();

    // ── Navigation ──────────────────────────────────────────────────────────
    public Game    Game   { get; set; } = null!;
    public Drive   Drive  { get; set; } = null!;
    public Period? Period { get; set; }
}
