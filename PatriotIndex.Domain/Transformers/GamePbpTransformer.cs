using System.Text.Json;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Helpers;

namespace PatriotIndex.Domain.Transformers;

public class GamePbpTransformer(string json)
{
    private Game? _game;
    private readonly Dictionary<Guid, Drive> _drives = new();

    public Game? Transform()
    {
        using var trf = new JsonTraverser(json);

        _game = new Game
        {
            Id                    = trf.GetGuid("id") ?? throw new Exception("game id is null"),
            SrId                  = trf.GetString("sr_id"),
            Status                = trf.GetStringN("status"),
            Scheduled             = trf.GetDateTime("scheduled"),
            Attendance            = trf.GetIntN("attendance"),
            GameType              = trf.GetStringN("game_type"),
            Title                 = trf.GetStringN("title"),
            Duration              = trf.GetStringN("duration"),
            SeasonYear            = trf.GetIntN("summary.season.year"),
            SeasonType            = trf.GetStringN("summary.season.type"),
            WeekSequence          = trf.GetIntN("summary.week.sequence"),
            WeekTitle             = trf.GetStringN("summary.week.title"),
            HomeTeamId            = trf.GetGuid("summary.home.id"),
            AwayTeamId            = trf.GetGuid("summary.away.id"),
            HomePoints            = trf.GetIntN("summary.home.points"),
            AwayPoints            = trf.GetIntN("summary.away.points"),
            VenueId               = trf.GetGuid("summary.venue.id"),
            Venue                 = trf.GetGuid("summary.venue.id") is { } venueId ? new Venue
            {
                Id       = venueId,
                Name     = trf.GetString("summary.venue.name") ?? "Unknown",
                City     = trf.GetStringN("summary.venue.city"),
                State    = trf.GetStringN("summary.venue.state"),
                Country  = trf.GetStringN("summary.venue.country"),
                Zip      = trf.GetStringN("summary.venue.zip"),
                Address  = trf.GetStringN("summary.venue.address"),
                Capacity = trf.GetIntN("summary.venue.capacity"),
                Surface  = trf.GetStringN("summary.venue.surface"),
                RoofType = trf.GetStringN("summary.venue.roof_type"),
                SrId     = trf.GetStringN("summary.venue.sr_id"),
                Lat      = trf.GetStringN("summary.venue.location.lat"),
                Lng      = trf.GetStringN("summary.venue.location.lng"),
            } : null,
            WeatherCondition      = trf.GetStringN("weather.condition"),
            WeatherTemp           = trf.GetIntN("weather.temp"),
            WeatherHumidity       = trf.GetIntN("weather.humidity"),
            WeatherWindSpeed      = trf.GetIntN("weather.wind.speed"),
            WeatherWindDirection  = trf.GetStringN("weather.wind.direction"),
            BroadcastNetwork      = trf.GetStringN("broadcast.network"),
            NeutralSite           = trf.GetBool("neutral_site"),
            ConferenceGame        = trf.GetBool("conference_game"),
        };

        _game.Periods = TransformPeriods(trf);
        _game.Drives  = _drives.Values;

        return _game;
    }

