namespace PatriotIndex.Domain.Entities.Stats.PlayerGame;

public class PlayerGameKicking
{
    public Guid PlayerGameId { get; set; }
    public short? FgAttempts { get; set; }
    public short? FgMade { get; set; }
    public decimal? FgPct { get; set; }
    public short? FgLongest { get; set; }
    public short? FgAtt2029 { get; set; }
    public short? FgMade2029 { get; set; }
    public short? FgAtt3039 { get; set; }
    public short? FgMade3039 { get; set; }
    public short? FgAtt4049 { get; set; }
    public short? FgMade4049 { get; set; }
    public short? FgAtt50Plus { get; set; }
    public short? FgMade50Plus { get; set; }
    public short? XpAttempts { get; set; }
    public short? XpMade { get; set; }
    public decimal? XpPct { get; set; }
    public short? KoAttempts { get; set; }
    public int? KoYards { get; set; }
    public decimal? KoAvgYards { get; set; }
    public short? KoTouchbacks { get; set; }
    public short? KoOutOfBounds { get; set; }
    public short? KoOnsideAttempts { get; set; }
    public short? KoOnsideSuccesses { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayerGameStats PlayerGameStats { get; set; } = null!;
}
