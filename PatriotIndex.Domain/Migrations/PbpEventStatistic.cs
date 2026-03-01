namespace PatriotIndex.Domain.Migrations;

public class PbpEventStatistic
{
    public Guid EventId { get; set; }

    public string StatType { get; set; } = null!;

    public Guid Id { get; set; }

    public Guid? PlayerId { get; set; }

    public Guid? TeamId { get; set; }

    public int? Yards { get; set; }

    public int? Attempt { get; set; }

    public int? Complete { get; set; }

    public int? Touchdown { get; set; }

    public int? Interception { get; set; }

    public int? Fumble { get; set; }

    public int? Sack { get; set; }

    public int? FirstDown { get; set; }

    public int? Penalty { get; set; }

    public int? PenaltyYards { get; set; }

    public int? ReturnYards { get; set; }

    public int? Touchback { get; set; }

    public string? Category { get; set; }

    public string? ExtraData { get; set; }

    public Guid? DriveEventId { get; set; }

    public long IdBigint { get; set; }

    public virtual PbpDriveEvent? DriveEvent { get; set; }

    public virtual PbpDriveEvent Event { get; set; } = null!;

    public virtual Player? Player { get; set; }

    public virtual Team? Team { get; set; }
}