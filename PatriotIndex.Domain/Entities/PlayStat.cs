using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PatriotIndex.Domain.Entities;

[Owned]
public class PlayStatPlayerRef
{
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5)]
    public string? Position { get; set; }

    [MaxLength(3)]
    public string? Jersey { get; set; }

    [MaxLength(100)]
    public string? SrId { get; set; }
}

[Owned]
public class PlayStatTeamRef
{
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5)]
    public string Alias { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? SrId { get; set; }
}

[Owned]
public class PlayStat
{
    [Required]
    [MaxLength(50)]
    public string StatType { get; set; } = string.Empty;

    public PlayStatPlayerRef Player { get; set; } = new();
    public PlayStatTeamRef   Team   { get; set; } = new();

    // Rush
    public int?  Attempt            { get; set; }
    public int?  Yards              { get; set; }
    public int?  Touchdowns         { get; set; }
    public int?  FirstDown          { get; set; }
    public int?  BrokenTackles      { get; set; }
    public int?  TackleForLoss      { get; set; }
    public int?  TackleForLossYards { get; set; }
    public int?  Scramble           { get; set; }
    public int?  KneelDown          { get; set; }
    public int?  YardsAfterContact  { get; set; }

    // Pass
    public int?    Complete         { get; set; }
    public int?    AttemptYards     { get; set; }
    public int?    AirYards         { get; set; }
    public int?    Sack             { get; set; }
    public int?    SackYards        { get; set; }
    public double? PocketTime       { get; set; }
    public int?    Interceptions    { get; set; }
    public int?    Defended         { get; set; }
    public int?    Batted           { get; set; }
    public int?    Hurry            { get; set; }
    public int?    Knockdown        { get; set; }
    public int?    Blitz            { get; set; }
    public int?    PoorThrow        { get; set; }
    public int?    ThrowAway        { get; set; }
    public int?    Spike            { get; set; }

    [MaxLength(50)]
    public string? IncompletionType { get; set; }

    // Receive
    public int? Target          { get; set; }
    public int? Reception       { get; set; }
    public int? YardsAfterCatch { get; set; }
    public int? Dropped         { get; set; }

    // Defense
    public int? Tackle        { get; set; }
    public int? AstTackle     { get; set; }
    public int? MissedTackles { get; set; }
    public int? DefTd         { get; set; }
    public int? SafetyTackle  { get; set; }
    public int? Interception  { get; set; }
    public int? ForcedFumble  { get; set; }

    // Kick / Punt / Return
    public int? NetYards      { get; set; }
    public int? Touchback     { get; set; }
    public int? FairCatch     { get; set; }
    public int? ReturnYards   { get; set; }
    public int? OnsideAttempt { get; set; }
    public int? OnsideSuccess { get; set; }
    public int? PuntYards     { get; set; }
    public int? HangTime      { get; set; }

    // Fumble
    public int? Fumble    { get; set; }
    public int? FumbleLost { get; set; }
    public int? OwnRec    { get; set; }
    public int? OppRec    { get; set; }

    // Field Goal / Extra Point
    public bool? Made     { get; set; }
    public int?  Distance { get; set; }

    // Penalty
    public int?  PenaltyYards { get; set; }
    public bool? Declined     { get; set; }

    [MaxLength(100)]
    public string? PenaltyType { get; set; }

    // Conversion / Block
    public bool? Success { get; set; }

    [MaxLength(50)]
    public string? ConversionType { get; set; }

    // Flags
    public bool? Nullified { get; set; }
    public int?  Inside20  { get; set; }
    public int?  GoalToGo  { get; set; }
    public int?  Safety    { get; set; }
}
