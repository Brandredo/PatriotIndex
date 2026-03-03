
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PatriotIndex.Domain.Entities;
// ─────────────────────────────────────────────────────────────────────────────
//  Shared reference types
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Lightweight player reference embedded inside a play statistic.</summary>
public sealed record PlayStatPlayer
{
    [JsonPropertyName("id")]       public string  Id       { get; init; } = string.Empty;
    [JsonPropertyName("name")]     public string  Name     { get; init; } = string.Empty;
    [JsonPropertyName("jersey")]   public string? Jersey   { get; init; }
    [JsonPropertyName("position")] public string? Position { get; init; }
    [JsonPropertyName("sr_id")]    public string? SrId     { get; init; }
}

/// <summary>Lightweight team reference embedded inside a play statistic.</summary>
public sealed record PlayStatTeam
{
    [JsonPropertyName("id")]     public string  Id     { get; init; } = string.Empty;
    [JsonPropertyName("name")]   public string  Name   { get; init; } = string.Empty;
    [JsonPropertyName("market")] public string? Market { get; init; }
    [JsonPropertyName("alias")]  public string? Alias  { get; init; }
    [JsonPropertyName("sr_id")]  public string? SrId   { get; init; }
}

// ─────────────────────────────────────────────────────────────────────────────
//  stat_type discriminator enum
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>All known values of the <c>stat_type</c> discriminator field.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PlayStatType
{
    Unknown,
    Rush,
    Pass,
    Receive,
    Defense,
    Fumble,
    Penalty,
    Kick,             // kickoff
    Punt,
    Return,           // kick/punt/misc return
    IntReturn,        // interception return
    MiscReturn,
    FieldGoal,
    ExtraPoint,
    FirstDown,
    Conversion,       // 2-point conversion attempt (pass/rush/receive)
    DefenseConversion,// defensive 2-point conversion attempt
    DownConversion,   // 3rd/4th down conversion attempt
    Block,            // blocked kick/punt
}

// ─────────────────────────────────────────────────────────────────────────────
//  Abstract base
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Base class for every object in the <c>events[].statistics[]</c> array of the
/// NFL Game Play-by-Play feed.  Each concrete subclass maps to one <c>stat_type</c>.
/// <para>
/// Deserialize an array with:
/// <code>
///   var stats = JsonSerializer.Deserialize&lt;List&lt;PlayStatistic&gt;&gt;(
///       json, PlayStatistic.JsonOptions);
/// </code>
/// </para>
/// </summary>
[JsonConverter(typeof(PlayStatisticConverter))]
public abstract class PlayStatistic
{
    
    public long Id { get; init; }
    public Guid PlayId { get; init; }
    public DriveEvent PlayEvent { get; init; }
    
    
    /// <summary>Identifies which statistical category this object represents.</summary>
    [JsonPropertyName("stat_type")]
    public abstract PlayStatType StatType { get; }

    /// <summary>
    /// The player credited with this statistic.
    /// Absent on team-level stats (e.g. touchback returns, first-down summaries).
    /// </summary>
    public PlayStatPlayer? Player { get; init; }

    /// <summary>The team credited with this statistic.</summary>
    public PlayStatTeam? Team { get; init; }

    /// <summary>
    /// Pre-configured <see cref="JsonSerializerOptions"/> that register the
    /// polymorphic converter for <see cref="PlayStatistic"/>.
    /// </summary>
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive     = true,
        DefaultIgnoreCondition          = JsonIgnoreCondition.WhenWritingNull,
        Converters                      = { new PlayStatisticConverter() },
    };
}

