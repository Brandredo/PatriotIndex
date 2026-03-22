namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class PlayDetail
{
    public Guid Id { get; set; }
    public Guid PlayId { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public int? Sequence { get; set; }
    public string? Direction { get; set; }
    public int? Yards { get; set; }
    public string? Result { get; set; }
    public string? StartAlias { get; set; }
    public int? StartYardline { get; set; }
    public string? EndAlias { get; set; }
    public int? EndYardline { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public GamePlay Play { get; set; } = null!;
    public ICollection<PlayDetailPlayer> Players { get; set; } = [];
}
