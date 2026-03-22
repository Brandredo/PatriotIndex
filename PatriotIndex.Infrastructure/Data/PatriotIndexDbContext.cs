using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Entities.People;
using PatriotIndex.Domain.Entities.PlayByPlay;
using PatriotIndex.Domain.Entities.Schedule;
using PatriotIndex.Domain.Entities.Stats.PlayerGame;
using PatriotIndex.Domain.Entities.Stats.PlayerSeason;
using PatriotIndex.Domain.Entities.Stats.TeamGame;
using PatriotIndex.Domain.Entities.Stats.TeamSeason;
using Coach = PatriotIndex.Domain.Entities.Organization.Coach;
using Conference = PatriotIndex.Domain.Entities.Organization.Conference;
using Division = PatriotIndex.Domain.Entities.Organization.Division;
using Game = PatriotIndex.Domain.Entities.Schedule.Game;
using Player = PatriotIndex.Domain.Entities.People.Player;
using PlayerGameStats = PatriotIndex.Domain.Entities.Stats.PlayerGame.PlayerGameStats;
using PlayerSeasonStats = PatriotIndex.Domain.Entities.Stats.PlayerSeason.PlayerSeasonStats;
using Season = PatriotIndex.Domain.Entities.Schedule.Season;
using Team = PatriotIndex.Domain.Entities.Organization.Team;
using TeamGameStats = PatriotIndex.Domain.Entities.Stats.TeamGame.TeamGameStats;
using TeamSeasonStats = PatriotIndex.Domain.Entities.Stats.TeamSeason.TeamSeasonStats;
using Venue = PatriotIndex.Domain.Entities.Organization.Venue;

namespace PatriotIndex.Infrastructure.Data;

public class PatriotIndexDbContext(DbContextOptions<PatriotIndexDbContext> options) : DbContext(options)
{
    // Organization
    public DbSet<Conference> Conferences => Set<Conference>();
    public DbSet<Division> Divisions => Set<Division>();
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Coach> Coaches => Set<Coach>();

    // People
    public DbSet<Player> Players => Set<Player>();
    public DbSet<PlayerDraft> PlayerDrafts => Set<PlayerDraft>();

    // Schedule
    public DbSet<Season> Seasons => Set<Season>();
    public DbSet<Week> Weeks => Set<Week>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameWeather> GameWeather => Set<GameWeather>();

    // Team Season Stats
    public DbSet<TeamSeasonStats> TeamSeasonStats => Set<TeamSeasonStats>();
    public DbSet<TeamSeasonPassing> TeamSeasonPassing => Set<TeamSeasonPassing>();
    public DbSet<TeamSeasonRushing> TeamSeasonRushing => Set<TeamSeasonRushing>();
    public DbSet<TeamSeasonReceiving> TeamSeasonReceiving => Set<TeamSeasonReceiving>();
    public DbSet<TeamSeasonDefense> TeamSeasonDefense => Set<TeamSeasonDefense>();
    public DbSet<TeamSeasonPunts> TeamSeasonPunts => Set<TeamSeasonPunts>();
    public DbSet<TeamSeasonKicking> TeamSeasonKicking => Set<TeamSeasonKicking>();
    public DbSet<TeamSeasonReturns> TeamSeasonReturns => Set<TeamSeasonReturns>();
    public DbSet<TeamSeasonMisc> TeamSeasonMisc => Set<TeamSeasonMisc>();

    // Player Season Stats
    public DbSet<PlayerSeasonStats> PlayerSeasonStats => Set<PlayerSeasonStats>();
    public DbSet<PlayerSeasonPassing> PlayerSeasonPassing => Set<PlayerSeasonPassing>();
    public DbSet<PlayerSeasonRushing> PlayerSeasonRushing => Set<PlayerSeasonRushing>();
    public DbSet<PlayerSeasonReceiving> PlayerSeasonReceiving => Set<PlayerSeasonReceiving>();
    public DbSet<PlayerSeasonDefense> PlayerSeasonDefense => Set<PlayerSeasonDefense>();
    public DbSet<PlayerSeasonPunts> PlayerSeasonPunts => Set<PlayerSeasonPunts>();
    public DbSet<PlayerSeasonKicking> PlayerSeasonKicking => Set<PlayerSeasonKicking>();
    public DbSet<PlayerSeasonReturns> PlayerSeasonReturns => Set<PlayerSeasonReturns>();

    // Team Game Stats
    public DbSet<TeamGameStats> TeamGameStats => Set<TeamGameStats>();
    public DbSet<TeamGamePassing> TeamGamePassing => Set<TeamGamePassing>();
    public DbSet<TeamGameRushing> TeamGameRushing => Set<TeamGameRushing>();
    public DbSet<TeamGameReceiving> TeamGameReceiving => Set<TeamGameReceiving>();
    public DbSet<TeamGameDefense> TeamGameDefense => Set<TeamGameDefense>();
    public DbSet<TeamGamePunts> TeamGamePunts => Set<TeamGamePunts>();
    public DbSet<TeamGameKicking> TeamGameKicking => Set<TeamGameKicking>();
    public DbSet<TeamGameReturns> TeamGameReturns => Set<TeamGameReturns>();
    public DbSet<TeamGameMisc> TeamGameMisc => Set<TeamGameMisc>();

    // Player Game Stats
    public DbSet<PlayerGameStats> PlayerGameStats => Set<PlayerGameStats>();
    public DbSet<PlayerGamePassing> PlayerGamePassing => Set<PlayerGamePassing>();
    public DbSet<PlayerGameRushing> PlayerGameRushing => Set<PlayerGameRushing>();
    public DbSet<PlayerGameReceiving> PlayerGameReceiving => Set<PlayerGameReceiving>();
    public DbSet<PlayerGameDefense> PlayerGameDefense => Set<PlayerGameDefense>();
    public DbSet<PlayerGamePunts> PlayerGamePunts => Set<PlayerGamePunts>();
    public DbSet<PlayerGameKicking> PlayerGameKicking => Set<PlayerGameKicking>();
    public DbSet<PlayerGameReturns> PlayerGameReturns => Set<PlayerGameReturns>();

    // Play-by-Play
    public DbSet<GamePeriod> GamePeriods => Set<GamePeriod>();
    public DbSet<GameDrive> GameDrives => Set<GameDrive>();
    public DbSet<GamePlay> GamePlays => Set<GamePlay>();
    public DbSet<PlaySituation> PlaySituations => Set<PlaySituation>();
    public DbSet<PlayPlayerStats> PlayPlayerStats => Set<PlayPlayerStats>();
    public DbSet<PlayPassStats> PlayPassStats => Set<PlayPassStats>();
    public DbSet<PlayReceiveStats> PlayReceiveStats => Set<PlayReceiveStats>();
    public DbSet<PlayRushStats> PlayRushStats => Set<PlayRushStats>();
    public DbSet<PlayDefenseStats> PlayDefenseStats => Set<PlayDefenseStats>();
    public DbSet<PlayKickStats> PlayKickStats => Set<PlayKickStats>();
    public DbSet<PlayReturnStats> PlayReturnStats => Set<PlayReturnStats>();
    
    public DbSet<SyncLog>           SyncLogs           { get; set; }
    public DbSet<AppConfig>         AppConfigs         { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PatriotIndexDbContext).Assembly);
        
        modelBuilder.Entity<AppConfig>(e =>
        {
            e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.HasIndex(x => x.Key).IsUnique();
        });
        
        modelBuilder.Entity<SyncLog>(e =>
        {
            e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.HasIndex(x => x.EntityType);
            e.HasIndex(x => x.StartedAt);
        });
    }
}
