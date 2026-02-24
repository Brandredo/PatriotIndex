using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;

namespace PatriotIndex.Ingestion.Converters.Endpoints;

public class BoxscoreDataConverter : IEndpointDataConverter
{
    public string EndpointKey => "boxscore";

    public Task ConvertAsync(string jsonContent, IDbContextFactory<PatriotIndexDbContext> dbFactory, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
