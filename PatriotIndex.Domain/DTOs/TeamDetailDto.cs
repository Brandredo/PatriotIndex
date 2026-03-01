using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.DTOs;

public record TeamDetailDto(
    Guid Id,
    string Name,
    string Market,
    string Alias,
    string? SrId,
    int? Founded,
    string? Owner,
    string? GeneralManager,
    string? President,
    string? Mascot,
    TeamColors? Colors,
    int? ChampionshipsWon,
    int? ConferenceTitles,
    int? DivisionTitles,
    int? PlayoffAppearances,
    VenueDto? Venue,
    DivisionSummaryDto? Division,
    IReadOnlyList<CoachDto> Coaches);