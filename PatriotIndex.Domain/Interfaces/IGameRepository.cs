using PatriotIndex.Domain.DTOs;

namespace PatriotIndex.Domain.Interfaces;

public interface IGameRepository
{
    Task<IReadOnlyList<GameSummaryDto>> GetGamesAsync(
        int? season, string? seasonType, int? week, Guid? teamId);

    Task<GamePbpDto?> GetGamePbpAsync(Guid gameId);
}
