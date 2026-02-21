namespace PatriotIndex.Domain.DTOs;

public record StatBlockDto(
    // Passing
    int PassAtt, int PassCmp, int PassYds, int PassTd, int PassInt,
    double PassRating, int PassSacks, int PassSackYds,
    // Rushing
    int RushAtt, int RushYds, int RushTd, double RushAvg, int RushLong,
    // Receiving
    int RecTargets, int RecReceptions, int RecYds, int RecTd, double RecAvg, int RecLong,
    // Defense
    int DefTackles, int DefAssists, double DefSacks, int DefInterceptions,
    int DefForcedFumbles, int DefPassesDefended, int DefQbHits,
    // Special Teams
    int FgAtt, int FgMade, int FgLong, int XpAtt, int XpMade,
    int PuntAtt, int PuntYds, double PuntAvg);