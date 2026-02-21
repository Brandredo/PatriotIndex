namespace PatriotIndex.Domain.Entities;

public class CoinToss
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid PeriodId { get; set; }
    public Guid WinnerId { get; set; }
    public string Decision { get; set; } = "";
    public string Direction { get; set; } = "";
}