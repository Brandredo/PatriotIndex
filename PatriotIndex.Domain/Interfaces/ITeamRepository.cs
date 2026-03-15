using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Interfaces;

public interface ITeamRepository
{
    Task<IReadOnlyList<TeamSummaryDto>> GetAllAsync();
    Task<TeamDetailDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<PlayerRosterDto>> GetRosterAsync(Guid teamId);
    Task<TeamSeasonSummaryDto?> GetSeasonStatsAsync(Guid teamId, int seasonYear, string seasonType);
    Task<IReadOnlyList<TeamPlayerStatsDto>> GetTeamPlayerStatsAsync(Guid teamId, int seasonYear, string seasonType);
    Task<IReadOnlyList<TeamSummaryWithRosterDto>> GetTeamsAndPlayers();
    Task<IReadOnlyList<TeamGameLogDto>> GetTeamGameLogAsync(Guid teamId, int? seasonYear, string? seasonType);
}