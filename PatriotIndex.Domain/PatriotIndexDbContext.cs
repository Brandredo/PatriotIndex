using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain;

public class PatriotIndexDbContext : DbContext
{
    
    public PatriotIndexDbContext(DbContextOptions<PatriotIndexDbContext> options) : base(options)
    {
    }
    
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<CoinToss> CoinTosses { get; set; }
    public DbSet<Conference> Conferences { get; set; }
    public DbSet<Division> Divisions { get; set; }
    public DbSet<Drive> Drives { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<PbpDriveEvent> PbpDriveEvents { get; set; }
    public DbSet<PbpEventStatistics> PbpEventStatistics { get; set; }
    public DbSet<Period> Periods { get; set; }  
    public DbSet<Play> Plays { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerGameStats> PlayerGameStats { get; set; }
    public DbSet<PlayerSeasonStats> PlayerSeasonStats { get; set; }
    public DbSet<PlayStat> PlayStats { get; set; }
    public DbSet<SyncLog> SyncLogs { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamColors> TeamColors { get; set; }
    public DbSet<TeamSeasonStats> TeamSeasonStats { get; set; }
    public DbSet<Venue> Venues { get; set; }
    
    
    
    
    
    
}