// ─────────────────────────────────────────────────────────────────────────────
//  Concrete stat classes
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Rushing attempt statistics (run plays, scrambles, kneel-downs).
/// <c>stat_type = "rush"</c>
/// </summary>
public sealed class RushPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Rush;

    [JsonPropertyName("attempt")]              public int     Attempt              { get; init; }
    [JsonPropertyName("yards")]                public int     Yards                { get; init; }
    [JsonPropertyName("touchdowns")]           public int     Touchdowns           { get; init; }
    [JsonPropertyName("first_down")]           public int     FirstDown            { get; init; }
    [JsonPropertyName("tlost")]                public int     TackleForLoss        { get; init; }
    [JsonPropertyName("tlost_yards")]          public int     TackleForLossYards   { get; init; }
    [JsonPropertyName("broken_tackles")]       public int     BrokenTackles        { get; init; }
    [JsonPropertyName("scramble")]             public int     Scramble             { get; init; }
    [JsonPropertyName("kneel_down")]           public int     KneelDown            { get; init; }
    [JsonPropertyName("yards_after_contact")]  public int     YardsAfterContact    { get; init; }
    /// <summary>1 if the rush occurred inside the opponent's 20-yard line.</summary>
    [JsonPropertyName("inside_20")]            public int?    Inside20             { get; init; }
    /// <summary>1 if the rush occurred on a goal-to-go down.</summary>
    [JsonPropertyName("goal_to_go")]           public int?    GoalToGo             { get; init; }
    /// <summary>1 if the rusher lateralled the ball on this play.</summary>
    [JsonPropertyName("lateral")]              public int?    Lateral              { get; init; }
    /// <summary>1 if the play was nullified by a penalty.</summary>
    [JsonPropertyName("nullified")]            public int?    Nullified            { get; init; }
    /// <summary>1 if the rush resulted in a safety.</summary>
    [JsonPropertyName("safety")]               public int?    Safety               { get; init; }
}

/// <summary>
/// Forward-pass attempt statistics (credited to the passer/QB).
/// <c>stat_type = "pass"</c>
/// </summary>
public sealed class PassPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Pass;

    [JsonPropertyName("attempt")]              public int     Attempt              { get; init; }
    /// <summary>Air distance (depth of target) in yards.</summary>
    [JsonPropertyName("attempt_yards")]        public int?    AttemptYards         { get; init; }
    [JsonPropertyName("complete")]             public int     Complete             { get; init; }
    [JsonPropertyName("yards")]                public int     Yards                { get; init; }
    [JsonPropertyName("touchdowns")]           public int     Touchdowns           { get; init; }
    [JsonPropertyName("interceptions")]        public int     Interceptions        { get; init; }
    [JsonPropertyName("int_touchdowns")]       public int?    IntTouchdowns        { get; init; }
    [JsonPropertyName("first_down")]           public int     FirstDown            { get; init; }
    [JsonPropertyName("air_yards")]            public int     AirYards             { get; init; }
    [JsonPropertyName("sack")]                 public int     Sack                 { get; init; }
    [JsonPropertyName("sack_yards")]           public int     SackYards            { get; init; }
    [JsonPropertyName("spike")]                public int     Spike                { get; init; }
    [JsonPropertyName("throw_away")]           public int     ThrowAway            { get; init; }
    [JsonPropertyName("poor_throw")]           public int     PoorThrow            { get; init; }
    [JsonPropertyName("defended")]             public int     Defended             { get; init; }
    [JsonPropertyName("dropped")]              public int     Dropped              { get; init; }
    [JsonPropertyName("batted")]               public int     Batted               { get; init; }
    [JsonPropertyName("on_target")]            public int?    OnTarget             { get; init; }
    [JsonPropertyName("blitz")]                public int     Blitz                { get; init; }
    [JsonPropertyName("hurry")]                public int     Hurry                { get; init; }
    [JsonPropertyName("knockdown")]            public int     Knockdown            { get; init; }
    [JsonPropertyName("pocket_time")]          public double? PocketTime           { get; init; }
    [JsonPropertyName("gross_yards")]          public int?    GrossYards           { get; init; }
    [JsonPropertyName("net_yards")]            public int?    NetYards             { get; init; }
    [JsonPropertyName("inside_20")]            public int?    Inside20             { get; init; }
    [JsonPropertyName("goal_to_go")]           public int?    GoalToGo             { get; init; }
    /// <summary>
    /// Reason the pass was incomplete (e.g. "overthrown", "dropped", "defensed", "batted").
    /// </summary>
    [JsonPropertyName("incompletion_type")]    public string? IncompletionType     { get; init; }
    [JsonPropertyName("nullified")]            public int?    Nullified            { get; init; }
    [JsonPropertyName("safety")]               public int?    Safety               { get; init; }
}

