namespace PatriotIndex.Domain.Entities;

public class Coach
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string FullName { get; set; } = "";
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Position { get; set; }

    public Team? Team { get; set; }
}