    private ICollection<Period> TransformPeriods(JsonTraverser trf)
    {
        return trf.GetArrayList("periods", period =>
        {
            var p = new Period
            {
                Id        = period.GetGuid("id") ?? throw new Exception("period id is null"),
                Number    = period.GetShort("number"),
                GameId    = _game!.Id,
                Type      = period.GetString("period_type"),
                Sequence  = period.GetShort("sequence"),
                HomeScore = period.GetShort("scoring.home.points"),
                AwayScore = period.GetShort("scoring.away.points"),
            };

            period.GetArrayList("pbp", driveEl =>
            {
                var d = new Drive
                {
                    Id                    = driveEl.GetGuid("id") ?? throw new Exception("drive id is null"),
                    Sequence              = driveEl.GetShortN("sequence"),
                    Type                  = driveEl.GetStringN("type"),
                    GameId                = _game!.Id,
                    TeamSequence          = driveEl.GetShortN("team_sequence"),
                    StartReason           = driveEl.GetStringN("start_reason"),
                    EndReason             = driveEl.GetStringN("end_reason"),
                    PlayCount             = driveEl.GetShortN("play_count"),
                    Duration              = driveEl.GetStringN("duration"),
                    FirstDowns            = driveEl.GetShortN("first_downs"),
                    GainedYards           = driveEl.GetShortN("gained_yards"),
                    PenaltyYards          = driveEl.GetShortN("penalty_yards"),
                    NetYards              = driveEl.GetShortN("net_yards"),
                    StartClock            = driveEl.GetStringN("start_clock"),
                    EndClock              = driveEl.GetStringN("end_clock"),
                    OffensiveTeamId       = driveEl.GetGuid("offensive_team.id"),
                    DefensiveTeamId       = driveEl.GetGuid("defensive_team.id"),
                    OffensivePoints       = driveEl.GetShortN("offensive_team.points"),
                    DefensivePoints       = driveEl.GetShortN("defensive_team.points"),
                    FirstDriveYardLine    = driveEl.GetShortN("first_drive_yardline"),
                    LastDriveYardLine     = driveEl.GetShortN("last_drive_yardline"),
                    FarthestDriveYardLine = driveEl.GetShortN("farthest_drive_yardline"),
                    PatPointsAttempted    = driveEl.GetShortN("pat_points_attempted"),
                };

                var (plays, events) = TransformEvents(driveEl, d.Id, p.Id);
                d.Plays  = plays;
                d.Events = events;

                if (!_drives.TryGetValue(d.Id, out var existing))
                {
                    _drives[d.Id] = d;
                }
                else
                {
                    foreach (var play  in plays)  existing.Plays.Add(play);
                    foreach (var evt   in events) existing.Events.Add(evt);
                }

                return d;
            });

            return p;
        });
    }

    private (List<Play> plays, List<GameEvent> events) TransformEvents(
        JsonTraverserItem driveEl, Guid driveId, Guid periodId)
    {
        var plays  = new List<Play>();
        var events = new List<GameEvent>();

        driveEl.GetArrayList("events", ev =>
        {
            var id       = ev.GetGuid("id") ?? throw new Exception("event id is null");
            var sequence = ev.GetDecimal("sequence");
            var clock    = ev.GetString("clock");
            var desc     = ev.GetStringN("description");
            var evType   = ev.GetStringN("type") ?? string.Empty;
            var playType = ev.GetStringN("play_type");

            if (evType == "play" && playType is not null)
            {
                var play = new Play
                {
                    Id               = id,
                    GameId           = _game!.Id,
                    DriveId          = driveId,
                    PeriodId         = periodId,
                    Sequence         = sequence,
                    Clock            = clock,
                    WallClock        = ev.GetDateTimeOffset("wall_clock"),
                    PlayType         = playType,
                    Description      = desc,
                    HomePoints       = ev.GetInt("home_points"),
                    AwayPoints       = ev.GetInt("away_points"),
                    QbSnap           = ev.GetStringN("qb_at_snap"),
                    Huddle           = ev.GetStringN("huddle"),
                    MenInBox         = ev.GetIntN("men_in_box"),
                    LeftTightEnds    = ev.GetIntN("left_tightends"),
                    RightTightEnds   = ev.GetIntN("right_tightends"),
                    HashMark         = ev.GetStringN("hash_mark"),
                    Blitz            = ev.GetBoolN("blitz"),
                    PlayAction       = ev.GetBoolN("play_action"),
                    RunPassOption    = ev.GetBoolN("run_pass_option"),
                    ScreenPass       = ev.GetBoolN("screen_pass"),
                    FakePunt         = ev.GetBoolN("fake_punt"),
                    FakeFieldGoal    = ev.GetBoolN("fake_field_goal"),
                    PlayDirection    = ev.GetStringN("play_direction"),
                    PassRoute        = ev.GetStringN("pass_route"),
                    StartSituation   = MapSituation(ev, "start_situation"),
                    EndSituation     = MapSituation(ev, "end_situation"),
                    Statistics       = MapStatistics(ev),
                    Details          = MapDetails(ev),
                };
                plays.Add(play);
            }
            else
            {
                events.Add(new GameEvent
                {
                    Id          = id,
                    GameId      = _game!.Id,
                    DriveId     = driveId,
                    PeriodId    = periodId,
                    Sequence    = sequence,
                    Clock       = clock,
                    EventType   = evType,
                    Description = desc,
                });
            }

            return (object?)null;
        });

        return (plays, events);
    }

