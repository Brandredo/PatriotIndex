namespace PatriotIndex.Domain.Migrations;

public class Team
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Market { get; set; } = null!;

    public string Alias { get; set; } = null!;

    public string? SrId { get; set; }

    public short? Founded { get; set; }

    public string? Owner { get; set; }

    public string? GeneralManager { get; set; }

    public string? President { get; set; }

    public string? Mascot { get; set; }

    public Guid? VenueId { get; set; }

    public Guid? DivisionId { get; set; }

    public string? SecondaryColor { get; set; }

    public int? ChampionshipsWon { get; set; }

    public int? ConferenceTitles { get; set; }

    public int? DivisionTitles { get; set; }

    public int? PlayoffAppearances { get; set; }

    public string? ChampionshipSeasons { get; set; }

    public virtual IEnumerable<Coach> Coaches { get; set; } = new List<Coach>();

    public virtual IEnumerable<CoinToss> CoinTosses { get; set; } = new List<CoinToss>();

    public virtual Division? Division { get; set; }

    public virtual IEnumerable<Drife> DrifeDefensiveTeams { get; set; } = new List<Drife>();

    public virtual IEnumerable<Drife> DrifeOffensiveTeams { get; set; } = new List<Drife>();

    public virtual IEnumerable<Game> GameAwayTeams { get; set; } = new List<Game>();

    public virtual IEnumerable<Game> GameHomeTeams { get; set; } = new List<Game>();

    public virtual IEnumerable<PbpDriveEvent> PbpDriveEventEndPossessionTeams { get; set; } = new List<PbpDriveEvent>();

    public virtual IEnumerable<PbpDriveEvent> PbpDriveEventStartPossessionTeams { get; set; } =
        new List<PbpDriveEvent>();

    public virtual IEnumerable<PbpEventStatistic> PbpEventStatistics { get; set; } = new List<PbpEventStatistic>();

    public virtual IEnumerable<Player> PlayerDraftTeams { get; set; } = new List<Player>();

    public virtual IEnumerable<PlayerGameStat> PlayerGameStats { get; set; } = new List<PlayerGameStat>();

    public virtual IEnumerable<PlayerSeasonStat> PlayerSeasonStats { get; set; } = new List<PlayerSeasonStat>();

    public virtual IEnumerable<Player> PlayerTeams { get; set; } = new List<Player>();

    public virtual TeamColor? TeamColor { get; set; }

    public virtual IEnumerable<TeamSeasonStat> TeamSeasonStats { get; set; } = new List<TeamSeasonStat>();

    public virtual Venue? Venue { get; set; }
}