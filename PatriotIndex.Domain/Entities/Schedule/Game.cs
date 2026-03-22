using PatriotIndex.Domain.Entities.Organization;

namespace PatriotIndex.Domain.Entities.Schedule;

public class Game
{
    public Guid Id { get; set; }
    public Guid SeasonId { get; set; }
    public Guid? WeekId { get; set; }
    public Guid? VenueId { get; set; }
    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }
    public DateTimeOffset ScheduledAt { get; set; }
    public string? Status { get; set; }
    public string? GameType { get; set; }
    public int? Attendance { get; set; }
    public short? HomePoints { get; set; }
    public short? AwayPoints { get; set; }
    public short? HomeWins { get; set; }
    public short? HomeLosses { get; set; }
    public short? HomeTies { get; set; }
    public short? AwayWins { get; set; }
    public short? AwayLosses { get; set; }
    public short? AwayTies { get; set; }
    public string? Clock { get; set; }
    public short? Quarter { get; set; }
    public string? Duration { get; set; }
    public string? Network { get; set; }
    public short? UsedTimeoutsHome { get; set; }
    public short? UsedTimeoutsAway { get; set; }
    public short? UsedChallengesHome { get; set; }
    public short? UsedChallengesAway { get; set; }
    public string? EntryMode { get; set; }
    public bool? NeutralSite { get; set; }
    public bool? ConferenceGame { get; set; }
    public string? Title { get; set; }
    public Guid? ParentId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Season Season { get; set; } = null!;
    public Week? Week { get; set; }
    public Venue? Venue { get; set; }
    public Team HomeTeam { get; set; } = null!;
    public Team AwayTeam { get; set; } = null!;
    public GameWeather? Weather { get; set; }
}
