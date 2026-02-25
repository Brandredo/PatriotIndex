using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Ingestion.Converters.Transformers;

public class DriveTransformer(DriveEventTransformer driveEventTransformer)
{
    
    public Drive Transform(JsonNavigator nav, Guid periodId)
    {
        
        var drive = new Drive
        {
            Id = nav["id"].GetGuid(),
            Sequence = nav["sequence"].GetInt32(),
            StartClock = nav["start_clock"].GetString(),
            EndClock = nav["end_clock"].GetString(),
            StartReason = nav["start_reason"].GetString(),
            EndReason = nav["end_reason"].GetString(),
            PlayCount = nav["play_count"].GetInt32(),
            Duration = nav["duration"].GetString(),
            FirstDowns = nav["first_downs"].GetInt32(),
            GainedYards = nav["gain"].GetInt32(),
            PenaltyYards = nav["penalty_yards"].GetInt32(),
            TeamSequence = nav["team_sequence"].GetInt32(),
            FirstDriveYardLine = nav["first_drive_yardline"].GetInt32(),
            LastDriveYardLine = nav["last_drive_yardline"].GetInt32(),
            FarthestDriveYardLine = nav["farthest_drive_yardline"].GetInt32(),
            NetYards = nav["net_yards"].GetInt32(),
            PatPointsAttempted = nav["pat_points_attempted"].GetInt32(),
            OffensiveTeamId = nav["offensive_team"]["id"].GetGuid(),
            DefensiveTeamId = nav["defensive_team"]["id"].GetGuid(),
        };

        foreach (var eventNav in nav["events"].EnumerateArray())
        {
            if (eventNav["type"].GetString() != "play")
            {
                Console.WriteLine($"Skipping non-play event {eventNav["type"].GetString()}");
                continue;
            }
            
            var evt = driveEventTransformer.Transform(eventNav, periodId);
            evt.DriveId = drive.Id;
            drive.Plays.Add(evt);
        }

        return drive;
    }
}
