namespace PatriotIndex.Domain.Entities;

public class PlayerGameStats
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public Guid GameId { get; set; }
    public Guid? TeamId { get; set; }
    // Passing
    public int PassAtt { get; set; }
    public int PassCmp { get; set; }
    public int PassYds { get; set; }
    public int PassTd { get; set; }
    public int PassInt { get; set; }
    public double PassRating { get; set; }
    public int PassSacks { get; set; }
    public int PassSackYds { get; set; }
    // Rushing
    public int RushAtt { get; set; }
    public int RushYds { get; set; }
    public int RushTd { get; set; }
    public double RushAvg { get; set; }
    public int RushLong { get; set; }
    public int RushFumbles { get; set; }
    // Receiving
    public int RecTargets { get; set; }
    public int RecReceptions { get; set; }
    public int RecYds { get; set; }
    public int RecTd { get; set; }
    public double RecAvg { get; set; }
    public int RecLong { get; set; }
    public int RecFumbles { get; set; }
    // Defense
    public int DefTackles { get; set; }
    public int DefAssists { get; set; }
    public double DefSacks { get; set; }
    public int DefInterceptions { get; set; }
    public int DefForcedFumbles { get; set; }
    public int DefPassesDefended { get; set; }
    public int DefQbHits { get; set; }
    // Special Teams
    public int FgAtt { get; set; }
    public int FgMade { get; set; }
    public int FgLong { get; set; }
    public int XpAtt { get; set; }
    public int XpMade { get; set; }
    public int PuntAtt { get; set; }
    public int PuntYds { get; set; }
    public double PuntAvg { get; set; }

    public Player? Player { get; set; }
    public Game? Game { get; set; }
    public Team? Team { get; set; }
}
