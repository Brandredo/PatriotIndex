using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;
using PatriotIndex.Ingestion.Converters.Transformers;
using PatriotIndex.Ingestion.Persistence;

namespace PatriotIndex.Ingestion.Converters.Endpoints;

public class PbpDataConverter(IServiceProvider sp, GameTransformer gameTransformer, ILogger<PbpDataConverter> logger) : IEndpointDataConverter
{
    public string EndpointKey => "pbp";

    public async Task ConvertAsync(string jsonContent, IDbContextFactory<PatriotIndexDbContext> dbFactory, CancellationToken ct)
    {
        var scope = sp.CreateScope();
        var saver = scope.ServiceProvider.GetRequiredService<GamePbpSaver>();
        
        using var doc = JsonDocument.Parse(jsonContent);
        var nav = new JsonNavigator(doc.RootElement);

        PatriotIndex.Domain.Entities.Game game;
        try
        {
            game = gameTransformer.Transform(nav);
        }
        catch (JsonPathException ex)
        {
            logger.LogError(ex, "JSON parse failure at path {Path}", ex.JsonPath);
            throw;
        }

        await saver.SaveAsync(game, dbFactory, ct);
    }
}
