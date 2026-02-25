using System;
using System.Collections.Generic;

namespace PatriotIndex.Domain.Migrations;

public partial class CoinToss
{
    public Guid Id { get; set; }

    public Guid PeriodId1 { get; set; }

    public Guid PeriodId { get; set; }

    public Guid WinnerId { get; set; }

    public string Decision { get; set; } = null!;

    public string Direction { get; set; } = null!;

    public virtual Period Period { get; set; } = null!;

    public virtual Period PeriodId1Navigation { get; set; } = null!;

    public virtual Team Winner { get; set; } = null!;
}
