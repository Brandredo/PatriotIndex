namespace PatriotIndex.Domain.Interfaces;

public interface IGameEvent
{
    Guid    Id          { get; }
    Guid    GameId      { get; }
    Guid    DriveId     { get; }
    decimal    Sequence    { get; }
    string  Clock       { get; }
    string? Description { get; }
}
