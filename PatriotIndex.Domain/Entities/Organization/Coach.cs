namespace PatriotIndex.Domain.Entities.Organization;

public class Coach
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string FullName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Position { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Team Team { get; set; } = null!;
}
