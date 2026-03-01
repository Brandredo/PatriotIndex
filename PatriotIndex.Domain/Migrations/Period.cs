namespace PatriotIndex.Domain.Migrations;

public class Period
{
    public Guid Id { get; set; }

    public int Number { get; set; }

    public Guid GameId { get; set; }

    public string Type { get; set; } = null!;

    public long Sequence { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int HomeScore { get; set; }

    public int AwayScore { get; set; }

    public Guid? GameId1 { get; set; }

    public virtual IEnumerable<CoinToss> CoinTossPeriodId1Navigations { get; set; } = new List<CoinToss>();

    public virtual IEnumerable<CoinToss> CoinTossPeriods { get; set; } = new List<CoinToss>();

    public virtual IEnumerable<Drife> Drives { get; set; } = new List<Drife>();

    public virtual Game Game { get; set; } = null!;

    public virtual Game? GameId1Navigation { get; set; }

    public virtual IEnumerable<PbpDriveEvent> PbpDriveEvents { get; set; } = new List<PbpDriveEvent>();
}