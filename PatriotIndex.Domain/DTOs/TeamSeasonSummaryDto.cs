namespace PatriotIndex.Domain.DTOs;

public record TeamSeasonSummaryDto(
    int GamesPlayed,
    double TeamPtsPerGame,
    int TeamTotalPoints,
    double OppPtsPerGame,
    int OppTotalPoints,
    SeasonStatsRecordDto Team,       // ALL stats from TeamSeasonStats.Record
    SeasonStatsRecordDto Opponents,  // ALL stats from TeamSeasonStats.Opponents
    StatBlockDto Stats               // existing player-stat block — category cards unchanged
);
