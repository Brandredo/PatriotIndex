namespace PatriotIndex.Domain.Entities;

public class Venue
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Zip { get; set; }
    public string? Address { get; set; }
    public int? Capacity { get; set; }
    public string? Surface { get; set; }
    public string? RoofType { get; set; }
    public string? SrId { get; set; }
    public string? Lat { get; set; }
    public string? Lng { get; set; }
}