/// <summary>
/// Pass reception statistics (credited to the receiver).
/// <c>stat_type = "receive"</c>
/// </summary>
public sealed class ReceivePlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Receive;

    [JsonPropertyName("target")]               public int  Target              { get; init; }
    [JsonPropertyName("reception")]            public int  Reception           { get; init; }
    [JsonPropertyName("yards")]                public int  Yards               { get; init; }
    [JsonPropertyName("touchdowns")]           public int  Touchdowns          { get; init; }
    [JsonPropertyName("first_down")]           public int  FirstDown           { get; init; }
    [JsonPropertyName("air_yards")]            public int  AirYards            { get; init; }
    [JsonPropertyName("yards_after_catch")]    public int  YardsAfterCatch     { get; init; }
    [JsonPropertyName("yards_after_contact")]  public int  YardsAfterContact   { get; init; }
    [JsonPropertyName("broken_tackles")]       public int  BrokenTackles       { get; init; }
    [JsonPropertyName("dropped")]              public int  Dropped             { get; init; }
    [JsonPropertyName("catchable")]            public int  Catchable           { get; init; }
    [JsonPropertyName("redzone_target")]       public int? RedzoneTarget       { get; init; }
    [JsonPropertyName("inside_20")]            public int? Inside20            { get; init; }
    [JsonPropertyName("goal_to_go")]           public int? GoalToGo            { get; init; }
    [JsonPropertyName("nullified")]            public int? Nullified           { get; init; }
    [JsonPropertyName("safety")]               public int? Safety              { get; init; }
}

