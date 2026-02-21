namespace PatriotIndex.Domain.DTOs;

public record DriveDto(
    Guid Id, int? PeriodNumber, int? Sequence,
    string? StartReason, string? EndReason,
    int? PlayCount, string? Duration, int? FirstDowns, int? NetYards,
    string? StartClock, string? EndClock,
    Guid? OffensiveTeamId, string? OffensiveTeamAlias,
    Guid? DefensiveTeamId, string? DefensiveTeamAlias,
    int OffensivePoints, int DefensivePoints,
    IReadOnlyList<PlayDto> Plays);

public record PlayDto(
    Guid Id, long? Sequence, string? Clock, string? PlayType, string? Description,
    int HomePoints, int AwayPoints,
    int? Down, int? Yfd,
    Guid? PossessionTeamId, string? PossessionTeamAlias,
    int? StartYardline, string? StartSideAlias,
    int? EndYardline, string? EndSideAlias,
    bool Scoring, bool Turnover, bool FirstDown, bool Penalty,
    bool FakePunt, bool FakeFg, bool ScreenPass, bool PlayAction, bool Rpo,
    string? HashMark, DateTime? WallClock,
    IReadOnlyList<PlayStatDto> Stats);

public record PlayStatDto(
    Guid Id, string StatType, Guid? PlayerId, string? PlayerName,
    Guid? TeamId, string? TeamAlias,
    int? Yards, int? Attempt, int? Complete, int? Touchdown,
    int? Interception, int? Fumble, int? Sack, int? Touchback, string? Category);

public record GamePbpDto(
    Guid GameId, string? Title, string? Status,
    Guid? HomeTeamId, string? HomeTeamAlias, int? HomePoints,
    Guid? AwayTeamId, string? AwayTeamAlias, int? AwayPoints,
    IReadOnlyList<DriveDto> Drives);
