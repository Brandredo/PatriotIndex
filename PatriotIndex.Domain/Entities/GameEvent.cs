using System.ComponentModel.DataAnnotations;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Entities;

public class GameEvent : IGameEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid  GameId   { get; set; }
    public Guid  DriveId  { get; set; }
    public Guid? PeriodId { get; set; }

    [Required]
    [MaxLength(50)]
    public string EventType { get; set; } = string.Empty;

    public decimal Sequence { get; set; }

    [Required]
    [MaxLength(10)]
    public string Clock { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public Game    Game   { get; set; } = null!;
    public Drive   Drive  { get; set; } = null!;
    public Period? Period { get; set; }
}