    private static GameSituation MapSituation(JsonTraverserItem ev, string prefix) => new()
    {
        Clock            = ev.GetString($"{prefix}.clock"),
        Down             = ev.GetIntN($"{prefix}.down"),
        YardsToFirstDown = ev.GetInt($"{prefix}.yfd"),
        Yardline         = ev.GetInt($"{prefix}.location.yardline"),
        YardlineTeam     = ev.GetStringN($"{prefix}.location.alias"),
        PossessionTeamId = ev.GetGuid($"{prefix}.possession.id"),
    };

    private static List<PlayStat> MapStatistics(JsonTraverserItem ev)
    {
        var statsEl = ev.Navigate("statistics");
        if (statsEl is null || statsEl.Value.ValueKind != JsonValueKind.Array)
            return [];

        var result = new List<PlayStat>();
        foreach (var el in statsEl.Value.EnumerateArray())
        {
            var stat = new PlayStat
            {
                StatType        = GetStr(el, "stat_type"),
                Player          = MapStatPlayer(el),
                Team            = MapStatTeam(el),
                Attempt         = GetInt(el, "attempt"),
                Yards           = GetInt(el, "yards"),
                Touchdowns      = GetInt(el, "touchdowns"),
                FirstDown       = GetInt(el, "first_down"),
                BrokenTackles   = GetInt(el, "broken_tackles"),
                TackleForLoss   = GetInt(el, "tlost"),
                TackleForLossYards = GetInt(el, "tlost_yards"),
                Scramble        = GetInt(el, "scramble"),
                KneelDown       = GetInt(el, "kneel_down"),
                YardsAfterContact = GetInt(el, "yards_after_contact"),
                Complete        = GetInt(el, "complete"),
                AttemptYards    = GetInt(el, "attempt_yards"),
                AirYards        = GetInt(el, "air_yards"),
                Sack            = GetInt(el, "sack"),
                SackYards       = GetInt(el, "sack_yards"),
                PocketTime      = GetDouble(el, "pocket_time"),
                Interceptions   = GetInt(el, "interceptions"),
                Defended        = GetInt(el, "defended"),
                Batted          = GetInt(el, "batted"),
                Hurry           = GetInt(el, "hurry"),
                Knockdown       = GetInt(el, "knockdown"),
                Blitz           = GetInt(el, "blitz"),
                PoorThrow       = GetInt(el, "poor_throw"),
                ThrowAway       = GetInt(el, "throw_away"),
                Spike           = GetInt(el, "spike"),
                IncompletionType = GetStrN(el, "incompletion_type"),
                Target          = GetInt(el, "target"),
                Reception       = GetInt(el, "reception"),
                YardsAfterCatch = GetInt(el, "yards_after_catch"),
                Dropped         = GetInt(el, "dropped"),
                Tackle          = GetInt(el, "tackle"),
                AstTackle       = GetInt(el, "ast_tackle"),
                MissedTackles   = GetInt(el, "missed_tackles"),
                DefTd           = GetInt(el, "def_td"),
                SafetyTackle    = GetInt(el, "safety_tackle"),
                Interception    = GetInt(el, "interception"),
                ForcedFumble    = GetInt(el, "forced_fumble"),
                NetYards        = GetInt(el, "net_yards"),
                Touchback       = GetInt(el, "touchback"),
                FairCatch       = GetInt(el, "fair_catch"),
                ReturnYards     = GetInt(el, "return_yards"),
                OnsideAttempt   = GetInt(el, "onside_attempt"),
                OnsideSuccess   = GetInt(el, "onside_success"),
                PuntYards       = GetInt(el, "punt_yards"),
                HangTime        = GetInt(el, "hang_time"),
                Fumble          = GetInt(el, "fumble"),
                FumbleLost      = GetInt(el, "fumble_lost"),
                OwnRec          = GetInt(el, "own_rec"),
                OppRec          = GetInt(el, "opp_rec"),
                Made            = GetBool(el, "made"),
                Distance        = GetInt(el, "distance"),
                PenaltyYards    = GetInt(el, "penalty_yards"),
                Declined        = GetBool(el, "declined"),
                PenaltyType     = GetStrN(el, "penalty_type"),
                Success         = GetBool(el, "success"),
                ConversionType  = GetStrN(el, "conversion_type"),
                Nullified       = GetBool(el, "nullified"),
                Inside20        = GetInt(el, "inside_20"),
                GoalToGo        = GetInt(el, "goal_to_go"),
                Safety          = GetInt(el, "safety"),
            };
            result.Add(stat);
        }
        return result;
    }

