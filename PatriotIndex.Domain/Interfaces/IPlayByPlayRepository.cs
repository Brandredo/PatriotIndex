using PatriotIndex.Domain.DTOs;

namespace PatriotIndex.Domain.Interfaces;

public interface IPlayByPlayRepository
{
    Task<GamePbpDto?> GetGamePbpAsync(Guid gameId);
    Task<DriveDto?> GetDriveAsync(Guid driveId);
}