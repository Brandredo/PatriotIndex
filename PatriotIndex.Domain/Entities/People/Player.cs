using PatriotIndex.Domain.Entities.Organization;

namespace PatriotIndex.Domain.Entities.People;

public class Player
{
    public Guid Id { get; set; }
    public Guid? TeamId { get; set; }
    public string Name { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AbbrName { get; set; }
    public string? Jersey { get; set; }
    public string Position { get; set; } = null!;
    public string? Status { get; set; }
    public short? Height { get; set; }
    public decimal? Weight { get; set; }
    public int? Salary { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? BirthPlace { get; set; }
    public string? College { get; set; }
    public string? CollegeConf { get; set; }
    public string? HighSchool { get; set; }
    public short? Experience { get; set; }
    public short? RookieYear { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Team? Team { get; set; }
    public PlayerDraft? Draft { get; set; }
}
