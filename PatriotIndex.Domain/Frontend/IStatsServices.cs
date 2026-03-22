using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Calculates all derivable statistics for a player from their <see cref="PlayerStatBlock"/>.
/// </summary>
public interface IPlayerStatsService
{
    /// <summary>
    /// Returns a fully populated <see cref="PlayerStatsResult"/> for the supplied stat block.
    /// Properties are null for any stat category not populated on the block.
    /// </summary>
    /// <param name="stats">The player's accumulated stat block.</param>
    PlayerStatsResult Calculate(PlayerStatBlock stats);
}

/// <summary>
/// Calculates all derivable statistics for a team from their <see cref="TeamStatBlock"/>.
/// </summary>
public interface ITeamStatsService
{
    /// <summary>
    /// Returns a fully populated <see cref="TeamStatsResult"/> for the supplied stat block.
    /// Properties are null for any stat category not populated on the block.
    /// </summary>
    /// <param name="stats">The team's accumulated stat block.</param>
    TeamStatsResult Calculate(TeamStatBlock stats);
}
