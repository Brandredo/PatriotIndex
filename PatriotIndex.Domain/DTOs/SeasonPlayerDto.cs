using System.Text.Json.Serialization;

namespace PatriotIndex.Domain.DTOs;

public record SeasonPlayerDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("jersey")] string? Jersey,
    [property: JsonPropertyName("position")] string? Position,
    [property: JsonPropertyName("sr_id")] string? SrId,
    [property: JsonPropertyName("games_played")] int GamesPlayed,
    [property: JsonPropertyName("games_started")] int GamesStarted,
    [property: JsonPropertyName("rushing")] SeasonRushingDto? Rushing,
    [property: JsonPropertyName("passing")] SeasonPassingDto? Passing,
    [property: JsonPropertyName("receiving")] SeasonReceivingDto? Receiving,
    [property: JsonPropertyName("defense")] SeasonDefenseDto? Defense,
    [property: JsonPropertyName("field_goals")] SeasonFieldGoalsDto? FieldGoals,
    [property: JsonPropertyName("extra_points")] SeasonPlayerExtraPointsDto? ExtraPoints,
    [property: JsonPropertyName("punts")] SeasonPuntsDto? Punts,
    [property: JsonPropertyName("kickoffs")] SeasonKickoffsDto? Kickoffs,
    [property: JsonPropertyName("punt_returns")] SeasonPuntReturnsDto? PuntReturns,
    [property: JsonPropertyName("kick_returns")] SeasonKickReturnsDto? KickReturns,
    [property: JsonPropertyName("misc_returns")] SeasonMiscReturnsDto? MiscReturns,
    [property: JsonPropertyName("fumbles")] SeasonFumblesDto? Fumbles,
    [property: JsonPropertyName("penalties")] SeasonPenaltiesDto? Penalties,
    [property: JsonPropertyName("int_returns")] SeasonIntReturnsDto? IntReturns
);
