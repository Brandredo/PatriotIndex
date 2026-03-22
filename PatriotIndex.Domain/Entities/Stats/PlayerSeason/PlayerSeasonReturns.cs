namespace PatriotIndex.Domain.Entities.Stats.PlayerSeason;

public class PlayerSeasonReturns
{
    public Guid PlayerSeasonId { get; set; }
    public short? KrReturns { get; set; }
    public int? KrYards { get; set; }
    public decimal? KrAvgYards { get; set; }
    public short? KrLongest { get; set; }
    public short? KrTouchdowns { get; set; }
    public short? KrFairCatches { get; set; }
    public short? KrLostFumbles { get; set; }
    public short? PrReturns { get; set; }
    public int? PrYards { get; set; }
    public decimal? PrAvgYards { get; set; }
    public short? PrLongest { get; set; }
    public short? PrTouchdowns { get; set; }
    public short? PrFairCatches { get; set; }
    public short? PrLostFumbles { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayerSeasonStats PlayerSeasonStats { get; set; } = null!;
}
