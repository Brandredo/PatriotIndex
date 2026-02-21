using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Enums;

namespace PatriotIndex.Domain.Interfaces;

public interface IPlayerRepository
{
    Task<IReadOnlyList<PlayerSummaryDto>> SearchAsync(string? search, Guid? teamId, PlayerPosition? position, string? status);
    Task<PlayerDetailDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<PlayerGameLogDto>> GetGameLogAsync(Guid playerId, int? seasonYear, string? seasonType);
}
