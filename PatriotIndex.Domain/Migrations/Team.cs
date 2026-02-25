using System;
using System.Collections.Generic;

namespace PatriotIndex.Domain.Migrations;

public partial class Team
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

    public virtual ICollection<Coach> Coaches { get; set; } = new List<Coach>();

    public virtual ICollection<CoinToss> CoinTosses { get; set; } = new List<CoinToss>();

    public virtual Division? Division { get; set; }

    public virtual ICollection<Drife> DrifeDefensiveTeams { get; set; } = new List<Drife>();

    public virtual ICollection<Drife> DrifeOffensiveTeams { get; set; } = new List<Drife>();

    public virtual ICollection<Game> GameAwayTeams { get; set; } = new List<Game>();

    public virtual ICollection<Game> GameHomeTeams { get; set; } = new List<Game>();

    public virtual ICollection<PbpDriveEvent> PbpDriveEventEndPossessionTeams { get; set; } = new List<PbpDriveEvent>();

    public virtual ICollection<PbpDriveEvent> PbpDriveEventStartPossessionTeams { get; set; } = new List<PbpDriveEvent>();

    public virtual ICollection<PbpEventStatistic> PbpEventStatistics { get; set; } = new List<PbpEventStatistic>();

    public virtual ICollection<Player> PlayerDraftTeams { get; set; } = new List<Player>();

    public virtual ICollection<PlayerGameStat> PlayerGameStats { get; set; } = new List<PlayerGameStat>();

    public virtual ICollection<PlayerSeasonStat> PlayerSeasonStats { get; set; } = new List<PlayerSeasonStat>();

    public virtual ICollection<Player> PlayerTeams { get; set; } = new List<Player>();

    public virtual TeamColor? TeamColor { get; set; }

    public virtual ICollection<TeamSeasonStat> TeamSeasonStats { get; set; } = new List<TeamSeasonStat>();

    public virtual Venue? Venue { get; set; }
}
