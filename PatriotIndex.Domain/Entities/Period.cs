namespace PatriotIndex.Domain.Entities;

public class Period
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public Guid GameId { get; set; }
    public string Type { get; set; } = "";
    public long Sequence { get; set; }
    // public DateTime StartTime { get; set; }
    // public DateTime EndTime { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    
    // Navigation properties
    public Game? Game { get; set; }
    public ICollection<Drive> Drives { get; set; } = [];
}