using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Ingestion.Converters.Transformers;

public class GameTransformer(IPeriodTransformer periodTransformer, DriveAggregatorFactory driveAggregatorFactory) : IJsonTransformer<Game>
{
    public Game Transform(JsonNavigator nav)
    {
        var game = new Game
        {
            Id = nav["id"].GetGuid(),
            Status = nav["status"].GetString(),
            Scheduled = nav["scheduled"].GetDateTime(),
            Attendance = nav["attendance"].GetInt32(),
            SrId = nav["sr_id"].GetString(),
            GameType = nav["game_type"].GetString(),
            ConferenceGame = nav["conference_game"].GetBoolean(),
            Title = nav["title"].GetString(),
            Duration = nav["duration"].GetString(),
            SeasonYear = nav["summary"]["season"]["year"].GetInt32(),
            SeasonType = nav["summary"]["season"]["type"].GetString(),
            WeekSequence = nav["summary"]["week"]["sequence"].GetInt32(),
            VenueId = nav["summary"]["venue"]["id"].GetGuid(),
            HomeTeamId = nav["summary"]["home"]["id"].GetGuid(),
            AwayTeamId = nav["summary"]["away"]["id"].GetGuid(),
            HomePoints = nav["summary"]["home"]["points"].GetInt32(),
            AwayPoints = nav["summary"]["away"]["points"].GetInt32(),
            WeatherCondition = nav["weather"]["condition"].GetString(),
            WeatherTemp = nav["weather"]["temp"].GetInt32(),
            WeatherHumidity = nav["weather"]["humidity"].GetInt32(),
            WeatherWindSpeed = nav["weather"]["wind"]["speed"].GetInt32(),
            WeatherWindDirection = nav["weather"]["wind"]["direction"].GetString(),
        };

        // Fresh aggregator per game — lifetime is naturally scoped here
        var driveAggregator = driveAggregatorFactory.Create();

        foreach (var periodNav in nav["periods"].EnumerateArray())
        {
            var period = periodTransformer.Transform(game, periodNav, driveAggregator);
            period.GameId = game.Id;
            game.Periods.Add(period);
        }

        return game;
    }
}
