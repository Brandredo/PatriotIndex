namespace PatriotIndex.Domain.DTOs;

public record TeamGameLogDto(
    Guid GameId,
    DateTime? Scheduled,
    string? Opponent,
    bool IsHome,
    int? TeamPoints,
    int? OpponentPoints,
    string? Result,
    string? PossessionTime,
    int? TotalYards,
    int? Turnovers,
    int? Penalties,
    int? RushPlays,
    int? PlayCount,
    double? AvgGain
);
