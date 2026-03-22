namespace PatriotIndex.Domain.Entities.Organization;

public class Conference
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<Division> Divisions { get; set; } = [];
}
