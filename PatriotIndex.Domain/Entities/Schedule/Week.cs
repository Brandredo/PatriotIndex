namespace PatriotIndex.Domain.Entities.Schedule;

public class Week
{
    public Guid Id { get; set; }
    public Guid SeasonId { get; set; }
    public short Sequence { get; set; }
    public string? Title { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Season Season { get; set; } = null!;
    public ICollection<Game> Games { get; set; } = [];
}
