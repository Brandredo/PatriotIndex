namespace PatriotIndex.Domain.DTOs;

public record PlayerSummaryDto(
    Guid Id, string? Name, string? FirstName, string? LastName,
    string? Jersey, string? Position, string? Status,
    TeamSummaryDto? Team);

public record PlayerDetailDto(
    Guid Id, string? Name, string? FirstName, string? LastName,
    string? Jersey, string? Position, string? Status, int? Experience,
    int? Height, int? Weight, string? BirthDate, string? College, int? RookieYear,
    long? Salary, string? SrId,
    int? DraftYear, int? DraftRound, int? DraftPick, string? DraftTeam,
    TeamSummaryDto? Team,
    IReadOnlyList<PlayerSeasonStatsDto> SeasonStats);

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

public record PlayerSeasonStatsDto(
    Guid Id, int SeasonYear, string SeasonType, int GamesPlayed, int GamesStarted,
    StatBlockDto Stats);

public record PlayerGameLogDto(
    Guid GameId, DateTime? Scheduled, string? Opponent, bool IsHome,
    int? HomePoints, int? AwayPoints, StatBlockDto Stats);
