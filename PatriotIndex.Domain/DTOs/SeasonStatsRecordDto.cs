using System.Text.Json.Serialization;

namespace PatriotIndex.Domain.DTOs;

public record SeasonStatsRecordDto(
    [property: JsonPropertyName("games_played")] int GamesPlayed,
    [property: JsonPropertyName("touchdowns")] SeasonTouchdownsDto? Touchdowns,
    [property: JsonPropertyName("rushing")] SeasonRushingDto? Rushing,
    [property: JsonPropertyName("passing")] SeasonPassingDto? Passing,
    [property: JsonPropertyName("receiving")] SeasonReceivingDto? Receiving,
    [property: JsonPropertyName("defense")] SeasonDefenseDto? Defense,
    [property: JsonPropertyName("field_goals")] SeasonFieldGoalsDto? FieldGoals,
    [property: JsonPropertyName("kickoffs")] SeasonKickoffsDto? Kickoffs,
    [property: JsonPropertyName("kick_returns")] SeasonKickReturnsDto? KickReturns,
    [property: JsonPropertyName("punts")] SeasonPuntsDto? Punts,
    [property: JsonPropertyName("punt_returns")] SeasonPuntReturnsDto? PuntReturns,
    [property: JsonPropertyName("interceptions")] SeasonInterceptionsDto? Interceptions,
    [property: JsonPropertyName("int_returns")] SeasonIntReturnsDto? IntReturns,
    [property: JsonPropertyName("fumbles")] SeasonFumblesDto? Fumbles,
    [property: JsonPropertyName("first_downs")] SeasonFirstDownsDto? FirstDowns,
    [property: JsonPropertyName("penalties")] SeasonPenaltiesDto? Penalties,
    [property: JsonPropertyName("misc_returns")] SeasonMiscReturnsDto? MiscReturns,
    [property: JsonPropertyName("extra_points")] SeasonTeamExtraPointsDto? ExtraPoints,
    [property: JsonPropertyName("efficiency")] SeasonEfficiencyDto? Efficiency
);
