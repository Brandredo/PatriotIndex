namespace PatriotIndex.Domain.Entities.Stats.PlayerGame;

public class PlayerGameDefense
{
    public Guid PlayerGameId { get; set; }
    public short? Tackles { get; set; }
    public short? Assists { get; set; }
    public short? Combined { get; set; }
    public decimal? Sacks { get; set; }
    public decimal? SackYards { get; set; }
    public short? Interceptions { get; set; }
    public short? IntYards { get; set; }
    public short? IntTouchdowns { get; set; }
    public short? PassesDefended { get; set; }
    public short? ForcedFumbles { get; set; }
    public short? FumbleRecoveries { get; set; }
    public short? FumbleRecYards { get; set; }
    public short? FumbleRecTds { get; set; }
    public short? QbHits { get; set; }
    public short? Blitzes { get; set; }
    public short? Hurries { get; set; }
    public short? Knockdowns { get; set; }
    public short? MissedTackles { get; set; }
    public short? DefTargets { get; set; }
    public short? DefCompletions { get; set; }
    public short? BattedPasses { get; set; }
    public short? Safeties { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public PlayerGameStats PlayerGameStats { get; set; } = null!;
}
