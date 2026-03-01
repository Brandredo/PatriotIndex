namespace PatriotIndex.Domain.Migrations;

public class Division
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Alias { get; set; } = null!;

    public Guid ConferenceId { get; set; }

    public virtual Conference Conference { get; set; } = null!;

    public virtual IEnumerable<Team> Teams { get; set; } = new List<Team>();
}