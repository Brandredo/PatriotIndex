using PatriotIndex.Domain.Entities;
using PatriotIndex.Ingestion.Converters.Transformers;

namespace PatriotIndex.Ingestion.Converters;

public class DriveAggregator(DriveTransformer driveTransformer, DriveEventTransformer driveEventTransformer)
{
    private readonly Dictionary<Guid, Drive> _drives = new();

    public Drive? Process(JsonNavigator nav)
    {
        var driveId = nav["id"].GetGuid();

        if (_drives.TryGetValue(driveId, out var existingDrive))
        {
            MergePlays(nav, existingDrive);
            return null;
        }

        var drive = driveTransformer.Transform(nav);
        //drive.PeriodId = periodId;
        _drives[drive.Id] = drive;
        return drive;
    }

    private void MergePlays(JsonNavigator nav, Drive existingDrive)
    {
        var existingPlayIds = existingDrive.Plays
            .Select(p => p.Id)
            .ToHashSet();

        foreach (var eventNav in nav["events"].EnumerateArray())
        {
            if (eventNav["type"].GetString() != "play")
                continue;

            var play = driveEventTransformer.Transform(eventNav);

            if (existingPlayIds.Contains(play.Id))
                continue;

            play.DriveId = existingDrive.Id;
            existingDrive.Plays.Add(play);
        }
    }
}