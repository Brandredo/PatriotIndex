using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;

namespace PatriotIndex.Ingestion.Converters.Endpoints;

public class ScheduleDataConverter : IEndpointDataConverter
{
    public string EndpointKey => "schedule";

    public Task ConvertAsync(string jsonContent, IDbContextFactory<PatriotIndexDbContext> dbFactory, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
