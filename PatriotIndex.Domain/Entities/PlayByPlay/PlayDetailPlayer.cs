using PatriotIndex.Domain.Entities.People;

namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class PlayDetailPlayer
{
    public Guid Id { get; set; }
    public Guid PlayDetailId { get; set; }
    public Guid? PlayerId { get; set; }
    public string? Role { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayDetail PlayDetail { get; set; } = null!;
    public Player? Player { get; set; }
}
