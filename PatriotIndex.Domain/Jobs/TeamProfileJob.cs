using Microsoft.Extensions.Logging;

namespace PatriotIndex.Domain.Jobs;

public class TeamProfileJob(IHttpClientFactory httpClientFactory, ILogger<TeamProfileJob> logger)
{

    
    public async Task RunAsync(Guid teamId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Team Profile Job");
        
    }
    
}