    private static PlayStatPlayerRef MapStatPlayer(JsonElement el)
    {
        if (!el.TryGetProperty("player", out var p))
            return new PlayStatPlayerRef();

        return new PlayStatPlayerRef
        {
            Id       = GetGuid(p, "id"),
            Name     = GetStr(p, "name"),
            Position = GetStrN(p, "position"),
            Jersey   = GetStrN(p, "jersey"),
            SrId     = GetStrN(p, "sr_id"),
        };
    }

    private static PlayStatTeamRef MapStatTeam(JsonElement el)
    {
        if (!el.TryGetProperty("team", out var t))
            return new PlayStatTeamRef();

        return new PlayStatTeamRef
        {
            Id    = GetGuid(t, "id"),
            Name  = GetStr(t, "name"),
            Alias = GetStr(t, "alias"),
            SrId  = GetStrN(t, "sr_id"),
        };
    }

    private static List<PlayDetail> MapDetails(JsonTraverserItem ev)
    {
        var detailsEl = ev.Navigate("details");
        if (detailsEl is null || detailsEl.Value.ValueKind != JsonValueKind.Array)
            return [];

        var result = new List<PlayDetail>();
        foreach (var el in detailsEl.Value.EnumerateArray())
        {
            var detail = new PlayDetail
            {
                Sequence    = el.TryGetProperty("sequence", out var seq) && seq.TryGetInt64(out var s) ? s : 0,
                Category    = GetStr(el, "category"),
                Description = GetStrN(el, "description"),
                Players     = MapDetailPlayers(el),
            };
            result.Add(detail);
        }
        return result;
    }

    private static List<PlayDetailPlayer> MapDetailPlayers(JsonElement el)
    {
        if (!el.TryGetProperty("players", out var arr) || arr.ValueKind != JsonValueKind.Array)
            return [];

        var result = new List<PlayDetailPlayer>();
        foreach (var p in arr.EnumerateArray())
        {
            result.Add(new PlayDetailPlayer
            {
                Id       = GetGuid(p, "id"),
                Name     = GetStr(p, "name"),
                Role     = GetStr(p, "role"),
                Position = GetStrN(p, "position"),
            });
        }
        return result;
    }

    // ── JsonElement helpers ────────────────────────────────────────────────

    private static Guid GetGuid(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v)) return Guid.Empty;
        if (v.TryGetGuid(out var g)) return g;
        if (v.ValueKind == JsonValueKind.String && Guid.TryParse(v.GetString(), out var parsed)) return parsed;
        return Guid.Empty;
    }

    private static int? GetInt(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v) || v.ValueKind == JsonValueKind.Null) return null;
        return v.TryGetInt32(out var i) ? i : null;
    }

    private static double? GetDouble(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v) || v.ValueKind == JsonValueKind.Null) return null;
        return v.TryGetDouble(out var d) ? d : null;
    }

    private static bool? GetBool(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v) || v.ValueKind == JsonValueKind.Null) return null;
        return v.ValueKind switch
        {
            JsonValueKind.True  => true,
            JsonValueKind.False => false,
            JsonValueKind.Number => v.TryGetInt32(out var i) ? i != 0 : null,
            _ => null
        };
    }

    private static string GetStr(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v)) return string.Empty;
        return v.GetString() ?? string.Empty;
    }

    private static string? GetStrN(JsonElement el, string prop)
    {
        if (!el.TryGetProperty(prop, out var v) || v.ValueKind == JsonValueKind.Null) return null;
        var s = v.GetString();
        return string.IsNullOrWhiteSpace(s) ? null : s;
    }
}
