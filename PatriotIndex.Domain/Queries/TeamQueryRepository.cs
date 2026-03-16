using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Context;
using PatriotIndex.Domain.DTOs;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Interfaces;

namespace PatriotIndex.Domain.Queries;

public class TeamQueryRepository(PatriotIndexDbContext db, ICacheService cache) : ITeamRepository
{
    public async Task<IReadOnlyList<TeamSummaryDto>> GetAllAsync()
    {
        var teams = await db.Teams
            .AsNoTracking()
            .Include(t => t.Colors)
            .Include(t => t.Division!.Conference)
            .Where(t => t.IsActive)
            .OrderBy(t => t.Market)
            .ToListAsync();

        return teams.Select(ToSummary).ToList();
    }
    
    public async Task<IReadOnlyList<TeamSummaryWithRosterDto>> GetTeamsAndPlayers()
    {
        var teams = await db.Teams
            .AsNoTracking()
            .Include(t => t.Players)
            .Where(t => t.IsActive)
            .OrderBy(t => t.Market)
            .ToListAsync();

        return teams.Select(ToSummaryWithRoster).ToList();
    }

    public async Task<TeamDetailDto?> GetByIdAsync(Guid id)
    {
        var t = await db.Teams
            .AsNoTracking()
            .Include(t => t.Colors)
            .Include(t => t.Division!.Conference)
            .Include(t => t.Venue)
            .Include(t => t.Coaches)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (t is null) return null;

        return new TeamDetailDto(
            t.Id, t.Name, t.Market, t.Alias, t.SrId,
            (int?)t.Founded,
            t.Owner, t.GeneralManager, t.President, t.Mascot,
            t.Colors,
            t.ChampionshipsWon, t.ConferenceTitles, t.DivisionTitles, t.PlayoffAppearances,
            t.Venue is null ? null : new VenueDto(
                t.Venue.Id, t.Venue.Name, t.Venue.City, t.Venue.State, t.Venue.Country,
                t.Venue.Capacity, t.Venue.Surface, t.Venue.RoofType, t.Venue.Lat, t.Venue.Lng),
            t.Division is null ? null : new DivisionSummaryDto(
                t.Division.Id, t.Division.Name, t.Division.Alias,
                new ConferenceSummaryDto(
                    t.Division.Conference!.Id, t.Division.Conference.Name, t.Division.Conference.Alias)),
            t.Coaches.Select(c => new CoachDto(c.Id, c.FullName, c.Position)).ToList());
    }

    public async Task<IReadOnlyList<PlayerRosterDto>> GetRosterAsync(Guid teamId)
    {
        var players = await db.Players
            .AsNoTracking()
            .Where(p => p.TeamId == teamId)
            .OrderBy(p => p.Position.ToString())
            .ThenBy(p => p.Name)
            .ToListAsync();

        return players
            .Select(p => new PlayerRosterDto(p.Id, p.Name, p.FirstName, p.LastName,
                p.Jersey, p.Position?.ToString(), p.Status, p.Experience))
            .ToList();
    }

    public async Task<TeamSeasonSummaryDto?> GetSeasonStatsAsync(Guid teamId, int seasonYear, string seasonType)
    {
        var stats = await db.TeamSeasonStats
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.TeamId == teamId
                                      && s.SeasonYear == seasonYear
                                      && s.SeasonType == seasonType);

        if (stats is null) return null;

        var games = await db.Games
            .AsNoTracking()
            .Where(g => (g.HomeTeamId == teamId || g.AwayTeamId == teamId)
                        && g.SeasonYear == seasonYear
                        && g.SeasonType == seasonType
                        && g.Status == "closed"
                        && g.HomePoints != null)
            .ToListAsync();

        int gp = stats.GamesPlayed > 0 ? stats.GamesPlayed : Math.Max(games.Count, 1);

        int totalScored  = games.Sum(g => g.HomeTeamId == teamId ? g.HomePoints!.Value : g.AwayPoints!.Value);
        int totalAllowed = games.Sum(g => g.HomeTeamId == teamId ? g.AwayPoints!.Value : g.HomePoints!.Value);