/// <summary>
/// Defensive play statistics credited to an individual defender.
/// <c>stat_type = "defense"</c>
/// </summary>
public sealed class DefensePlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Defense;

    /// <summary>
    /// Sub-category of this defensive credit (e.g. "primary", "assist").
    /// Distinguishes primary tacklers from assisted tacklers.
    /// </summary>
    [JsonPropertyName("category")]             public string? Category              { get; init; }
    /// <summary>1 if this is the primary (unassisted) defender on the play.</summary>
    [JsonPropertyName("primary")]              public int?    Primary               { get; init; }

    // Tackles
    [JsonPropertyName("tackle")]               public int     Tackle                { get; init; }
    [JsonPropertyName("assist")]               public int     Assist                { get; init; }
    /// <summary>Assisted tackle credited to a second defender.</summary>
    [JsonPropertyName("assisted_tackle")]      public int?    AssistedTackle        { get; init; }
    [JsonPropertyName("tloss")]                public double  TackleForLoss         { get; init; }
    [JsonPropertyName("tloss_yards")]          public double  TackleForLossYards    { get; init; }
    /// <summary>Assisted tackle-for-loss credited to a second defender.</summary>
    [JsonPropertyName("assisted_tloss")]       public double? AssistedTackleForLoss { get; init; }
    [JsonPropertyName("missed_tackle")]        public int     MissedTackle          { get; init; }

    // Pass rush / sacks
    [JsonPropertyName("sack")]                 public double  Sack                  { get; init; }
    [JsonPropertyName("sack_yards")]           public double  SackYards             { get; init; }
    /// <summary>Sack credited jointly to more than one defender (e.g. 0.5 sack each).</summary>
    [JsonPropertyName("assisted_sack")]        public double? AssistedSack          { get; init; }
    [JsonPropertyName("qb_hit")]               public int     QbHit                 { get; init; }
    [JsonPropertyName("hurry")]                public int     Hurry                 { get; init; }
    [JsonPropertyName("knockdown")]            public int     Knockdown             { get; init; }
    [JsonPropertyName("blitz")]                public int     Blitz                 { get; init; }

    // Coverage / turnovers
    [JsonPropertyName("interception")]         public int     Interception          { get; init; }
    [JsonPropertyName("int_yards")]            public int?    InterceptionYards     { get; init; }
    [JsonPropertyName("int_touchdowns")]       public int?    InterceptionTouchdowns{ get; init; }
    [JsonPropertyName("pass_defended")]        public int     PassDefended          { get; init; }
    [JsonPropertyName("batted_pass")]          public int     BattedPass            { get; init; }
    [JsonPropertyName("def_target")]           public int?    DefTarget             { get; init; }
    [JsonPropertyName("def_comp")]             public int?    DefComp               { get; init; }

    // Forced turnovers / recoveries
    [JsonPropertyName("forced_fumble")]        public int     ForcedFumble          { get; init; }
    [JsonPropertyName("fumble_recovery")]      public int     FumbleRecovery        { get; init; }
    [JsonPropertyName("safety")]               public int     Safety                { get; init; }

    // Special-teams defense
    [JsonPropertyName("sp_tackle")]            public int?    SpTackle              { get; init; }
    [JsonPropertyName("sp_assist")]            public int?    SpAssist              { get; init; }
    [JsonPropertyName("sp_forced_fumble")]     public int?    SpForcedFumble        { get; init; }
    [JsonPropertyName("sp_fumble_recovery")]   public int?    SpFumbleRecovery      { get; init; }
    [JsonPropertyName("sp_block")]             public int?    SpBlock               { get; init; }

    // Miscellaneous (e.g. QB run)
    [JsonPropertyName("misc_tackle")]          public int?    MiscTackle            { get; init; }
    [JsonPropertyName("misc_assist")]          public int?    MiscAssist            { get; init; }
    [JsonPropertyName("misc_forced_fumble")]   public int?    MiscForcedFumble      { get; init; }
    [JsonPropertyName("misc_fumble_recovery")] public int?    MiscFumbleRecovery    { get; init; }

    [JsonPropertyName("nullified")]            public int?    Nullified             { get; init; }
}

/// <summary>
/// Fumble statistics (fumble lost/recovered, forced fumble credit).
/// <c>stat_type = "fumble"</c>
/// </summary>
public sealed class FumblePlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Fumble;

    /// <summary>
    /// Context in which the fumble occurred (e.g. "rush", "pass", "receive", "kick_return").
    /// </summary>
    [JsonPropertyName("play_category")]        public string? PlayCategory      { get; init; }

    [JsonPropertyName("fumble")]               public int     Fumbles           { get; init; }
    [JsonPropertyName("lost_fumble")]          public int     LostFumbles       { get; init; }
    [JsonPropertyName("own_rec")]              public int     OwnRecoveries     { get; init; }
    [JsonPropertyName("own_rec_yards")]        public int     OwnRecoveryYards  { get; init; }
    [JsonPropertyName("opp_rec")]              public int     OppRecoveries     { get; init; }
    [JsonPropertyName("opp_rec_yards")]        public int     OppRecoveryYards  { get; init; }
    [JsonPropertyName("out_of_bounds")]        public int     OutOfBounds       { get; init; }
    [JsonPropertyName("forced_fumble")]        public int     ForcedFumbles     { get; init; }
    [JsonPropertyName("own_rec_tds")]          public int     OwnRecoveryTds    { get; init; }
    [JsonPropertyName("opp_rec_tds")]          public int     OppRecoveryTds    { get; init; }
    [JsonPropertyName("ez_rec_tds")]           public int     EndZoneRecoveryTds{ get; init; }
    [JsonPropertyName("nullified")]            public int?    Nullified         { get; init; }
}

