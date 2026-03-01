using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Repository;

public class TeamsRepository(PatriotIndexDbContext ctx, ILogger<TeamsRepository> logger)
{
    public async Task<IEnumerable<Team>> GetTeamsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all teams");
        return await ctx.Teams.AsNoTracking().Where(t => t.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<Team?> GetTeamByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Getting team with id: {id}");
        return await ctx.Teams.FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Guid>> GetTeamIdsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all team ids");
        return await ctx.Teams.AsNoTracking().Where(t => t.IsActive).Select(t => t.Id).ToListAsync(cancellationToken);
    }

    // delete a team based on the team's id


    // save or update a team
    public async Task SaveOrUpdateAsync(Team team, CancellationToken ct)
    {
        await using var transaction = await ctx.Database.BeginTransactionAsync(ct);

        try
        {
            // insert venue
            if (team.VenueId != Guid.Empty && team.Venue != null) await UpsertVenueAsync(team.Venue);

            // insert team
            await UpsertTeamAsync(team);

            if (team.Colors != null) await UpsertTeamColorsAsync(team.Id, team.Colors);

            // update coaches
            foreach (var coach in team.Coaches) await UpsertCoachAsync(coach);

            // update players
            foreach (var player in team.Players) await UpsertPlayerAsync(player);

            await transaction.CommitAsync(ct);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(ct);
            Console.WriteLine(e);
            throw;
        }
    }


    private async Task UpsertTeamAsync(Team team)
    {
        var venueId = team.VenueId is { } v && v != Guid.Empty ? v : (Guid?)null;
        var divisionId = team.DivisionId is { } d && d != Guid.Empty ? d : (Guid?)null;

        logger.LogDebug("Upserting team {TeamId} ({Alias})", team.Id, team.Alias);

        await ctx.Database.ExecuteSqlRawAsync(
            @"INSERT INTO teams(
                    id,
                    name,
                    market,
                    alias,
                    sr_id,
                    founded,
                    owner,
                    general_manager,
                    president,
                    mascot,
                    venue_id,
                    division_id,
                    championships_won,
                    conference_titles,
                    division_titles,
                    playoff_appearances,
                    championship_seasons
                )
                VALUES (
                    {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9},
                    {10}, {11}, {12}, {13}, {14}, {15}, {16}
                )
                ON CONFLICT(id) DO UPDATE SET
                    name = EXCLUDED.name,
                    market = EXCLUDED.market,
                    alias = EXCLUDED.alias,
                    sr_id = EXCLUDED.sr_id,
                    founded = EXCLUDED.founded,
                    owner = EXCLUDED.owner,
                    general_manager = EXCLUDED.general_manager,
                    president = EXCLUDED.president,
                    mascot = EXCLUDED.mascot,
                    venue_id = EXCLUDED.venue_id,
                    division_id = EXCLUDED.division_id,
                    championships_won = EXCLUDED.championships_won,
                    conference_titles = EXCLUDED.conference_titles,
                    division_titles = EXCLUDED.division_titles,
                    playoff_appearances = EXCLUDED.playoff_appearances,
                    championship_seasons = EXCLUDED.championship_seasons",
            team.Id,
            team.Name,
            team.Market,
            team.Alias,
            team.SrId,
            team.Founded,
            team.Owner,
            team.GeneralManager,
            team.President,
            team.Mascot,
            venueId,
            divisionId,
            team.ChampionshipsWon,
            team.ConferenceTitles,
            team.DivisionTitles,
            team.PlayoffAppearances,
            team.ChampionshipSeasons
        );
    }

    private async Task UpsertTeamColorsAsync(Guid teamId, TeamColors? teamColors)
    {
        if (teamColors is null || string.IsNullOrWhiteSpace(teamColors.Primary)) return;

        if (teamColors?.Secondary != null)
        {
            await ctx.Database.ExecuteSqlRawAsync(
                @"INSERT INTO team_colors(id, primary_color, secondary_color) values ({0}, {1}, {2}) ON CONFLICT(id) DO UPDATE SET
                    primary_color = EXCLUDED.primary_color,
                    secondary_color = EXCLUDED.secondary_color,
                    id = EXCLUDED.id"
                , teamId, teamColors.Primary, teamColors.Secondary);
            return;
        }

        await ctx.Database.ExecuteSqlRawAsync(
            @"INSERT INTO team_colors(id, primary_color) values({0}, {1}) ON CONFLICT(id) DO UPDATE SET
                primary_color = EXCLUDED.primary_color, id = EXCLUDED.id", teamId, teamColors!.Primary);
    }

    private async Task UpsertVenueAsync(Venue venue)
    {
        await ctx.Database.ExecuteSqlRawAsync(
            @"INSERT INTO venues(id, name, city, state, country, zip, address, capacity, surface, roof_type, sr_id, lat, lng) values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}) ON CONFLICT(id) DO UPDATE SET name = EXCLUDED.name, city = EXCLUDED.city, state = EXCLUDED.state, country = EXCLUDED.country, zip = EXCLUDED.zip, address = EXCLUDED.address, capacity = EXCLUDED.capacity, surface = EXCLUDED.surface, roof_type = EXCLUDED.roof_type, sr_id = EXCLUDED.sr_id, lat = EXCLUDED.lat, lng = EXCLUDED.lng",
            venue.Id, venue.Name, venue.City, venue.State, venue.Country, venue.Zip, venue.Address, venue.Capacity,
            venue.Surface, venue.RoofType, venue.SrId, venue.Lat, venue.Lng);
    }

    private async Task UpsertCoachAsync(Coach coach)
    {
        await ctx.Database.ExecuteSqlRawAsync(
            @"INSERT INTO coaches(id, team_id, first_name, last_name, full_name, position) values ({0}, {1}, {2}, {3}, {4}, {5}) ON CONFLICT(id) DO UPDATE SET id = EXCLUDED.id, team_id = EXCLUDED.team_id, first_name = EXCLUDED.first_name, last_name = EXCLUDED.last_name, full_name = EXCLUDED.full_name, position = EXCLUDED.position",
            coach.Id, coach.TeamId, coach.FirstName, coach.LastName, coach.FullName, coach.Position);
    }

    private async Task UpsertPlayerAsync(Player player)
    {
        await ctx.Database.ExecuteSqlRawAsync(
            @"INSERT INTO players(id, team_id, first_name, last_name, name, jersey, position, height, weight, birth_date, college, rookie_year, status, experience, salary, sr_id, draft_year, draft_round, draft_pick, draft_team_id) values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}) ON CONFLICT(id) DO UPDATE SET team_id = EXCLUDED.team_id, first_name = EXCLUDED.first_name, last_name = EXCLUDED.last_name, name = EXCLUDED.name, jersey = EXCLUDED.jersey, position = EXCLUDED.position, height = EXCLUDED.height, weight = EXCLUDED.weight, birth_date = EXCLUDED.birth_date, college = EXCLUDED.college, rookie_year = EXCLUDED.rookie_year, status = EXCLUDED.status, experience = EXCLUDED.experience, salary = EXCLUDED.salary, sr_id = EXCLUDED.sr_id, draft_year = EXCLUDED.draft_year, draft_round = EXCLUDED.draft_round, draft_pick = EXCLUDED.draft_pick, draft_team_id = EXCLUDED.draft_team_id",
            player.Id, player.TeamId, player.FirstName, player.LastName, player.Name, player.Jersey, player.Position,
            player.Height, player.Weight, player.BirthDate, player.College, player.RookieYear, player.Status,
            player.Experience, player.Salary, player.SrId, player.DraftYear, player.DraftRound, player.DraftPick,
            player.DraftTeamId);
    }
}