        var r = stats.Record;
        var statBlock = new StatBlockDto(
            r.Passing?.Attempts ?? 0,
            r.Passing?.Completions ?? 0,
            r.Passing?.Yards ?? 0,
            r.Passing?.Touchdowns ?? 0,
            r.Passing?.Interceptions ?? 0,
            (double)(r.Passing?.Rating ?? 0),
            r.Passing?.Sacks ?? 0,
            r.Passing?.SackYards ?? 0,
            r.Rushing?.Attempts ?? 0,
            r.Rushing?.Yards ?? 0,
            r.Rushing?.Touchdowns ?? 0,
            r.Rushing?.AvgYards ?? 0,
            r.Rushing?.Longest ?? 0,
            r.Receiving?.Targets ?? 0,
            r.Receiving?.Receptions ?? 0,
            r.Receiving?.Yards ?? 0,
            r.Receiving?.Touchdowns ?? 0,
            (double)(r.Receiving?.AvgYards ?? 0),
            r.Receiving?.Longest ?? 0,
            r.Defense?.Tackles ?? 0,
            r.Defense?.Assists ?? 0,
            (double)(r.Defense?.Sacks ?? 0),
            r.Defense?.Interceptions ?? 0,
            r.Defense?.ForcedFumbles ?? 0,
            r.Defense?.PassesDefended ?? 0,
            r.Defense?.QbHits ?? 0,
            r.FieldGoals?.Attempts ?? 0,
            r.FieldGoals?.Made ?? 0,
            r.FieldGoals?.Longest ?? 0,
            r.ExtraPoints?.Kicks?.Attempts ?? 0,
            r.ExtraPoints?.Kicks?.Made ?? 0,
            r.Punts?.Attempts ?? 0,
            r.Punts?.Yards ?? 0,
            (double)(r.Punts?.AvgYards ?? 0));

