namespace PatriotIndex.Domain.DTOs;

public record PlayCallStatsDto(
    int GamesPlayed,
    int TotalPlays,
    PlayCallRushDto Rush,
    PlayCallPassDto Pass);

public record PlayCallRushDto(int Total, int Scramble, int KneelDown, int Rpo, int Standard);
public record PlayCallPassDto(int Total, int PlayAction, int Screen, int Rpo, int Standard);
