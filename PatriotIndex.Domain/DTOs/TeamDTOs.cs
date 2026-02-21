namespace PatriotIndex.Domain.DTOs;

public record VenueDto(
    Guid Id, string Name, string? City, string? State, string? Country,
    int? Capacity, string? Surface, string? RoofType, string? Lat, string? Lng);

public record ConferenceSummaryDto(Guid Id, string Name, string Alias);
public record DivisionSummaryDto(Guid Id, string Name, string Alias, ConferenceSummaryDto Conference);

public record TeamSummaryDto(
    Guid Id, string Name, string Market, string Alias,
    string? PrimaryColor, string? SecondaryColor,
    DivisionSummaryDto? Division);

public record CoachDto(Guid Id, string FullName, string? Position);

public record TeamDetailDto(
    Guid Id, string Name, string Market, string Alias, string? SrId,
    int? Founded, string? Owner, string? GeneralManager, string? President, string? Mascot,
    string? PrimaryColor, string? SecondaryColor,
    int? ChampionshipsWon, int? ConferenceTitles, int? DivisionTitles, int? PlayoffAppearances,
    VenueDto? Venue, DivisionSummaryDto? Division,
    IReadOnlyList<CoachDto> Coaches);

public record PlayerRosterDto(
    Guid Id, string? Name, string? FirstName, string? LastName,
    string? Jersey, string? Position, string? Status, int? Experience);
