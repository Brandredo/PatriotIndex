using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Ingestion.Converters.Transformers;

public class DriveEventTransformer
{
    public DriveEvent Transform(JsonNavigator nav, Guid periodId)
    {
        return new DriveEvent
        {
            Id = nav["id"].GetGuid(),
            PeriodId = periodId,
            Sequence = nav["sequence"].GetDecimal(),
            EventType = nav["type"].GetString(),
            PlayType = nav["play_type"].GetString(),
            Clock = nav["clock"].GetString(),
            WallClock = nav["wall_clock"].GetString(),
            Description = nav["description"].GetString(),
            HomeScore = nav["home_points"].GetInt32(),
            AwayScore = nav["away_points"].GetInt32(),
            StartClock = nav["start_situation"]["clock"].GetString(),
            StartDown = nav["start_situation"]["down"].GetInt32(),
            StartYardsToGain = nav["start_situation"]["yfd"].GetInt32(),
            StartLocationYardLine = nav["start_situation"]["location"]["yardline"].GetInt32(),
            StartPossessionTeamId = nav["start_situation"]["possession"]["id"].GetGuid(),
            EndClock = nav["end_situation"]["clock"].GetString(),
            EndDown = nav["end_situation"]["down"].GetInt32(),
            EndYardsToGain = nav["end_situation"]["yfd"].GetInt32(),
            EndLocationYardLine = nav["start_situation"]["location"]["yardline"].GetInt32(),
            EndPossessionTeamId = nav["start_situation"]["possession"]["id"].GetGuid(),
            PassRoute = nav.Optional("pass_route")?.GetString() ?? string.Empty,
            QbSnap = nav.Optional("qb_at_snap")?.GetString(),
            Huddle = nav.Optional("huddle")?.GetString(),
            MenInBox = nav.Optional("men_in_box")?.GetInt32(),
            LeftTightEnds = nav.Optional("left_tightends")?.GetInt32(),
            RightTightEnds = nav.Optional("right_tightends")?.GetInt32(),
            HashMark = nav.Optional("hash_mark")?.GetString(),
            PlayersRushed = nav.Optional("players_rushed")?.GetInt32(),
            PlayDirection = nav.Optional("play_direction")?.GetString(),
            PocketLocation = nav.Optional("pocket_location")?.GetString(),
            FakePunt = nav.Optional("fake_punt")?.GetBoolean() ?? false,
            FakeFieldGoal = nav.Optional("fake_field_goal")?.GetBoolean() ?? false,
            ScreenPass = nav.Optional("screen_pass")?.GetBoolean() ?? false,
            Blitz = nav.Optional("blitz")?.GetBoolean() ?? false,
            PlayAction = nav.Optional("play_action")?.GetBoolean() ?? false,
            RunPassOption = nav.Optional("run_pass_option")?.GetBoolean() ?? false,
        };
    }
}
