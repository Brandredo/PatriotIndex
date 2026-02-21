namespace PatriotIndex.Domain.DTOs;

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