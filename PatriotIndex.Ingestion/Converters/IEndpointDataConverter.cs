using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;

namespace PatriotIndex.Ingestion.Converters;

public interface IEndpointDataConverter
{
    string EndpointKey { get; }
    Task ConvertAsync(string jsonContent, IDbContextFactory<PatriotIndexDbContext> dbFactory, CancellationToken ct);
}
