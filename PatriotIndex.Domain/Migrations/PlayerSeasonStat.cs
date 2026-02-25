using System;
using System.Collections.Generic;

namespace PatriotIndex.Domain.Migrations;

public partial class PlayerSeasonStat
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }

    public Guid? TeamId { get; set; }

    public int SeasonYear { get; set; }

    public string SeasonType { get; set; } = null!;

    public int GamesPlayed { get; set; }

    public int GamesStarted { get; set; }

    public int PassAtt { get; set; }

    public int PassCmp { get; set; }

    public int PassYds { get; set; }

    public int PassTd { get; set; }

    public int PassInt { get; set; }

    public double PassRating { get; set; }

    public int PassSacks { get; set; }

    public int PassSackYds { get; set; }

    public int RushAtt { get; set; }

    public int RushYds { get; set; }

    public int RushTd { get; set; }

    public double RushAvg { get; set; }

    public int RushLong { get; set; }

    public int RushFumbles { get; set; }

    public int RecTargets { get; set; }

    public int RecReceptions { get; set; }

    public int RecYds { get; set; }

    public int RecTd { get; set; }

    public double RecAvg { get; set; }

    public int RecLong { get; set; }

    public int RecFumbles { get; set; }

    public int DefTackles { get; set; }

    public int DefAssists { get; set; }

    public double DefSacks { get; set; }

    public int DefInterceptions { get; set; }

    public int DefForcedFumbles { get; set; }

    public int DefPassesDefended { get; set; }

    public int DefQbHits { get; set; }

    public int FgAtt { get; set; }

    public int FgMade { get; set; }

    public int FgLong { get; set; }

    public int XpAtt { get; set; }

    public int XpMade { get; set; }

    public int PuntAtt { get; set; }

    public int PuntYds { get; set; }

    public double PuntAvg { get; set; }

    public virtual Player Player { get; set; } = null!;

    public virtual Team? Team { get; set; }
}
