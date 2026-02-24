using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;
using PatriotIndex.Ingestion.Converters;

namespace PatriotIndex.Ingestion;

public class Worker(
    ILogger<Worker> logger,
    IHttpClientFactory httpFactory,
    IDbContextFactory<PatriotIndexDbContext> dbContextFactory,
    IEnumerable<IEndpointDataConverter> converters) : BackgroundService
{
    private readonly Dictionary<string, IEndpointDataConverter> _converterMap =
        converters.ToDictionary(c => c.EndpointKey);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        using var client = httpFactory.CreateClient("MyApi");
        var response = await client.GetAsync("games/5848514c-3977-4aa3-9db0-94ed5d0ebb34/pbp.json", stoppingToken);
        var content = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync(stoppingToken);
        await _converterMap["pbp"].ConvertAsync(content, dbContextFactory, stoppingToken);

        return;
        
        
        var sema = new SemaphoreSlim(1);

        var tasks = new List<Task>();

        foreach (var t1 in _teamIds)
        {
            var endpoint = $"teams/{t1}/profile.json";

            tasks.Add(ProcessTeamAsync(endpoint, stoppingToken, sema));
        }

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Worker failed while processing team profiles.");
        }
        finally
        {
            sema.Dispose();
        }

        logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);

        async Task ProcessTeamAsync(string endpoint, CancellationToken token, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync(token);
            try
            {
                using var client = httpFactory.CreateClient("MyApi");
                var response = await client.GetAsync(endpoint, token);
                var content = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync(token);
                await _converterMap["team_profile"].ConvertAsync(content, dbContextFactory, token);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to process team profile for {Endpoint}", endpoint);
            }
            finally
            {
                await Task.Delay(3000, token);
                semaphore.Release();
            }
        }
    }

    private readonly List<string> _teamIds =
    [
        "f0e724b0-4cbf-495a-be47-013907608da9",
        "7b112545-38e6-483c-a55c-96cf6ee49cb8",
        "ad4ae08f-d808-42d5-a1e6-e9bc4e34d123",
        "768c92aa-75ff-4a43-bcc0-f2798c2e1724",
        "ce92bd47-93d5-4fe9-ada4-0fc681e6caa0",
        "d5a2eb42-8065-4174-ab79-0a6fa820e35e",
        "4254d319-1bc7-4f81-b4ab-b5e6f3402b69",
        "de760528-1dc0-416a-a978-b510d20692ff",
        "1f6dcffb-9823-43cd-9ff4-e7a8466749b5",
        "6680d28d-d4d2-49f6-aace-5292d3ec02c2",
        "82cf9565-6eb9-4f01-bdbd-5aa0d472fcd9",
        "22052ff7-c065-42ee-bc8f-c4691c50e624",
        "e627eec7-bbae-4fa4-8e73-8e1d6bc5c060",
        "4809ecb0-abd3-451d-9c4a-92a90b83ca06",
        "386bdbf9-9eea-4869-bb9a-274b0bc66e80",
        "e6aa13a4-0055-48a9-bc41-be28dc106929",
        "04aa1c9d-66da-489d-b16a-1dee3f2eec4d",
        "f7ddd7fa-0bae-4f90-bc8e-669e4d6cf2de",
        "5fee86ae-74ab-4bdd-8416-42a9dd9964f3",
        "c5a59daa-53a7-4de0-851f-fb12be893e9e",
        "a20471b4-a8d9-40c7-95ad-90cc30e46932",
        "f14bf5cc-9a82-4a38-bc15-d39f75ed5314",
        "97354895-8c77-4fd4-a860-32e62ea7382a",
        "7d4fcc64-9cb5-4d1b-8e75-8a906d1e1576",
        "2eff2a03-54d4-46ba-890e-2bc3925548f3",
        "ebd87119-b331-4469-9ea6-d51fe3ce2f1c",
        "0d855753-ea21-4953-89f9-0e20aff9eb73",
        "3d08af9e-c767-4f88-a7dc-b920c6d2b4a8",
        "cb2f9f1f-ac67-424e-9e72-1475cb0ed398",
        "23ed0bf0-f058-11ee-9989-93cc4251593a",
        "82d2d380-3834-4938-835f-aec541e5ece7",
        "d26a1ca5-722d-4274-8f97-c92e49c96315",
        "33405046-04ee-4058-a950-d606f8c30852"
    ];
}



