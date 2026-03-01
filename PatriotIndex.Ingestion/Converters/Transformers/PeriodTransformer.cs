using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Ingestion.Converters.Transformers;

public class PeriodTransformer(ILogger<PeriodTransformer> logger) : IPeriodTransformer
{
    public Period Transform(Game game, JsonNavigator nav, DriveAggregator driveAggregator)
    {
        var period = new Period
        {
            Id = nav["id"].GetGuid(),
            Sequence = nav["sequence"].GetInt64(),
            Type = nav["period_type"].GetString(),
            Number = nav["number"].GetInt32(),
            HomeScore = nav["scoring"]["home"]["points"].GetInt32(),
            AwayScore = nav["scoring"]["away"]["points"].GetInt32(),
        };

        foreach (var item in nav["pbp"].EnumerateArray())
        {
            if (item["type"].GetString() != "drive")
            {
                logger.LogInformation("Skipping non-drive pbp item {Id} with item type {Type}", item.Optional("id")?.GetString(), item["type"].GetString());
                continue;
            }

            var drive = driveAggregator.Process(item, period.Id);
            if (drive is not null)
            {
                drive.GameId = game.Id;
                //period.Drives.Add(drive);
            }
            else
            {
                logger.LogInformation("Drive {DriveId} already seen — plays merged into existing drive", item["id"].GetString());
            }
        }

        return period;
    }
}
