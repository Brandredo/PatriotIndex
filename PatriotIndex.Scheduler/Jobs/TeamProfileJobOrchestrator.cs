using System.Diagnostics;
using Hangfire;

namespace PatriotIndex.Scheduler.Jobs;

[AutomaticRetry(Attempts = 0)]
public class TeamProfileJobOrchestrator(
    TeamsRepository teamRepository,
    IBackgroundJobClient backgroundJobClient,
    ILogger<TeamProfileJobOrchestrator> logger)
{
    private readonly List<Guid> _teamIds =
    [
        new("f0e724b0-4cbf-495a-be47-013907608da9"), // 49ers
        new("7b112545-38e6-483c-a55c-96cf6ee49cb8"), // Bears
        new("ad4ae08f-d808-42d5-a1e6-e9bc4e34d123"), // Bengals
        new("768c92aa-75ff-4a43-bcc0-f2798c2e1724"), // Bills
        new("ce92bd47-93d5-4fe9-ada4-0fc681e6caa0"), // Broncos
        new("d5a2eb42-8065-4174-ab79-0a6fa820e35e"), // Browns
        new("4254d319-1bc7-4f81-b4ab-b5e6f3402b69"), // Buccaneers
        new("de760528-1dc0-416a-a978-b510d20692ff"), // Cardinals
        new("1f6dcffb-9823-43cd-9ff4-e7a8466749b5"), // Chargers
        new("6680d28d-d4d2-49f6-aace-5292d3ec02c2"), // Chiefs
        new("82cf9565-6eb9-4f01-bdbd-5aa0d472fcd9"), // Colts
        new("22052ff7-c065-42ee-bc8f-c4691c50e624"), // Commanders
        new("e627eec7-bbae-4fa4-8e73-8e1d6bc5c060"), // Cowboys
        new("4809ecb0-abd3-451d-9c4a-92a90b83ca06"), // Dolphins
        new("386bdbf9-9eea-4869-bb9a-274b0bc66e80"), // Eagles
        new("e6aa13a4-0055-48a9-bc41-be28dc106929"), // Falcons
        new("04aa1c9d-66da-489d-b16a-1dee3f2eec4d"), // Giants
        new("f7ddd7fa-0bae-4f90-bc8e-669e4d6cf2de"), // Jaguars
        new("5fee86ae-74ab-4bdd-8416-42a9dd9964f3"), // Jets
        new("c5a59daa-53a7-4de0-851f-fb12be893e9e"), // Lions
        new("a20471b4-a8d9-40c7-95ad-90cc30e46932"), // Packers
        new("f14bf5cc-9a82-4a38-bc15-d39f75ed5314"), // Panthers
        new("97354895-8c77-4fd4-a860-32e62ea7382a"), // Patriots
        new("7d4fcc64-9cb5-4d1b-8e75-8a906d1e1576"), // Raiders
        new("2eff2a03-54d4-46ba-890e-2bc3925548f3"), // Rams
        new("ebd87119-b331-4469-9ea6-d51fe3ce2f1c"), // Ravens
        new("0d855753-ea21-4953-89f9-0e20aff9eb73"), // Saints
        new("3d08af9e-c767-4f88-a7dc-b920c6d2b4a8"), // Seahawks
        new("cb2f9f1f-ac67-424e-9e72-1475cb0ed398"), // Steelers
        new("23ed0bf0-f058-11ee-9989-93cc4251593a"), // TBD
        new("82d2d380-3834-4938-835f-aec541e5ece7"), // Texans
        new("d26a1ca5-722d-4274-8f97-c92e49c96315"), // Titans
        new("33405046-04ee-4058-a950-d606f8c30852")
    ];

    public async Task RunAsync()
    {
        using var activity =
            PatriotIndexTelemetry.Source.StartActivity("TeamProfileJobOrchestrator.RunAsync", ActivityKind.Internal);

        logger.LogInformation("Starting Team Profile Job Orchestrator");

        var teamIds = await teamRepository.GetTeamIdsAsync(); // returns all 32 team ids

        if (teamIds.Count() == 0)
            teamIds = _teamIds;
        //teamIds = teamIds.Take(1);
        activity?.SetTag("team.count", teamIds.Count());
        //teamsIds = teamsIds.Take(1);
        foreach (var id in teamIds)
            backgroundJobClient.Enqueue<TeamProfileJob>(job => job.RunAsync(id, CancellationToken.None));
        logger.LogInformation("Team Profile Job Orchestrator completed");
        activity?.SetStatus(ActivityStatusCode.Ok);
    }
}