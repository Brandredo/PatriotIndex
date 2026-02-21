using PatriotIndex.Domain.DTOs;

namespace PatriotIndex.Domain.Interfaces;

public interface IPlayerRepository
{
    Task<IReadOnlyList<PlayerSummaryDto>> SearchAsync(string? search, Guid? teamId, string? position, string? status);
    Task<PlayerDetailDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<PlayerGameLogDto>> GetGameLogAsync(Guid playerId, int? seasonYear, string? seasonType);
}
