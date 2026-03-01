namespace PatriotIndex.Domain.Migrations;

public class TeamColor
{
    public Guid Id { get; set; }

    public string PrimaryColor { get; set; } = null!;

    public string? SecondaryColor { get; set; }

    public virtual Team IdNavigation { get; set; } = null!;
}