        return new TeamSeasonSummaryDto(
            gp,
            games.Count > 0 ? Math.Round((double)totalScored  / games.Count, 1) : 0,
            totalScored,
            games.Count > 0 ? Math.Round((double)totalAllowed / games.Count, 1) : 0,
            totalAllowed,
            MapRecord(stats.Record,    gp),
            MapRecord(stats.Opponents, gp),
            statBlock);
    }

    private static SeasonStatsRecordDto MapRecord(TeamStatBlock b, int gp) => new(
        gp,
        b.Touchdowns == null ? null : new SeasonTouchdownsDto(
            b.Touchdowns.Pass, b.Touchdowns.Rush, b.Touchdowns.TotalReturn,
            b.Touchdowns.Total, b.Touchdowns.FumbleReturn, b.Touchdowns.IntReturn,
            b.Touchdowns.KickReturn, b.Touchdowns.PuntReturn, b.Touchdowns.Other),
        b.Rushing == null ? null : new SeasonRushingDto(
            b.Rushing.AvgYards, b.Rushing.Attempts, b.Rushing.Touchdowns,
            b.Rushing.TacklesForLoss, b.Rushing.TacklesForLossYards,
            b.Rushing.Yards, b.Rushing.Longest, b.Rushing.LongestTouchdown,
            b.Rushing.RedzoneAttempts, b.Rushing.BrokenTackles, b.Rushing.KneelDowns,
            b.Rushing.Scrambles, b.Rushing.YardsAfterContact, b.Rushing.FirstDowns),
        b.Passing == null ? null : new SeasonPassingDto(
            b.Passing.Attempts, b.Passing.Completions, b.Passing.CmpPct,
            b.Passing.Interceptions, b.Passing.SackYards, b.Passing.Rating,
            b.Passing.Touchdowns, b.Passing.AvgYards, b.Passing.Sacks,
            b.Passing.Longest, b.Passing.LongestTouchdown, b.Passing.AirYards,
            b.Passing.RedzoneAttempts, b.Passing.NetYards, b.Passing.Yards,
            b.Passing.GrossYards, b.Passing.IntTouchdowns, b.Passing.ThrowAways,
            b.Passing.PoorThrows, b.Passing.DefendedPasses, b.Passing.DroppedPasses,
            b.Passing.Spikes, b.Passing.Blitzes, b.Passing.Hurries,
            b.Passing.Knockdowns, b.Passing.PocketTime, b.Passing.BattedPasses,
            b.Passing.OnTargetThrows, b.Passing.FirstDowns, b.Passing.AvgPocketTime),
        b.Receiving == null ? null : new SeasonReceivingDto(
            b.Receiving.Targets, b.Receiving.Receptions, b.Receiving.AvgYards,
            b.Receiving.Yards, b.Receiving.Touchdowns, b.Receiving.YardsAfterCatch,
            b.Receiving.Longest, b.Receiving.LongestTouchdown, b.Receiving.RedzoneTargets,
            b.Receiving.AirYards, b.Receiving.BrokenTackles, b.Receiving.DroppedPasses,
            b.Receiving.CatchablePasses, b.Receiving.YardsAfterContact, b.Receiving.FirstDowns),
        b.Defense == null ? null : new SeasonDefenseDto(
            b.Defense.Tackles, b.Defense.Assists, b.Defense.Combined,
            b.Defense.Sacks, b.Defense.SackYards, b.Defense.Interceptions,
            b.Defense.PassesDefended, b.Defense.ForcedFumbles, b.Defense.FumbleRecoveries,
            b.Defense.QbHits, b.Defense.Tloss, b.Defense.TlossYards,
            b.Defense.Safeties, b.Defense.SpTackles, b.Defense.SpAssists,
            b.Defense.SpForcedFumbles, b.Defense.SpFumbleRecoveries, b.Defense.SpBlocks,
            b.Defense.MiscTackles, b.Defense.MiscAssists, b.Defense.MiscForcedFumbles,
            b.Defense.MiscFumbleRecoveries, b.Defense.SpOwnFumbleRecoveries,
            b.Defense.SpOppFumbleRecoveries, b.Defense.ThreeAndOutsForced,
            b.Defense.FourthDownStops, b.Defense.DefTargets, b.Defense.DefComps,
            b.Defense.Blitzes, b.Defense.Hurries, b.Defense.Knockdowns,
            b.Defense.MissedTackles, b.Defense.BattedPasses),
        b.FieldGoals == null ? null : new SeasonFieldGoalsDto(
            b.FieldGoals.Attempts, b.FieldGoals.Made, b.FieldGoals.Blocked,
            b.FieldGoals.Yards, b.FieldGoals.AvgYards, b.FieldGoals.Longest,
            b.FieldGoals.Missed, b.FieldGoals.Pct,
            b.FieldGoals.Attempts19, b.FieldGoals.Attempts29, b.FieldGoals.Attempts39,
            b.FieldGoals.Attempts49, b.FieldGoals.Attempts50,
            b.FieldGoals.Made19, b.FieldGoals.Made29, b.FieldGoals.Made39,
            b.FieldGoals.Made49, b.FieldGoals.Made50),
        b.Kickoffs == null ? null : new SeasonKickoffsDto(
            b.Kickoffs.Kickoffs, b.Kickoffs.Endzone, b.Kickoffs.Inside20,
            b.Kickoffs.ReturnYards, b.Kickoffs.Returned, b.Kickoffs.Touchbacks,
            b.Kickoffs.Yards, b.Kickoffs.OutOfBounds, b.Kickoffs.OnsideAttempts,
            b.Kickoffs.OnsideSuccesses, b.Kickoffs.SquibKicks),
        b.KickReturns == null ? null : new SeasonKickReturnsDto(
            b.KickReturns.AvgYards, b.KickReturns.Yards, b.KickReturns.Longest,
            b.KickReturns.Touchdowns, b.KickReturns.LongestTouchdown,
            b.KickReturns.Faircatches, b.KickReturns.Returns),
        b.Punts == null ? null : new SeasonPuntsDto(
            b.Punts.Attempts, b.Punts.Yards, b.Punts.NetYards, b.Punts.Blocked,
            b.Punts.Touchbacks, b.Punts.Inside20, b.Punts.ReturnYards,
            b.Punts.AvgNetYards, b.Punts.AvgYards, b.Punts.Longest,
            b.Punts.HangTime, b.Punts.AvgHangTime),
        b.PuntReturns == null ? null : new SeasonPuntReturnsDto(
            b.PuntReturns.AvgYards, b.PuntReturns.Returns, b.PuntReturns.Yards,
            b.PuntReturns.Longest, b.PuntReturns.Touchdowns,
            b.PuntReturns.LongestTouchdown, b.PuntReturns.Faircatches),
        b.Interceptions == null ? null : new SeasonInterceptionsDto(
            b.Interceptions.ReturnYards, b.Interceptions.Returned, b.Interceptions.Interceptions),
        b.IntReturns == null ? null : new SeasonIntReturnsDto(
            b.IntReturns.AvgYards, b.IntReturns.Yards, b.IntReturns.Longest,
            b.IntReturns.Touchdowns, b.IntReturns.LongestTouchdown, b.IntReturns.Returns),
        b.Fumbles == null ? null : new SeasonFumblesDto(
            b.Fumbles.Fumbles, b.Fumbles.LostFumbles, b.Fumbles.OwnRec,
            b.Fumbles.OwnRecYards, b.Fumbles.OppRec, b.Fumbles.OppRecYards,
            b.Fumbles.OutOfBounds, b.Fumbles.ForcedFumbles,
            b.Fumbles.OwnRecTds, b.Fumbles.OppRecTds, b.Fumbles.EzRecTds),
        b.FirstDowns == null ? null : new SeasonFirstDownsDto(
            b.FirstDowns.Pass, b.FirstDowns.Penalty, b.FirstDowns.Rush, b.FirstDowns.Total),
        b.Penalties == null ? null : new SeasonPenaltiesDto(
            b.Penalties.Penalties, b.Penalties.Yards, b.Penalties.FirstDowns),
        b.MiscReturns == null ? null : new SeasonMiscReturnsDto(
            b.MiscReturns.Yards, b.MiscReturns.Touchdowns, b.MiscReturns.LongestTouchdown,
            b.MiscReturns.BlkFgTouchdowns, b.MiscReturns.BlkPuntTouchdowns,
            b.MiscReturns.FgReturnTouchdowns, b.MiscReturns.EzRecTouchdowns, b.MiscReturns.Returns),
        b.ExtraPoints == null ? null : new SeasonTeamExtraPointsDto(
            b.ExtraPoints.Kicks == null ? null : new SeasonEpKicksDto(
                b.ExtraPoints.Kicks.Attempts, b.ExtraPoints.Kicks.Blocked,
                b.ExtraPoints.Kicks.Made, b.ExtraPoints.Kicks.Pct),
            b.ExtraPoints.Conversions == null ? null : new SeasonEpConversionsDto(
                b.ExtraPoints.Conversions.PassAttempts, b.ExtraPoints.Conversions.PassSuccesses,
                b.ExtraPoints.Conversions.RushAttempts, b.ExtraPoints.Conversions.RushSuccesses,
                b.ExtraPoints.Conversions.DefenseAttempts, b.ExtraPoints.Conversions.DefenseSuccesses,
                b.ExtraPoints.Conversions.TurnoverSuccesses)),
        b.Efficiency == null ? null : new SeasonEfficiencyDto(
            b.Efficiency.Goaltogo == null ? null : new SeasonEfficiencyBlockDto(
                b.Efficiency.Goaltogo.Attempts, b.Efficiency.Goaltogo.Successes, b.Efficiency.Goaltogo.Pct),
            b.Efficiency.Redzone == null ? null : new SeasonEfficiencyBlockDto(
                b.Efficiency.Redzone.Attempts, b.Efficiency.Redzone.Successes, b.Efficiency.Redzone.Pct),
            b.Efficiency.Thirddown == null ? null : new SeasonEfficiencyBlockDto(
                b.Efficiency.Thirddown.Attempts, b.Efficiency.Thirddown.Successes, b.Efficiency.Thirddown.Pct),
            b.Efficiency.Fourthdown == null ? null : new SeasonEfficiencyBlockDto(
                b.Efficiency.Fourthdown.Attempts, b.Efficiency.Fourthdown.Successes, b.Efficiency.Fourthdown.Pct))
    );

    public async Task<IReadOnlyList<TeamPlayerStatsDto>> GetTeamPlayerStatsAsync(
        Guid teamId, int seasonYear, string seasonType)
    {
        var stats = await db.PlayerSeasonStats
            .AsNoTracking()
            .Include(s => s.Player)
            .Where(s => s.TeamId == teamId
                        && s.SeasonYear == seasonYear
                        && s.SeasonType == seasonType)
            .OrderBy(s => s.Player!.Position.ToString())
            .ThenBy(s => s.Player!.Name)
            .ToListAsync();

        return stats.Select(s => new TeamPlayerStatsDto(
            s.PlayerId,
            s.Player?.Name,
            s.Player?.Jersey,
            s.Player?.Position?.ToString(),
            s.GamesPlayed,
            s.GamesStarted,
            PlayerQueryRepository.MapStatBlock(s.Stats)
        )).ToList();
    }

    public async Task<IReadOnlyList<TeamGameLogDto>> GetTeamGameLogAsync(
        Guid teamId, int? seasonYear, string? seasonType)
    {
        var query = db.TeamGameStats
            .AsNoTracking()
            .Include(tgs => tgs.Game!.HomeTeam)
            .Include(tgs => tgs.Game!.AwayTeam)
            .Where(tgs => tgs.TeamId == teamId);

        if (seasonYear.HasValue)
            query = query.Where(tgs => tgs.Game!.SeasonYear == seasonYear);
        if (!string.IsNullOrWhiteSpace(seasonType))
            query = query.Where(tgs => tgs.Game!.SeasonType == seasonType);

        var results = await query
            .OrderByDescending(tgs => tgs.Game!.Scheduled)
            .Take(25)
            .ToListAsync();

        return results.Select(tgs =>
        {
            var g      = tgs.Game!;
            var isHome = tgs.IsHome;
            var opponent = isHome
                ? g.AwayTeam?.Alias ?? "?"
                : g.HomeTeam?.Alias ?? "?";
            var teamPts = isHome ? g.HomePoints : g.AwayPoints;
            var oppPts  = isHome ? g.AwayPoints  : g.HomePoints;
            var result  = teamPts.HasValue && oppPts.HasValue
                ? (teamPts > oppPts ? "W" : teamPts < oppPts ? "L" : "T")
                : null;

            return new TeamGameLogDto(
                g.Id,
                g.Scheduled,
                opponent,
                isHome,
                teamPts,
                oppPts,
                result,
                tgs.Stats.Summary?.PossessionTime,
                tgs.Stats.Summary?.TotalYards,
                tgs.Stats.Summary?.Turnovers,
                tgs.Stats.Summary?.Penalties,
                tgs.Stats.Summary?.RushPlays,
                tgs.Stats.Summary?.PlayCount,
                tgs.Stats.Summary?.AvgGain);
        }).ToList();
    }

    public async Task<PlayCallStatsDto> GetPlayCallStatsAsync(Guid teamId, int season, string seasonType)
    {
        var plays = await db.Plays
            .AsNoTracking()
            .Where(p => p.Drive.OffensiveTeamId == teamId
                        && p.Game.SeasonYear == season
                        && p.Game.SeasonType == seasonType
                        && (p.PlayType == "pass" || p.PlayType == "rush")
                        && p.Statistics.All(s => s.Nullified != true) // ← exclude nullified
                        && !(p.PlayType == "pass"
                             && p.Statistics.Any(s => s.Sack > 0)))          // ← exclude sacks (pass plays only)
            .Select(p => new {
                p.PlayType,
                p.PlayAction,
                p.ScreenPass,
                p.RunPassOption,
                IsScramble = p.Statistics.Any(s => s.Scramble > 0),
                IsKneel    = p.Statistics.Any(s => s.KneelDown > 0),
            })
            .ToListAsync();

        var gamesPlayed = await db.Games
            .AsNoTracking()
            .CountAsync(g => g.SeasonYear == season
                          && g.SeasonType == seasonType
                          && (g.HomeTeamId == teamId || g.AwayTeamId == teamId));

        int rushTotal = 0, rushScramble = 0, rushKneel = 0, rushRpo = 0, rushStd = 0;
        int passTotal = 0, passAction = 0, passScreen = 0, passRpo = 0, passStd = 0;

        foreach (var p in plays)
        {
            if (p.PlayType == "rush")
            {
                rushTotal++;
                if (p.IsKneel)                   rushKneel++;
                else if (p.IsScramble)            rushScramble++;
                else if (p.RunPassOption == true) rushRpo++;
                else                              rushStd++;
            }
            else
            {
                passTotal++;
                if (p.PlayAction == true)          passAction++;
                else if (p.ScreenPass == true)     passScreen++;
                else if (p.RunPassOption == true)  passRpo++;
                else                               passStd++;
            }
        }

        return new PlayCallStatsDto(
            gamesPlayed,
            rushTotal + passTotal,
            new PlayCallRushDto(rushTotal, rushScramble, rushKneel, rushRpo, rushStd),
            new PlayCallPassDto(passTotal, passAction, passScreen, passRpo, passStd));
    }

    public async Task<QuarterScoringDto> GetQuarterScoringAsync(Guid teamId, int season, string seasonType)
    {
        var rows = await db.Periods
            .AsNoTracking()
            .Where(p => p.Game!.SeasonYear == season
                     && p.Game.SeasonType == seasonType
                     && (p.Game.HomeTeamId == teamId || p.Game.AwayTeamId == teamId))
            .Select(p => new
            {
                p.GameId,
                p.Number,
                IsHome = p.Game!.HomeTeamId == teamId,
                p.HomeScore,
                p.AwayScore,
            })
            .ToListAsync();

        var gamesPlayed = rows.Select(r => r.GameId).Distinct().Count();

        if (gamesPlayed == 0)
            return new QuarterScoringDto(0, []);

        // Bucket period numbers: 1-4 = regular quarters, 5+ = OT (use quarter=5)
        var grouped = rows
            .GroupBy(r => r.Number <= 4 ? r.Number : 5)
            .OrderBy(g => g.Key)
            .Select(g =>
            {
                double teamTotal = g.Sum(r => r.IsHome ? r.HomeScore : r.AwayScore);
                double oppTotal  = g.Sum(r => r.IsHome ? r.AwayScore : r.HomeScore);
                return new QuarterScoreDto(
                    g.Key,
                    Math.Round(teamTotal / gamesPlayed, 1),
                    Math.Round(oppTotal  / gamesPlayed, 1));
            })
            .ToList();

        return new QuarterScoringDto(gamesPlayed, grouped);
    }

    // ── Stats page ────────────────────────────────────────────────────────

    public async Task<PagedResultDto<TeamStatsSummaryDto>> GetAllTeamsStatsAsync(
        int season, string seasonType,
        string? cursor, int limit, CancellationToken ct = default)
    {
        var cacheKey = $"stats:teams:{season}:{seasonType}";
        var full = await cache.GetOrSetAsync(
            cacheKey,
            () => FetchAllTeamsFromDb(season, seasonType),
            season < DateTime.UtcNow.Year ? TimeSpan.FromHours(24) : TimeSpan.FromMinutes(30),
            ct);

        return PaginateTeams(full ?? [], cursor, limit);
    }

    private async Task<TeamStatsSummaryDto[]> FetchAllTeamsFromDb(int season, string seasonType)
    {
        // 1. All season stats
        var allStats = await db.TeamSeasonStats
            .AsNoTracking()
            .Include(s => s.Team!.Colors)
            .Where(s => s.SeasonYear == season && s.SeasonType == seasonType)
            .ToListAsync();

        if (allStats.Count == 0) return [];

        var teamIds = allStats.Select(s => s.TeamId).ToHashSet();

        // 2. Closed games for scoring
        var games = await db.Games
            .AsNoTracking()
            .Where(g => g.SeasonYear == season && g.SeasonType == seasonType
                        && g.Status == "closed" && g.HomePoints != null)
            .ToListAsync();

        // 3. Possession time from game stats
        var gameLogs = await db.TeamGameStats
            .AsNoTracking()
            .Include(tgs => tgs.Game)
            .Where(tgs => teamIds.Contains(tgs.TeamId)
                          && tgs.Game!.SeasonYear == season
                          && tgs.Game.SeasonType == seasonType
                          && tgs.Game.Status == "closed")
            .Select(tgs => new { tgs.TeamId, tgs.Stats.Summary!.PossessionTime })
            .ToListAsync();

        // 4. Drive efficiency
        var drives = await db.Drives
            .AsNoTracking()
            .Include(d => d.Game)
            .Where(d => d.OffensiveTeamId != null
                        && teamIds.Contains(d.OffensiveTeamId!.Value)
                        && d.Game!.SeasonYear == season
                        && d.Game.SeasonType == seasonType
                        && d.Game.Status == "closed")
            .Select(d => new
            {
                TeamId        = d.OffensiveTeamId!.Value,
                d.NetYards,
                d.PlayCount,
                d.OffensivePoints,
            })
            .ToListAsync();

        // Group by team in memory
        var scoresByTeam = games
            .SelectMany(g => new[]
            {
                new { TeamId = g.HomeTeamId, Points = g.HomePoints!.Value, OppPoints = g.AwayPoints!.Value },
                new { TeamId = g.AwayTeamId, Points = g.AwayPoints!.Value, OppPoints = g.HomePoints!.Value }
            })
            .Where(x => x.TeamId.HasValue && teamIds.Contains(x.TeamId.Value))
            .GroupBy(x => x.TeamId!.Value)
            .ToDictionary(g => g.Key, g => (
                TotalPts:    g.Sum(x => x.Points),
                TotalOpp:    g.Sum(x => x.OppPoints),
                GameCount:   g.Count()
            ));

        var possessionByTeam = gameLogs
            .GroupBy(x => x.TeamId)
            .ToDictionary(g => g.Key, g =>
            {
                var totalSecs = g.Sum(x => ParsePossessionSeconds(x.PossessionTime));
                return g.Count() > 0 ? totalSecs / g.Count() : 0.0;
            });

        var drivesByTeam = drives
            .GroupBy(x => x.TeamId)
            .ToDictionary(g => g.Key, g => (
                Count:  g.Count(),
                Yards:  g.Sum(x => x.NetYards ?? 0),
                Plays:  g.Sum(x => x.PlayCount ?? 0),
                Points: g.Sum(x => x.OffensivePoints ?? 0)
            ));

        return allStats
            .Select(s =>
            {
                var t   = s.Team!;
                var r   = s.Record;
                var opp = s.Opponents;
                int gp  = Math.Max(s.GamesPlayed, 1);

                scoresByTeam.TryGetValue(s.TeamId, out var sc);
                possessionByTeam.TryGetValue(s.TeamId, out var topAvg);
                drivesByTeam.TryGetValue(s.TeamId, out var drv);

                int drvCount = Math.Max(drv.Count, 1);

                // Turnovers: takeaways - giveaways
                int takeaways = (r.Defense?.Interceptions ?? 0) + (r.Fumbles?.OppRec ?? 0);
                int giveaways = (r.Passing?.Interceptions ?? 0) + (r.Fumbles?.LostFumbles ?? 0);

                // Pressure rate: (def hurries + sacks + knockdowns) / opp pass att
                int pressures = (r.Defense?.Hurries ?? 0)
                              + (int)(r.Defense?.Sacks ?? 0)
                              + (r.Defense?.Knockdowns ?? 0);
                int oppPassAtt = opp.Passing?.Attempts ?? 1;

                return new TeamStatsSummaryDto(
                    t.Id,
                    t.Name,
                    t.Market,
                    t.Alias,
                    t.Colors,
                    s.GamesPlayed,
                    // Core scoring
                    sc.GameCount > 0 ? Math.Round((decimal)sc.TotalPts / sc.GameCount, 1) : 0m,
                    sc.GameCount > 0 ? Math.Round((decimal)sc.TotalOpp / sc.GameCount, 1) : 0m,
                    // Core yards
                    Math.Round((decimal)((r.Passing?.Yards ?? 0) + (r.Rushing?.Yards ?? 0)) / gp, 1),
                    Math.Round((decimal)(r.Passing?.Yards ?? 0) / gp, 1),
                    Math.Round((decimal)(r.Rushing?.Yards ?? 0) / gp, 1),
                    // Core efficiency
                    takeaways - giveaways,
                    r.Efficiency?.Thirddown?.Pct ?? 0m,
                    r.Efficiency?.Redzone?.Pct   ?? 0m,
                    r.Efficiency?.Goaltogo?.Pct  ?? 0m,
                    // Core misc
                    Math.Round((decimal)(r.Penalties?.Penalties ?? 0) / gp, 1),
                    Math.Round((decimal)(r.Penalties?.Yards ?? 0) / gp, 1),
                    Math.Round((decimal)topAvg, 0),
                    // Advanced drives
                    Math.Round((decimal)drv.Points / drvCount, 2),
                    Math.Round((decimal)drv.Yards  / drvCount, 1),
                    Math.Round((decimal)drv.Plays  / drvCount, 1),
                    // Advanced defense (confirmed populated)
                    r.Defense?.ThreeAndOutsForced ?? 0,
                    r.Defense?.MissedTackles      ?? 0,
                    r.Defense?.Blitzes            ?? 0,
                    oppPassAtt > 0 ? Math.Round((decimal)pressures / oppPassAtt * 100, 1) : 0m
                );
            })
            .OrderByDescending(x => x.PtsPerGame)
            .ToArray();
    }

    private static double ParsePossessionSeconds(string? time)
    {
        if (string.IsNullOrWhiteSpace(time)) return 0;
        var parts = time.Split(':');
        return parts.Length switch
        {
            2 when int.TryParse(parts[0], out var m) && int.TryParse(parts[1], out var s)
                => m * 60 + s,
            3 when int.TryParse(parts[0], out var h) && int.TryParse(parts[1], out var m2) && int.TryParse(parts[2], out var s2)
                => h * 3600 + m2 * 60 + s2,
            _ => 0
        };
    }

    private static PagedResultDto<TeamStatsSummaryDto> PaginateTeams(
        TeamStatsSummaryDto[] all, string? cursor, int limit)
    {
        var start = cursor is null ? 0
            : Array.FindIndex(all, x => x.TeamId.ToString() == cursor) + 1;
        if (start < 0) start = 0;
        var page = all.Skip(start).Take(limit).ToArray();
        var nextCursor = page.Length == limit && start + limit < all.Length
            ? page.Last().TeamId.ToString() : null;
        return new PagedResultDto<TeamStatsSummaryDto>(page, nextCursor, all.Length);
    }

    private static TeamSummaryDto ToSummary(Team t) => new(
        t.Id, t.Name, t.Market, t.Alias,
        t.Colors,
        t.Division is null ? null : new DivisionSummaryDto(
            t.Division.Id, t.Division.Name, t.Division.Alias,
            new ConferenceSummaryDto(
                t.Division.Conference!.Id, t.Division.Conference.Name, t.Division.Conference.Alias)));
    
    private static TeamSummaryWithRosterDto ToSummaryWithRoster(Team t) => new(
        t.Id, t.Name, t.Market, t.Alias,
        t.Players.Select(p => new PlayerMinDto(p.Id, p.Name)).ToList());
}
