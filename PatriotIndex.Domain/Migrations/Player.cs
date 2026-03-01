namespace PatriotIndex.Domain.Migrations;

public class Player
{
    public Guid Id { get; set; }

    public Guid? TeamId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Name { get; set; }

    public string? Jersey { get; set; }

    public int? Position { get; set; }

    public int? Height { get; set; }

    public decimal? Weight { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? College { get; set; }

    public int? RookieYear { get; set; }

    public string? Status { get; set; }

    public int? Experience { get; set; }

    public long? Salary { get; set; }

    public string? SrId { get; set; }

    public int? DraftYear { get; set; }

    public int? DraftRound { get; set; }

    public int? DraftPick { get; set; }

    public Guid? DraftTeamId { get; set; }

    public virtual Team? DraftTeam { get; set; }

    public virtual IEnumerable<PbpEventStatistic> PbpEventStatistics { get; set; } = new List<PbpEventStatistic>();

    public virtual IEnumerable<PlayerGameStat> PlayerGameStats { get; set; } = new List<PlayerGameStat>();

    public virtual IEnumerable<PlayerSeasonStat> PlayerSeasonStats { get; set; } = new List<PlayerSeasonStat>();

    public virtual Team? Team { get; set; }
}