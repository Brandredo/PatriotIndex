using PatriotIndex.Domain.DTOs;

namespace PatriotIndex.Domain.Interfaces;

public interface ILeaderboardRepository
{
    Task<LeaderboardDto> GetLeaderboardAsync(string category, int seasonYear, string seasonType, string? position, int limit);
}
