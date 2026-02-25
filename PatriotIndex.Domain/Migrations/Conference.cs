using System;
using System.Collections.Generic;

namespace PatriotIndex.Domain.Migrations;

public partial class Conference
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Alias { get; set; } = null!;

    public virtual ICollection<Division> Divisions { get; set; } = new List<Division>();
}
