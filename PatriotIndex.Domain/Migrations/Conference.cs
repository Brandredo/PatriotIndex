namespace PatriotIndex.Domain.Migrations;

public class Conference
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Alias { get; set; } = null!;

    public virtual IEnumerable<Division> Divisions { get; set; } = new List<Division>();
}