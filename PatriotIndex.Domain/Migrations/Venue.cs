using System;
using System.Collections.Generic;

namespace PatriotIndex.Domain.Migrations;

public partial class Venue
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

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

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
