using System.Runtime.Serialization;

namespace PatriotIndex.Domain.Enums;

public enum PlayRole
{
    [EnumMember(Value = "ast_sack")]
    AstSack,

    [EnumMember(Value = "ast_tackle")]
    AstTackle,

    [EnumMember(Value = "block")]
    Block,

    [EnumMember(Value = "catch")]
    Catch,

    [EnumMember(Value = "conversion")]
    Conversion,

    [EnumMember(Value = "defend")]
    Defend,

    [EnumMember(Value = "defender")]
    Defender,

    [EnumMember(Value = "down")]
    Down,

    [EnumMember(Value = "fair_catch")]
    FairCatch,

    [EnumMember(Value = "first_down")]
    FirstDown,

    [EnumMember(Value = "force_fum")]
    ForceFum,

    [EnumMember(Value = "force_fum_ast")]
    ForceFumAst,

    [EnumMember(Value = "fum_rec")]
    FumRec,

    [EnumMember(Value = "fumble")]
    Fumble,

    [EnumMember(Value = "hit")]
    Hit,

    [EnumMember(Value = "hold")]
    Hold,

    [EnumMember(Value = "kick")]
    Kick,

    [EnumMember(Value = "lateral")]
    Lateral,

    [EnumMember(Value = "miss")]
    Miss,

    [EnumMember(Value = "muff")]
    Muff,

    [EnumMember(Value = "out_of_bounds")]
    OutOfBounds,

    [EnumMember(Value = "pass")]
    Pass,

    [EnumMember(Value = "penalty")]
    Penalty,

    [EnumMember(Value = "punt")]
    Punt,

    [EnumMember(Value = "receive")]
    Receive,

    [EnumMember(Value = "recovery")]
    Recovery,

    [EnumMember(Value = "return")]
    Return,

    [EnumMember(Value = "reviewed_by")]
    ReviewedBy,

    [EnumMember(Value = "rush")]
    Rush,

    [EnumMember(Value = "sack")]
    Sack,

    [EnumMember(Value = "safety")]
    Safety,

    [EnumMember(Value = "snap")]
    Snap,

    [EnumMember(Value = "tackle")]
    Tackle,

    [EnumMember(Value = "timeout")]
    Timeout,

    [EnumMember(Value = "touchback")]
    Touchback
}