/// <summary>
/// Penalty statistics attributed to an individual player or team.
/// <c>stat_type = "penalty"</c>
/// </summary>
public sealed class PenaltyPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Penalty;

    [JsonPropertyName("penalties")]            public int     Penalties     { get; init; }
    [JsonPropertyName("yards")]                public int     Yards         { get; init; }
    [JsonPropertyName("first_down")]           public int     FirstDown     { get; init; }
    [JsonPropertyName("no_play")]              public int?    NoPlay        { get; init; }
    [JsonPropertyName("penalty_type")]         public string? PenaltyType   { get; init; }
    [JsonPropertyName("declined")]             public int?    Declined      { get; init; }
    [JsonPropertyName("offsetting")]           public int?    Offsetting    { get; init; }
}

/// <summary>
/// Kickoff statistics credited to the kicker.
/// <c>stat_type = "kick"</c>
/// </summary>
public sealed class KickPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Kick;

    [JsonPropertyName("attempt")]              public int  Attempt              { get; init; }
    [JsonPropertyName("yards")]                public int  Yards                { get; init; }
    [JsonPropertyName("net_yards")]            public int  NetYards             { get; init; }
    [JsonPropertyName("touchback")]            public int  Touchback            { get; init; }
    [JsonPropertyName("onside_attempt")]       public int  OnsideAttempt        { get; init; }
    [JsonPropertyName("onside_success")]       public int  OnsideSuccess        { get; init; }
    [JsonPropertyName("squib_kick")]           public int  SquibKick            { get; init; }
    [JsonPropertyName("endzone")]              public int? Endzone              { get; init; }
    [JsonPropertyName("inside_20")]            public int? Inside20             { get; init; }
    [JsonPropertyName("returned")]             public int? Returned             { get; init; }
    [JsonPropertyName("out_of_bounds")]        public int? OutOfBounds          { get; init; }
    /// <summary>1 if the kicking team recovered their own kick.</summary>
    [JsonPropertyName("own_recovery")]         public int? OwnRecovery          { get; init; }
    /// <summary>1 if the kicking team's own recovery resulted in a touchdown.</summary>
    [JsonPropertyName("own_recovery_td")]      public int? OwnRecoveryTouchdown { get; init; }
    [JsonPropertyName("nullified")]            public int? Nullified            { get; init; }
}

/// <summary>
/// Punt statistics credited to the punter.
/// <c>stat_type = "punt"</c>
/// </summary>
public sealed class PuntPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Punt;

    [JsonPropertyName("attempt")]              public int     Attempt     { get; init; }
    [JsonPropertyName("yards")]                public int     Yards       { get; init; }
    [JsonPropertyName("net_yards")]            public int     NetYards    { get; init; }
    [JsonPropertyName("touchback")]            public int     Touchback   { get; init; }
    [JsonPropertyName("inside_20")]            public int     Inside20    { get; init; }
    [JsonPropertyName("blocked")]              public int     Blocked     { get; init; }
    [JsonPropertyName("hang_time")]            public double? HangTime    { get; init; }
    [JsonPropertyName("return_yards")]         public int?    ReturnYards { get; init; }
    [JsonPropertyName("fair_catch")]           public int?    FairCatch   { get; init; }
    /// <summary>1 if the punt was downed by the kicking team.</summary>
    [JsonPropertyName("downed")]               public int?    Downed      { get; init; }
    /// <summary>1 if the punt landed in or through the end zone (non-touchback).</summary>
    [JsonPropertyName("end_zone")]             public int?    EndZone     { get; init; }
    [JsonPropertyName("nullified")]            public int?    Nullified   { get; init; }
}

