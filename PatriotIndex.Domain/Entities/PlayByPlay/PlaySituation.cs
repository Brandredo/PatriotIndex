using PatriotIndex.Domain.Entities.Organization;

namespace PatriotIndex.Domain.Entities.PlayByPlay;

public class PlaySituation
{
    public Guid Id { get; set; }
    public Guid PlayId { get; set; }
    public string SituationType { get; set; } = null!;
    public short? Down { get; set; }
    public short? Yfd { get; set; }
    public string? Clock { get; set; }
    public Guid? LocationTeamId { get; set; }
    public short? LocationYardline { get; set; }
    public Guid? PossessionTeamId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public GamePlay Play { get; set; } = null!;
    public Team? LocationTeam { get; set; }
    public Team? PossessionTeam { get; set; }
}
