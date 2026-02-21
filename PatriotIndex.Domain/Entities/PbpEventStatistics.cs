namespace PatriotIndex.Domain.Entities;

public class PbpEventStatistics
{
    public Guid EventId { get; set; }
    public string StatType { get; set; } = "";
    public string? Category { get; set; }
}