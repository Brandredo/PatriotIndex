using System.Text.Json.Serialization;

namespace PatriotIndex.Domain.DTOs;

public record SeasonalStatsApiResponse(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("market")] string Market,
    [property: JsonPropertyName("alias")] string Alias,
    [property: JsonPropertyName("sr_id")] string? SrId,
    [property: JsonPropertyName("season")] SeasonDto Season,
    [property: JsonPropertyName("record")] SeasonStatsRecordDto? Record,
    [property: JsonPropertyName("opponents")] SeasonStatsRecordDto? Opponents,
    [property: JsonPropertyName("players")] IReadOnlyList<SeasonPlayerDto> Players
);

public record SeasonDto(
    [property: JsonPropertyName("id")] Guid? Id,
    [property: JsonPropertyName("year")] int Year,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("name")] string? Name
);
