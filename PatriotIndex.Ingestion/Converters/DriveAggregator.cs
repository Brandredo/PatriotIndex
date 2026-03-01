using PatriotIndex.Domain.Entities;
using PatriotIndex.Ingestion.Converters.Transformers;

namespace PatriotIndex.Ingestion.Converters;

public class DriveAggregator(DriveTransformer driveTransformer, DriveEventTransformer driveEventTransformer)
{
    private readonly Dictionary<Guid, Drive> _drives = new();

    public Drive? Process(JsonNavigator nav, Guid periodId)
    {
        var driveId = nav["id"].GetGuid();

        if (_drives.TryGetValue(driveId, out var existingDrive))
        {
            MergePlays(nav, existingDrive, periodId);
            return null;
        }

        var drive = driveTransformer.Transform(nav, periodId);
        //drive.PeriodId = periodId;
        _drives[drive.Id] = drive;
        return drive;
    }

    private void MergePlays(JsonNavigator nav, Drive existingDrive, Guid periodId)
    {
        var existingPlayIds = existingDrive.Plays
            .Select(p => p.Id)
            .ToHashSet();

        foreach (var eventNav in nav["events"].EnumerateArray())
        {
            if (eventNav["type"].GetString() != "play")
                continue;

            var play = driveEventTransformer.Transform(eventNav, periodId);

            if (existingPlayIds.Contains(play.Id))
                continue;

            play.DriveId = existingDrive.Id;
            //existingDrive.Plays.Add(play);
        }
    }
}