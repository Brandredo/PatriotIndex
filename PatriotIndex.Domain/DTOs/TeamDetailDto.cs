namespace PatriotIndex.Domain.DTOs;

public record TeamDetailDto(
    Guid Id, string Name, string Market, string Alias, string? SrId,
    int? Founded, string? Owner, string? GeneralManager, string? President, string? Mascot,
    string? PrimaryColor, string? SecondaryColor,
    int? ChampionshipsWon, int? ConferenceTitles, int? DivisionTitles, int? PlayoffAppearances,
    VenueDto? Venue, DivisionSummaryDto? Division,
    IReadOnlyList<CoachDto> Coaches);