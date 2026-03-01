using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Enums;

namespace PatriotIndex.Domain.Interfaces;

public interface ILeaderboardRepository
{
    Task<LeaderboardDto> GetLeaderboardAsync(string category, int seasonYear, string seasonType,
        PlayerPosition? position, int limit);
}