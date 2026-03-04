namespace PatriotIndex.Domain.Interfaces;

public interface IGameEvent
{
    Guid    Id          { get; }
    Guid    GameId      { get; }
    Guid    DriveId     { get; }
    long    Sequence    { get; }
    string  Clock       { get; }
    string? Description { get; }
}
