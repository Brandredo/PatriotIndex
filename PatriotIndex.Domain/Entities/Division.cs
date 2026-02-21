namespace PatriotIndex.Domain.Entities;

public class Division
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Alias { get; set; } = "";
    public Guid ConferenceId { get; set; }

    public Conference? Conference { get; set; }
    public ICollection<Team> Teams { get; set; } = [];
}
