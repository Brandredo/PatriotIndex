using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Repository;
using PatriotIndex.Ingestion.Converters.Transformers;
using PatriotIndex.Ingestion.Persistence;

namespace PatriotIndex.Ingestion.Converters.Endpoints;

public class TeamProfileDataConverter(IServiceProvider sp, TeamTransformer teamTransformer, ILogger<PbpDataConverter> logger) : IEndpointDataConverter
{
    public string EndpointKey => "team_profile";

    public async Task ConvertAsync(string jsonContent, IDbContextFactory<PatriotIndexDbContext> dbFactory, CancellationToken ct)
    {
        
        var scope = sp.CreateScope();
        var saver = scope.ServiceProvider.GetRequiredService<TeamsRepository>();
        
        using var doc = JsonDocument.Parse(jsonContent);
        var nav = new JsonNavigator(doc.RootElement);

        Team team;
        try
        {
            team = teamTransformer.Transform(nav);
        }
        catch (JsonPathException ex)
        {
            logger.LogError(ex, "JSON parse failure at path {Path}", ex.JsonPath);
            throw;
        }

        await saver.SaveOrUpdateAsync(team, ct);
    }
}