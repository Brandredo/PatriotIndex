namespace PatriotIndex.Domain.Entities;

public class Season
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public SeasonStatus Status { get; set; }
    public string Code { get; set; }
}