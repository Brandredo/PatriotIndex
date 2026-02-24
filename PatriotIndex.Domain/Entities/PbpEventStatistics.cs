using System.Text.Json;

namespace PatriotIndex.Domain.Entities;

public class PbpEventStatistics
{
    public long Id { get; set; }
    public Guid EventId { get; set; }
    
    // ------------
    public string StatType { get; set; } = "";
    public Guid? PlayerId { get; set; }
    public Guid? TeamId { get; set; }
    public int? Yards { get; set; }
    public int? Attempt { get; set; }
    public int? Complete { get; set; }
    public int? Touchdown { get; set; }
    public int? Interception { get; set; }
    public int? Fumble { get; set; }
    public int? Sack { get; set; }
    public int? FirstDown { get; set; }
    public int? Penalty { get; set; }
    public int? PenaltyYards { get; set; }
    public int? ReturnYards { get; set; }
    public int? Touchback { get; set; }
    public string? Category { get; set; }
    public JsonDocument? ExtraData { get; set; }
    public DriveEvent? DriveEvent { get; set; }
    public Player? Player { get; set; }
    public Team? Team { get; set; }
}