namespace PatriotIndex.Domain.Entities;

public class Game
{
    public Guid Id { get; set; }
    public string? SrId { get; set; }
    public string? Status { get; set; }
    public DateTime? Scheduled { get; set; }
    public int? Attendance { get; set; }
    public string? GameType { get; set; }
    public string? Title { get; set; }
    public string? Duration { get; set; }
    public int? SeasonYear { get; set; }
    public string? SeasonType { get; set; }
    public Guid? SeasonId { get; set; }
    public int? WeekSequence { get; set; }
    public string? WeekTitle { get; set; }
    public Guid? HomeTeamId { get; set; }
    public Guid? AwayTeamId { get; set; }
    public int? HomePoints { get; set; }
    public int? AwayPoints { get; set; }
    public Guid? VenueId { get; set; }
    public string? WeatherCondition { get; set; }
    public int? WeatherTemp { get; set; }
    public int? WeatherHumidity { get; set; }
    public int? WeatherWindSpeed { get; set; }
    public string? WeatherWindDirection { get; set; }
    public string? BroadcastNetwork { get; set; }
    public bool? NeutralSite { get; set; }
    public bool? ConferenceGame { get; set; }

    public Team? HomeTeam { get; set; }
    public Team? AwayTeam { get; set; }
    public Venue? Venue { get; set; }
    public ICollection<Drive> Drives { get; set; } = [];
    public ICollection<Period> Periods { get; set; } = [];
    public Guid? WeekId { get; set; }
}