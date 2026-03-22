namespace PatriotIndex.Domain.Entities.Organization;

public class Team
{
    public Guid Id { get; set; }
    public Guid DivisionId { get; set; }
    public Guid? VenueId { get; set; }
    public string Name { get; set; } = null!;
    public string Alias { get; set; } = null!;
    public string? Market { get; set; }
    public string? Mascot { get; set; }
    public string? Owner { get; set; }
    public short? Founded { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Division Division { get; set; } = null!;
    public Venue? Venue { get; set; }
    public ICollection<Coach> Coaches { get; set; } = [];
}
