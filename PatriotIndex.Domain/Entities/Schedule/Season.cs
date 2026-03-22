namespace PatriotIndex.Domain.Entities.Schedule;

public class Season
{
    public Guid Id { get; set; }
    public short Year { get; set; }
    public string Type { get; set; } = null!;
    public string? Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<Week> Weeks { get; set; } = [];
    public ICollection<Game> Games { get; set; } = [];
}
