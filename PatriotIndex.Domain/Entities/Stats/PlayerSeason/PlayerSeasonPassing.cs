namespace PatriotIndex.Domain.Entities.Stats.PlayerSeason;

public class PlayerSeasonPassing
{
    public Guid PlayerSeasonId { get; set; }
    public int? Attempts { get; set; }
    public int? Completions { get; set; }
    public decimal? CmpPct { get; set; }
    public int? Yards { get; set; }
    public short? Touchdowns { get; set; }
    public short? Interceptions { get; set; }
    public decimal? Rating { get; set; }
    public decimal? Sacks { get; set; }
    public decimal? SackYards { get; set; }
    public int? AirYards { get; set; }
    public int? NetYards { get; set; }
    public decimal? PocketTime { get; set; }
    public short? Hurries { get; set; }
    public short? Blitzes { get; set; }
    public short? Knockdowns { get; set; }
    public short? BattedPasses { get; set; }
    public short? DroppedPasses { get; set; }
    public short? PoorThrows { get; set; }
    public short? ThrowAways { get; set; }
    public short? OnTargetThrows { get; set; }
    public short? RedzoneAttempts { get; set; }
    public short? Longest { get; set; }
    public short? LongestTd { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayerSeasonStats PlayerSeasonStats { get; set; } = null!;
}