/// <summary>
/// Return statistics for kick, punt, and miscellaneous returns.
/// The <see cref="Category"/> field distinguishes the type of return.
/// <c>stat_type = "return"</c>
/// </summary>
public sealed class ReturnPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Return;

    /// <summary>Type of return: "kick_return", "punt_return", "misc_return".</summary>
    [JsonPropertyName("category")]             public string? Category      { get; init; }
    /// <summary>
    /// Original play type that generated the return opportunity
    /// (e.g. "kickoff", "punt", "blocked_fg").
    /// </summary>
    [JsonPropertyName("play_category")]        public string? PlayCategory  { get; init; }

    [JsonPropertyName("yards")]                public int     Yards         { get; init; }
    [JsonPropertyName("touchdowns")]           public int     Touchdowns    { get; init; }
    [JsonPropertyName("touchback")]            public int     Touchback     { get; init; }
    [JsonPropertyName("fair_catch")]           public int?    FairCatch     { get; init; }
    [JsonPropertyName("first_down")]           public int?    FirstDown     { get; init; }
    [JsonPropertyName("longest")]              public int?    Longest       { get; init; }
    /// <summary>1 if the returner was downed in-bounds by the kicking/punting team.</summary>
    [JsonPropertyName("downed")]               public int?    Downed        { get; init; }
    /// <summary>1 if the returner went out of bounds.</summary>
    [JsonPropertyName("out_of_bounds")]        public int?    OutOfBounds   { get; init; }
    /// <summary>1 if the returner lateralled the ball.</summary>
    [JsonPropertyName("lateral")]              public int?    Lateral       { get; init; }
    [JsonPropertyName("nullified")]            public int?    Nullified     { get; init; }
}

/// <summary>
/// Interception-return statistics credited to the intercepting defender.
/// <c>stat_type = "int_return"</c>
/// </summary>
public sealed class IntReturnPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.IntReturn;

    [JsonPropertyName("yards")]                public int  Yards      { get; init; }
    [JsonPropertyName("touchdowns")]           public int  Touchdowns { get; init; }
    [JsonPropertyName("longest")]              public int? Longest    { get; init; }
    [JsonPropertyName("returns")]              public int? Returns    { get; init; }
}

/// <summary>
/// Miscellaneous return statistics (blocked FG/punt returns, fumble returns, etc.).
/// <c>stat_type = "misc_return"</c>
/// </summary>
public sealed class MiscReturnPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.MiscReturn;

    [JsonPropertyName("yards")]                    public int  Yards                 { get; init; }
    [JsonPropertyName("touchdowns")]               public int  Touchdowns            { get; init; }
    [JsonPropertyName("blk_fg_touchdowns")]        public int? BlkFgTouchdowns       { get; init; }
    [JsonPropertyName("blk_punt_touchdowns")]      public int? BlkPuntTouchdowns     { get; init; }
    [JsonPropertyName("fg_return_touchdowns")]     public int? FgReturnTouchdowns    { get; init; }
    [JsonPropertyName("ez_rec_touchdowns")]        public int? EzRecTouchdowns       { get; init; }
    [JsonPropertyName("returns")]                  public int? Returns               { get; init; }
    [JsonPropertyName("longest_touchdown")]        public int? LongestTouchdown      { get; init; }
}

/// <summary>
/// Field-goal attempt statistics credited to the kicker.
/// <c>stat_type = "field_goal"</c>
/// </summary>
public sealed class FieldGoalPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.FieldGoal;

    [JsonPropertyName("attempt")]              public int  Attempt       { get; init; }
    /// <summary>Distance of the attempt in yards.</summary>
    [JsonPropertyName("attempt_yards")]        public int? AttemptYards  { get; init; }
    [JsonPropertyName("made")]                 public int  Made          { get; init; }
    [JsonPropertyName("missed")]               public int? Missed        { get; init; }
    [JsonPropertyName("blocked")]              public int  Blocked       { get; init; }
    /// <summary>Yards credited on a successful kick (equals attempt_yards when made).</summary>
    [JsonPropertyName("yards")]                public int  Yards         { get; init; }
    [JsonPropertyName("inside_20")]            public int? Inside20      { get; init; }
    /// <summary>1 if the missed/blocked FG was returned by the defense.</summary>
    [JsonPropertyName("returned")]             public int? Returned      { get; init; }
    [JsonPropertyName("nullified")]            public int? Nullified     { get; init; }
}

