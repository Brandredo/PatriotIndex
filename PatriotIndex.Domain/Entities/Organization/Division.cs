namespace PatriotIndex.Domain.Entities.Organization;

public class Division
{
    public Guid Id { get; set; }
    public Guid ConferenceId { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Conference Conference { get; set; } = null!;
    public ICollection<Team> Teams { get; set; } = [];
}
