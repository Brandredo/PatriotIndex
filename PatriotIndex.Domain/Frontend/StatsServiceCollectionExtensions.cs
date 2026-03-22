using Microsoft.Extensions.DependencyInjection;

namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Registers all stats-calculation services with the DI container.
/// </summary>
public static class StatsServiceCollectionExtensions
{
    /// <summary>
    /// Adds <see cref="IPlayerStatsService"/> and <see cref="ITeamStatsService"/>
    /// as singleton services. The calculators are stateless and thread-safe,
    /// so singleton lifetime is appropriate.
    /// </summary>
    public static IServiceCollection AddStatsServices(this IServiceCollection services)
    {
        services.AddSingleton<IPlayerStatsService, PlayerStatsService>();
        services.AddSingleton<ITeamStatsService, TeamStatsService>();
        return services;
    }
}
