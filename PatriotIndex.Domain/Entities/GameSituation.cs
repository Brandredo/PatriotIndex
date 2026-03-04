using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PatriotIndex.Domain.Entities;

[Owned]
public class GameSituation
{
    [MaxLength(10)]
    public string Clock { get; set; } = string.Empty;

    public int? Down { get; set; }
    public int  YardsToFirstDown { get; set; }
    public int  Yardline { get; set; }

    [MaxLength(10)]
    public string? YardlineTeam { get; set; }

    // FK → teams(id), configured in Play's OwnsOne block via HasOne<Team>()
    public Guid? PossessionTeamId { get; set; }
}
