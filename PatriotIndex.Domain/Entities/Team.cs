namespace PatriotIndex.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Market { get; set; } = "";
    public string Alias { get; set; } = "";
    public string? SrId { get; set; }
    public int? Founded { get; set; }
    public string? Owner { get; set; }
    public string? GeneralManager { get; set; }
    public string? President { get; set; }
    public string? Mascot { get; set; }
    public Guid? VenueId { get; set; }
    public Guid? DivisionId { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public int? ChampionshipsWon { get; set; }
    public int? ConferenceTitles { get; set; }
    public int? DivisionTitles { get; set; }
    public int? PlayoffAppearances { get; set; }

    public Division? Division { get; set; }
    public Venue? Venue { get; set; }
    public ICollection<Coach> Coaches { get; set; } = [];
    public ICollection<Player> Players { get; set; } = [];
}
