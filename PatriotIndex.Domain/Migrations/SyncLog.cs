using System;
using System.Collections.Generic;

namespace PatriotIndex.Domain.Migrations;

public partial class SyncLog
{
    public Guid Id { get; set; }

    public string EntityType { get; set; } = null!;

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string Status { get; set; } = null!;

    public int RecordCount { get; set; }

    public string? ErrorMessage { get; set; }

    public string? RawResponse { get; set; }
}
