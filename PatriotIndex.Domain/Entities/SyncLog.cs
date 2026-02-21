using System.Text.Json;

namespace PatriotIndex.Domain.Entities;

public class SyncLog
{
    public Guid Id { get; set; }
    public string EntityType { get; set; } = "";
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; } = "";
    public int RecordCount { get; set; }
    public string? ErrorMessage { get; set; }
    public JsonDocument? RawResponse { get; set; }
}
