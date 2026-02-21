namespace PatriotIndex.Domain.Entities;

public class Play
{
    public Guid Id { get; set; }
    public Guid DriveId { get; set; }
    public Guid GameId { get; set; }
    public long? Sequence { get; set; }
    public string? Clock { get; set; }
    public string? PlayType { get; set; }
    public string? Description { get; set; }
    public int HomePoints { get; set; }
    public int AwayPoints { get; set; }
    public int? Down { get; set; }
    public int? Yfd { get; set; }
    public Guid? PossessionTeamId { get; set; }
    public int? StartYardline { get; set; }
    public Guid? StartSideId { get; set; }
    public int? EndYardline { get; set; }
    public Guid? EndSideId { get; set; }
    public bool Scoring { get; set; }
    public bool Turnover { get; set; }
    public bool FirstDown { get; set; }
    public bool Penalty { get; set; }
    public bool FakePunt { get; set; }
    public bool FakeFg { get; set; }
    public bool ScreenPass { get; set; }
    public bool PlayAction { get; set; }
    public bool Rpo { get; set; }
    public string? HashMark { get; set; }
    public bool Official { get; set; }
    public DateTime? WallClock { get; set; }

    public Drive? Drive { get; set; }
    public Game? Game { get; set; }
    public ICollection<PlayStat> PlayStats { get; set; } = [];
}