/// <summary>
/// Extra-point (PAT) kick statistics.
/// <c>stat_type = "extra_point"</c>
/// </summary>
public sealed class ExtraPointPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.ExtraPoint;

    [JsonPropertyName("attempt")]              public int  Attempt  { get; init; }
    [JsonPropertyName("made")]                 public int  Made     { get; init; }
    [JsonPropertyName("missed")]               public int? Missed   { get; init; }
    [JsonPropertyName("blocked")]              public int? Blocked  { get; init; }
    /// <summary>1 if the snap was fumbled or the kick was otherwise aborted before the attempt.</summary>
    [JsonPropertyName("aborted")]              public int? Aborted  { get; init; }
    /// <summary>1 if the defense returned a blocked or missed PAT for 2 points.</summary>
    [JsonPropertyName("returned")]             public int? Returned { get; init; }
    /// <summary>1 if the defense scored a safety on the PAT attempt.</summary>
    [JsonPropertyName("safety")]               public int? Safety   { get; init; }
}

/// <summary>
/// Two-point conversion attempt statistics (pass, rush, or receive).
/// <c>stat_type = "conversion"</c>
/// </summary>
public sealed class ConversionPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Conversion;

    /// <summary>Type of conversion attempt: "pass", "rush", "receive".</summary>
    [JsonPropertyName("category")]             public string? Category { get; init; }
    [JsonPropertyName("attempt")]              public int     Attempt  { get; init; }
    [JsonPropertyName("complete")]             public int     Complete { get; init; }
    /// <summary>1 if the defense scored a safety on the conversion attempt.</summary>
    [JsonPropertyName("safety")]               public int?    Safety   { get; init; }
}

/// <summary>
/// Defensive two-point conversion statistics
/// (credited when the defense stops or scores on a conversion attempt).
/// <c>stat_type = "defense_conversion"</c>
/// </summary>
public sealed class DefenseConversionPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.DefenseConversion;

    /// <summary>Context of the conversion play being defended (e.g. "pass", "rush").</summary>
    [JsonPropertyName("category")]             public string? Category { get; init; }
    [JsonPropertyName("attempt")]              public int     Attempt  { get; init; }
    [JsonPropertyName("complete")]             public int     Complete { get; init; }
}

/// <summary>
/// Third-down or fourth-down conversion statistics (team-level).
/// <c>stat_type = "down_conversion"</c>
/// </summary>
public sealed class DownConversionPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.DownConversion;

    /// <summary>The down on which the conversion was attempted (3 or 4).</summary>
    [JsonPropertyName("down")]                 public int Down     { get; init; }
    [JsonPropertyName("attempt")]              public int Attempt  { get; init; }
    [JsonPropertyName("complete")]             public int Complete { get; init; }
}

/// <summary>
/// Blocked-kick statistics (blocked punts, blocked field goals, blocked PATs).
/// <c>stat_type = "block"</c>
/// </summary>
public sealed class BlockPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Block;

    /// <summary>Type of play that was blocked: "punt", "field_goal", "extra_point".</summary>
    [JsonPropertyName("category")]             public string? Category { get; init; }
    /// <summary>Number of blocks credited to this player on this play (usually 1).</summary>
    [JsonPropertyName("blocks")]               public int     Blocks   { get; init; }
}

/// <summary>
/// First-down statistics broken out by how the first down was gained (team-level).
/// <c>stat_type = "first_down"</c>
/// </summary>
public sealed class FirstDownPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.FirstDown;

    /// <summary>
    /// Method by which the first down was gained: "rush", "pass", or "penalty".
    /// Present when a single-category credit is emitted per play;
    /// absent when the aggregate row carries all three counts.
    /// </summary>
    [JsonPropertyName("category")]             public string? Category { get; init; }
    [JsonPropertyName("rush")]                 public int     Rush     { get; init; }
    [JsonPropertyName("pass")]                 public int     Pass     { get; init; }
    [JsonPropertyName("penalty")]              public int     Penalty  { get; init; }
    [JsonPropertyName("total")]                public int?    Total    { get; init; }
}

