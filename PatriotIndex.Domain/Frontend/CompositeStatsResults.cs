namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Complete calculated statistics for a player, combining all applicable stat blocks.
/// Each property is null when the underlying <c>SeasonXxxStats</c> was not populated.
/// </summary>
public sealed record PlayerStatsResult
{
    // ── By Position: Quarterback ──────────────────────────────────────────────

    /// <inheritdoc cref="PassingStatsResult"/>
    public PassingStatsResult? Passing { get; init; }

    /// <inheritdoc cref="RushingStatsResult"/>
    public RushingStatsResult? Rushing { get; init; }

    // ── By Position: Running Back / Wide Receiver / Tight End ────────────────

    /// <inheritdoc cref="ReceivingStatsResult"/>
    public ReceivingStatsResult? Receiving { get; init; }

    // ── By Position: Defense ──────────────────────────────────────────────────

    /// <inheritdoc cref="DefenseStatsResult"/>
    //public Defense DefenseStats { get; init; } = new();

    // ── By Position: Kicker ───────────────────────────────────────────────────

    /// <inheritdoc cref="FieldGoalStatsResult"/>
    public FieldGoalStatsResult? FieldGoals { get; init; }

    /// <inheritdoc cref="PlayerExtraPointStatsResult"/>
    public PlayerExtraPointStatsResult? ExtraPoints { get; init; }

    /// <inheritdoc cref="KickoffStatsResult"/>
    public KickoffStatsResult? Kickoffs { get; init; }

    // ── By Position: Punter ───────────────────────────────────────────────────

    /// <inheritdoc cref="PuntingStatsResult"/>
    public PuntingStatsResult? Punts { get; init; }

    // ── By Position: Return Specialist ───────────────────────────────────────

    /// <inheritdoc cref="KickReturnStatsResult"/>
    public KickReturnStatsResult? KickReturns { get; init; }

    /// <inheritdoc cref="PuntReturnStatsResult"/>
    public PuntReturnStatsResult? PuntReturns { get; init; }

    /// <inheritdoc cref="IntReturnStatsResult"/>
    public IntReturnStatsResult? IntReturns { get; init; }

    /// <inheritdoc cref="MiscReturnStatsResult"/>
    public MiscReturnStatsResult? MiscReturns { get; init; }

    // ── Universal ─────────────────────────────────────────────────────────────

    /// <inheritdoc cref="FumbleStatsResult"/>
    public FumbleStatsResult? Fumbles { get; init; }

    /// <inheritdoc cref="PenaltyStatsResult"/>
    public PenaltyStatsResult? Penalties { get; init; }
    
    public DefenseStatsResult? Defense { get; init; }

    // ── Cross-Block Derived ───────────────────────────────────────────────────

    /// <summary>
    /// Rushing.Yards + Receiving.Yards. Available when both stat blocks are present.
    /// The primary workhorse volume stat for RBs.
    /// </summary>
    public int? ScrimmageYards { get; init; }

    /// <summary>Rushing.Attempts + Receiving.Receptions. Combined usage metric for RBs.</summary>
    public int? TotalTouches { get; init; }

    /// <summary>Rushing.Touchdowns + Receiving.Touchdowns. Full scoring picture for skill players.</summary>
    public int? TotalOffensiveTouchdowns { get; init; }

    /// <summary>
    /// (Passing.Interceptions + Fumbles.LostFumbles) / (Passing.Attempts + Rushing.Attempts) × 100.
    /// Comprehensive turnover risk per play for quarterbacks.
    /// </summary>
    public double? TotalTurnoverRate { get; init; }

    /// <summary>Fumbles.LostFumbles / (Rushing.Attempts + Receiving.Receptions) × 100. Ball-security for skill players.</summary>
    public double? FumbleRatePerTouch { get; init; }

    // ── Nested type alias to avoid circular reference ─────────────────────────

    // public sealed record Defense
    // {
    //     public DefenseStatsResult? Stats { get; init; }
    // }
}

/// <summary>
/// Complete calculated statistics for a team, combining all stat blocks in <c>TeamStatBlock</c>.
/// </summary>
public sealed record TeamStatsResult
{
    // ── Offense ───────────────────────────────────────────────────────────────

    /// <inheritdoc cref="PassingStatsResult"/>
    public PassingStatsResult? Passing { get; init; }

    /// <inheritdoc cref="RushingStatsResult"/>
    public RushingStatsResult? Rushing { get; init; }

    /// <inheritdoc cref="ReceivingStatsResult"/>
    public ReceivingStatsResult? Receiving { get; init; }

    // ── Defense ───────────────────────────────────────────────────────────────

    /// <inheritdoc cref="DefenseStatsResult"/>
    public DefenseStatsResult? Defense { get; init; }

    /// <inheritdoc cref="TeamInterceptionStatsResult"/>
    public TeamInterceptionStatsResult? Interceptions { get; init; }

    // ── Special Teams ─────────────────────────────────────────────────────────

    /// <inheritdoc cref="FieldGoalStatsResult"/>
    public FieldGoalStatsResult? FieldGoals { get; init; }

    /// <inheritdoc cref="KickoffStatsResult"/>
    public KickoffStatsResult? Kickoffs { get; init; }

    /// <inheritdoc cref="PuntingStatsResult"/>
    public PuntingStatsResult? Punts { get; init; }

    /// <inheritdoc cref="KickReturnStatsResult"/>
    public KickReturnStatsResult? KickReturns { get; init; }

    /// <inheritdoc cref="PuntReturnStatsResult"/>
    public PuntReturnStatsResult? PuntReturns { get; init; }

    /// <inheritdoc cref="IntReturnStatsResult"/>
    public IntReturnStatsResult? IntReturns { get; init; }

    /// <inheritdoc cref="MiscReturnStatsResult"/>
    public MiscReturnStatsResult? MiscReturns { get; init; }

    /// <inheritdoc cref="TeamExtraPointStatsResult"/>
    public TeamExtraPointStatsResult? ExtraPoints { get; init; }

    // ── Turnovers / Penalties ─────────────────────────────────────────────────

    /// <inheritdoc cref="FumbleStatsResult"/>
    public FumbleStatsResult? Fumbles { get; init; }

    /// <inheritdoc cref="PenaltyStatsResult"/>
    public PenaltyStatsResult? Penalties { get; init; }

    // ── Team-Only ─────────────────────────────────────────────────────────────

    /// <inheritdoc cref="EfficiencyStatsResult"/>
    public EfficiencyStatsResult? Efficiency { get; init; }

    /// <inheritdoc cref="FirstDownStatsResult"/>
    public FirstDownStatsResult? FirstDowns { get; init; }

    /// <inheritdoc cref="TouchdownDistributionResult"/>
    public TouchdownDistributionResult? TouchdownDistribution { get; init; }

    /// <inheritdoc cref="TurnoverDifferentialResult"/>
    public TurnoverDifferentialResult? TurnoverDifferential { get; init; }

    // ── Team Red Zone (Cross-Block) ───────────────────────────────────────────

    /// <summary>
    /// (Passing.Touchdowns + Rushing.Touchdowns) / Efficiency.Redzone.Attempts × 100.
    /// TDs specifically (not just any score) per red zone trip.
    /// Differentiates TD efficiency from settling for field goals.
    /// </summary>
    public double? RedzoneTdRate { get; init; }
}
