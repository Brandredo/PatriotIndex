namespace PatriotIndex.Domain.Entities.Schedule;

public class GameWeather
{
    public Guid GameId { get; set; }
    public short? Temp { get; set; }
    public short? Humidity { get; set; }
    public string? Condition { get; set; }
    public short? WindSpeed { get; set; }
    public string? WindDirection { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Game Game { get; set; } = null!;
}
