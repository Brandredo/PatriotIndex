using System;
using System.Collections.Generic;

namespace PatriotIndex.Domain.Migrations;

public partial class Division
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Alias { get; set; } = null!;

    public Guid ConferenceId { get; set; }

    public virtual Conference Conference { get; set; } = null!;

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