/// <summary>
/// Fallback for any <c>stat_type</c> value not matched by the known types.
/// Preserves all original JSON properties for forward-compatibility.
/// </summary>
public sealed class UnknownPlayStat : PlayStatistic
{
    public override PlayStatType StatType => PlayStatType.Unknown;

    /// <summary>The raw, unrecognised <c>stat_type</c> string from the payload.</summary>
    public string? RawStatType { get; init; }

    /// <summary>All JSON properties preserved as raw elements.</summary>
    public Dictionary<string, JsonElement> RawProperties { get; init; } = new();
}

// ─────────────────────────────────────────────────────────────────────────────
//  Polymorphic JSON converter
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Reads the <c>stat_type</c> field first, then dispatches deserialization to
/// the appropriate <see cref="PlayStatistic"/> concrete subclass.
/// </summary>
public sealed class PlayStatisticConverter : JsonConverter<PlayStatistic>
{
    private static readonly Dictionary<string, Type> StatTypeMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["rush"]               = typeof(RushPlayStat),
            ["pass"]               = typeof(PassPlayStat),
            ["receive"]            = typeof(ReceivePlayStat),
            ["defense"]            = typeof(DefensePlayStat),
            ["fumble"]             = typeof(FumblePlayStat),
            ["penalty"]            = typeof(PenaltyPlayStat),
            ["kick"]               = typeof(KickPlayStat),
            ["punt"]               = typeof(PuntPlayStat),
            ["return"]             = typeof(ReturnPlayStat),
            ["int_return"]         = typeof(IntReturnPlayStat),
            ["misc_return"]        = typeof(MiscReturnPlayStat),
            ["field_goal"]         = typeof(FieldGoalPlayStat),
            ["extra_point"]        = typeof(ExtraPointPlayStat),
            ["conversion"]         = typeof(ConversionPlayStat),
            ["defense_conversion"] = typeof(DefenseConversionPlayStat),
            ["down_conversion"]    = typeof(DownConversionPlayStat),
            ["block"]              = typeof(BlockPlayStat),
            ["first_down"]         = typeof(FirstDownPlayStat),
        };

    // Inner options intentionally omit this converter to prevent infinite recursion.
    private static readonly JsonSerializerOptions _innerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition      = JsonIgnoreCondition.WhenWritingNull,
    };

    public override PlayStatistic? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("stat_type", out var statTypeProp))
            return BuildUnknown(root, rawStatType: null);

        var statTypeStr = statTypeProp.GetString() ?? string.Empty;

        if (!StatTypeMap.TryGetValue(statTypeStr, out var concreteType))
            return BuildUnknown(root, rawStatType: statTypeStr);

        var json = root.GetRawText();
        return (PlayStatistic?)JsonSerializer.Deserialize(json, concreteType, _innerOptions);
    }

    public override void Write(
        Utf8JsonWriter writer,
        PlayStatistic value,
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, _innerOptions);
    }

    private static UnknownPlayStat BuildUnknown(JsonElement root, string? rawStatType)
    {
        var props = new Dictionary<string, JsonElement>();
        foreach (var prop in root.EnumerateObject())
            props[prop.Name] = prop.Value.Clone();

        return new UnknownPlayStat
        {
            RawStatType   = rawStatType,
            RawProperties = props,
            Player        = TryDeserialize<PlayStatPlayer>(root, "player"),
            Team          = TryDeserialize<PlayStatTeam>(root, "team"),
        };
    }

    private static T? TryDeserialize<T>(JsonElement root, string propertyName)
        => root.TryGetProperty(propertyName, out var el)
            ? JsonSerializer.Deserialize<T>(el.GetRawText(), _innerOptions)
            : default;
}