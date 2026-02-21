namespace PatriotIndex.Domain.Entities;

public class Conference
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Alias { get; set; } = "";

    public ICollection<Division> Divisions { get; set; } = [];
}
