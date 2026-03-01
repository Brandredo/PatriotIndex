using PatriotIndex.Domain.Enums;

namespace PatriotIndex.Domain.Entities;

public class Player
{
    public Guid Id { get; set; }
    public Guid? TeamId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Name { get; set; }
    public string? Jersey { get; set; }
    public PlayerPosition? Position { get; set; }
    public int? Height { get; set; }
    public decimal? Weight { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? College { get; set; }
    public int? RookieYear { get; set; }
    public string? Status { get; set; }
    public int? Experience { get; set; }
    public long? Salary { get; set; } = null;
    public string? SrId { get; set; }
    public int? DraftYear { get; set; }
    public int? DraftRound { get; set; }
    public int? DraftPick { get; set; }
    public Guid? DraftTeamId { get; set; }

    public Team? Team { get; set; }
    public Team? DraftTeam { get; set; }
    public IEnumerable<PlayerSeasonStats>? SeasonStats { get; set; } = [];
    public IEnumerable<PlayerGameStats>? GameStats { get; set; } = [];
}