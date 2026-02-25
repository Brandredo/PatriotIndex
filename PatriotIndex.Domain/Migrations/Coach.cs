using System;
using System.Collections.Generic;

namespace PatriotIndex.Domain.Migrations;

public partial class Coach
{
    public Guid Id { get; set; }

    public Guid TeamId { get; set; }

    public string FullName { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Position { get; set; }

    public virtual Team Team { get; set; } = null!;
}
