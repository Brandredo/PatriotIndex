using PatriotIndex.Domain.Entities.Schedule;

namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class GamePeriod
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public short Number { get; set; }
    public short? Sequence { get; set; }
    public string? PeriodType { get; set; }
    public short? HomePoints { get; set; }
    public short? AwayPoints { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Game Game { get; set; } = null!;
    public ICollection<GamePlay> Plays { get; set; } = [];
}
