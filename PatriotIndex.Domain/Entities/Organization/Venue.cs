namespace PatriotIndex.Domain.Entities.Organization;

public class Venue
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public string? Zip { get; set; }
    public int? Capacity { get; set; }
    public string? Surface { get; set; }
    public string? RoofType { get; set; }
    public decimal? Lat { get; set; }
    public decimal? Lng { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<Team> Teams { get; set; } = [];
}
