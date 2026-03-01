using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Helpers;

namespace PatriotIndex.Domain.Transformers;

public class GamePbpTransformer(string json)
{
    private Game? _game;
    
    public Game? Transform()
    {
        using var trf = new JsonTraverser(json);

        _game = new Game
        {
            Id = trf.GetGuid("id") ?? throw new Exception("game id is null"),
            SrId = trf.GetString("sr_id"),
            Status = trf.GetStringN("status"),
            Scheduled = trf.GetDateTime( "scheduled"),
            Attendance = trf.GetIntN("attendance"),
            GameType = trf.GetStringN("game_type"),
            Title = trf.GetStringN("title"),
            Duration = trf.GetStringN("duration"),
            SeasonYear = trf.GetIntN("summary.season.year"),
            SeasonType = trf.GetStringN("summary.season.type"),
            WeekSequence = trf.GetIntN("summary.week.sequence"),
            WeekTitle = trf.GetStringN("summary.week.title"),
            HomeTeamId = trf.GetGuid("summary.home.id"),
            AwayTeamId = trf.GetGuid("summary.away.id"),
            HomePoints = trf.GetIntN("summary.home.points"),
            AwayPoints = trf.GetIntN("summary.away.points"),
            VenueId = trf.GetGuid("summary.venue.id"),
            WeatherCondition = trf.GetStringN("weather.condition"),
            WeatherTemp = trf.GetIntN("weather.temp"),
            WeatherHumidity = trf.GetIntN("weather.humidity"),
            WeatherWindSpeed = trf.GetIntN("weather.wind.speed"),
            WeatherWindDirection = trf.GetStringN("weather.wind.direction"),
            BroadcastNetwork = trf.GetStringN("broadcast.network"),
            NeutralSite = trf.GetBool( "neutral_site"),
            ConferenceGame = trf.GetBool( "conference_game"),
            Periods = TransformPeriods(trf)
            // HomeTeam = null,
            // AwayTeam = null,
            // Venue = null,
            // Drives = null,
            // Periods = null
        };

        return _game;
    }

    private IEnumerable<Period> TransformPeriods(JsonTraverser trf)
    {

        var periods = trf.GetArrayList("periods", period =>
        {
            var p = new Period
            {
                Id = period.GetGuid("id") ?? throw new Exception("period id is null"),
                Number = period.GetShort("number"),
                //GameId = period.GetGuid("game.id") ?? throw new Exception("game id is null"),
                Type = period.GetString("period_type"),
                Sequence = period.GetShort("sequence"),
                HomeScore = period.GetShort("scoring.home.points"),
                AwayScore = period.GetShort("scoring.away.points"),
                // Game = null,
                // Drives = null
            };

            var drives = period.GetArrayList("drives", drive =>
            {

                var d = new Drive
                {
                    Id = drive.GetGuid("id") ?? throw new Exception("drive id is null"),
                    Sequence = drive.GetShort("sequence"),
                    GameId = trf.GetGuid("game.id") ?? throw new Exception("game id is null"),
                    TeamSequence = drive.GetShortN("team_sequence"),
                    StartReason = drive.GetStringN("start_reason"),
                    EndReason = drive.GetStringN("end_reason"),
                    PlayCount = drive.GetShortN("play_count"),
                    Duration = drive.GetStringN("duration"),
                    FirstDowns = drive.GetShortN("first_downs"),
                    GainedYards = drive.GetShortN("gained_yards"),
                    PenaltyYards = drive.GetShortN("penalty_yards"),
                    NetYards = drive.GetShortN("net_yards"),
                    StartClock = drive.GetStringN("start_clock"),
                    EndClock = drive.GetStringN("end_clock"),
                    OffensiveTeamId = drive.GetGuid("offensive_team.id") ?? throw new Exception("offensive team id is null"),
                    DefensiveTeamId = drive.GetGuid("defensive_team.id") ?? throw new Exception("defensive team id is null"),
                    OffensivePoints = drive.GetShortN("offensive_team.points"),
                    DefensivePoints = drive.GetShortN("defensive_team.points"),
                    FirstDriveYardLine = drive.GetShortN("first_drive_yardline"),
                    LastDriveYardLine = drive.GetShortN("last_drive_yardline"),
                    FarthestDriveYardLine = drive.GetShortN("farthest_drive_yardline"),
                    PatPointsAttempted = drive.GetShortN("pat_points_attempted"),
                    // Game = null,
                    // OffensiveTeam = null,
                    // DefensiveTeam = null,
                    // Plays = null
                };


                var driveEvents = drive.GetArrayList("events", driveEvent =>
                {

                    var e = new DriveEvent
                    {
                        DriveId = d.Id, // a play should be a part of a drive
                        PeriodId = p.Id, // a play should be a part of a period
                        EventType = drive.GetString("type"),
                        Id = driveEvent.GetGuid("id") ?? throw new Exception("drive event id is null"),
                        Sequence = driveEvent.GetDecimal("sequence"),
                        Clock = driveEvent.GetString("clock"),
                        WallClock = driveEvent.GetString("wall_clock"),
                        Description = driveEvent.GetStringN("description"),
                        HomeScore = driveEvent.GetShort("home_points"),
                        AwayScore = driveEvent.GetShort("away_points"),
                        PlayType = driveEvent.GetStringN("play_type"),
                        PassRoute = driveEvent.GetStringN("pass_route"),
                        QbSnap = driveEvent.GetStringN("qb_at_snap"),
                        Huddle = driveEvent.GetStringN("huddle"),
                        MenInBox = driveEvent.GetShortN("men_in_box"),
                        LeftTightEnds = driveEvent.GetShortN("left_tightends"),
                        RightTightEnds = driveEvent.GetShortN("right_tightends"),
                        HashMark = driveEvent.GetStringN("hash_mark"),
                        PlayersRushed = driveEvent.GetShortN("players_rushed"),
                        PlayDirection = driveEvent.GetStringN("play_direction"),
                        PocketLocation = driveEvent.GetStringN("pocket_location"),
                        FakePunt = driveEvent.GetBoolN("fake_punt"),
                        FakeFieldGoal = driveEvent.GetBoolN("fake_field_goal"),
                        ScreenPass = driveEvent.GetBoolN("screen_pass"),
                        Blitz = driveEvent.GetBoolN("blitz"),
                        PlayAction = driveEvent.GetBoolN("play_action"),
                        RunPassOption = driveEvent.GetBoolN("run_pass_option"),
                        StartClock = driveEvent.GetStringN("start_situation.clock"),
                        StartDown = driveEvent.GetShortN( "start_situation.down"),
                        StartYardsToGain = driveEvent.GetShortN( "start_situation.yfd"),
                        StartLocationYardLine = driveEvent.GetShortN( "start_situation.location.yardline"),
                        StartPossessionTeamId = driveEvent.GetGuid( "start_situation.location.id"),
                        EndClock = driveEvent.GetStringN("end_situation.clock"),
                        EndDown = driveEvent.GetShortN( "end_situation.down"),
                        EndYardsToGain = driveEvent.GetShortN( "end_situation.yfd"),
                        EndLocationYardLine = driveEvent.GetShortN( "end_situation.location.yardline"),
                        EndPossessionTeamId = driveEvent.GetGuid( "end_situation.location.id"),
                        // Drive = null,
                        // EventStats = null,
                        // Period = null,
                        // StartTeam = null,
                        // EndTeam = null
                    };

                    return e;
                });
                
                d.Plays = driveEvents;
                
                return d;
            });
            
            p.Drives = drives;
            
            return p;
        });
            
        return periods;
    }
    
    
}