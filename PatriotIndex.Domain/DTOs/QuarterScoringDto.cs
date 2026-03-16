namespace PatriotIndex.Domain.DTOs;

public record QuarterScoringDto(
    int GamesPlayed,
    IReadOnlyList<QuarterScoreDto> Quarters);

public record QuarterScoreDto(
    int Quarter,
    double TeamAvgPts,
    double OppAvgPts);
