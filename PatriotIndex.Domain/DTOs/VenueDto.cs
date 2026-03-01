namespace PatriotIndex.Domain.DTOs;

public record VenueDto(
    Guid Id,
    string Name,
    string? City,
    string? State,
    string? Country,
    int? Capacity,
    string? Surface,
    string? RoofType,
    string? Lat,
    string? Lng);