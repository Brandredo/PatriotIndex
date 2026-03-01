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
    public DbSet<DriveEvent> PbpDriveEvents { get; set; }
    public DbSet<PbpEventStatistics> PbpEventStatistics { get; set; }
    public DbSet<Period> Periods { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerGameStats> PlayerGameStats { get; set; }
    public DbSet<PlayerSeasonStats> PlayerSeasonStats { get; set; }
    public DbSet<SyncLog> SyncLogs { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamSeasonStats> TeamSeasonStats { get; set; }
    public DbSet<Venue> Venues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Conference ────────────────────────────────────────────────
        modelBuilder.Entity<Conference>(e => { e.HasIndex(x => x.Alias).IsUnique(); });

        // ── Division ──────────────────────────────────────────────────
        modelBuilder.Entity<Division>(e =>
        {
            e.HasIndex(x => x.ConferenceId);

            e.HasOne(x => x.Conference)
                .WithMany(x => x.Divisions)
                .HasForeignKey(x => x.ConferenceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Venue ─────────────────────────────────────────────────────
        modelBuilder.Entity<Venue>(e =>
        {
            // Nullable unique — add .HasFilter("[SrId] IS NOT NULL") for SQL Server
            e.HasIndex(x => x.SrId).IsUnique();
        });

        // ── Team ──────────────────────────────────────────────────────
        modelBuilder.Entity<Team>(e =>
        {
            e.HasIndex(x => x.Alias).IsUnique();
            e.HasIndex(x => x.SrId).IsUnique();
            e.HasIndex(x => x.VenueId);
            e.HasIndex(x => x.DivisionId);

            e.HasOne(x => x.Division)
                .WithMany(x => x.Teams)
                .HasForeignKey(x => x.DivisionId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Venue)
                .WithMany()
                .HasForeignKey(x => x.VenueId)
                .OnDelete(DeleteBehavior.SetNull);

            // TeamColors has no PK — configured as an owned entity stored in a separate table.
            e.OwnsOne(x => x.Colors, colors =>
            {
                colors.ToTable("team_colors");
                colors.WithOwner().HasForeignKey("TeamId");

                colors.Property(x => x.Primary).HasColumnName("primary_color");
                colors.Property(x => x.Secondary).HasColumnName("secondary_color");
            });
        });

        // ── Coach ─────────────────────────────────────────────────────
        modelBuilder.Entity<Coach>(e =>
        {
            e.HasIndex(x => x.TeamId);

            e.HasOne(x => x.Team)
                .WithMany(x => x.Coaches)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Player ────────────────────────────────────────────────────
        modelBuilder.Entity<Player>(e =>
        {
            e.HasIndex(x => x.SrId).IsUnique();
            e.HasIndex(x => x.TeamId);

            e.HasOne(x => x.Team)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(x => x.DraftTeam)
                .WithMany()
                .HasForeignKey(x => x.DraftTeamId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ── Game ──────────────────────────────────────────────────────
        modelBuilder.Entity<Game>(e =>
        {
            e.HasIndex(x => x.SrId).IsUnique();
            e.HasIndex(x => x.HomeTeamId);
            e.HasIndex(x => x.AwayTeamId);
            e.HasIndex(x => x.Scheduled);
            e.HasIndex(x => new { x.SeasonYear, x.SeasonType, x.WeekSequence });

            e.HasOne(x => x.HomeTeam)
                .WithMany()
                .HasForeignKey(x => x.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.AwayTeam)
                .WithMany()
                .HasForeignKey(x => x.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Venue)
                .WithMany()
                .HasForeignKey(x => x.VenueId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ── Period ────────────────────────────────────────────────────
        modelBuilder.Entity<Period>(e =>
        {
            // e.HasIndex(x => x.GameId);
            // e.HasIndex(x => new { x.GameId, x.Number }).IsUnique();

            // e.HasOne<Game>()
            //  .WithMany()
            //  .HasForeignKey(x => x.GameId)
            //  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── CoinToss ──────────────────────────────────────────────────
        modelBuilder.Entity<CoinToss>(e =>
        {
            e.HasIndex(x => x.PeriodId);

            // Restrict to avoid multiple cascade paths: Game → CoinToss and Game → Period → CoinToss
            e.HasOne<Period>()
                .WithMany()
                .HasForeignKey(x => x.PeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            // WinnerId: team that won the toss (no nav property on CoinToss)
            e.HasOne<Team>()
                .WithMany()
                .HasForeignKey(x => x.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Drive ─────────────────────────────────────────────────────
        modelBuilder.Entity<Drive>(e =>
        {
            e.HasIndex(x => x.GameId);
            //e.HasIndex(x => x.Sequence);
            //e.HasIndex(x => new { x.GameId, x.Sequence });
            e.HasKey(x => x.Id);

            e.HasOne(x => x.Game)
                .WithMany(x => x.Drives)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // No nav property on Drive for Period
            // e.HasOne<Period>()
            //  .WithMany()
            //  .HasForeignKey(x => x.PeriodId)
            //  .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(x => x.Plays)
                .WithOne(x => x.Drive)
                .HasForeignKey(x => x.DriveId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.OffensiveTeam)
                .WithMany()
                .HasForeignKey(x => x.OffensiveTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.DefensiveTeam)
                .WithMany()
                .HasForeignKey(x => x.DefensiveTeamId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // ── PlayerSeasonStats ─────────────────────────────────────────
        modelBuilder.Entity<PlayerSeasonStats>(e =>
        {
            e.HasIndex(x => x.PlayerId);
            e.HasIndex(x => new { x.PlayerId, x.SeasonYear, x.SeasonType }).IsUnique();

            e.HasOne(x => x.Player)
                .WithMany(x => x.SeasonStats)
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Team)
                .WithMany()
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ── PlayerGameStats ───────────────────────────────────────────
        modelBuilder.Entity<PlayerGameStats>(e =>
        {
            e.HasIndex(x => x.PlayerId);
            e.HasIndex(x => x.GameId);
            e.HasIndex(x => new { x.PlayerId, x.GameId }).IsUnique();

            e.HasOne(x => x.Player)
                .WithMany(x => x.GameStats)
                .HasForeignKey(x => x.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Restrict to avoid multiple cascade paths: Player → PlayerGameStats and Game → PlayerGameStats
            e.HasOne(x => x.Game)
                .WithMany()
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Team)
                .WithMany()
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ── TeamSeasonStats ───────────────────────────────────────────
        modelBuilder.Entity<TeamSeasonStats>(e =>
        {
            e.HasIndex(x => x.TeamId);
            e.HasIndex(x => new { x.TeamId, x.SeasonYear, x.SeasonType }).IsUnique();

            e.HasOne(x => x.Team)
                .WithMany()
                .HasForeignKey(x => x.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ── PbpDriveEvent ─────────────────────────────────────────────
        // NOTE: PbpDriveEvent has no DriveId — no FK to Drive can be configured until one is added.
        // Also has duplicate-typed properties (EventType/Type, DriveType/PlayType) — review entity.
        modelBuilder.Entity<DriveEvent>(e =>
        {
            e.HasIndex(x => x.Sequence);

            e.HasOne(x => x.StartTeam)
                .WithMany()
                .HasForeignKey(x => x.StartPossessionTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.EndTeam)
                .WithMany()
                .HasForeignKey(x => x.EndPossessionTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(x => x.EventStats)
                .WithOne(x => x.DriveEvent)
                .HasForeignKey(x => x.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ── PbpEventStatistics ────────────────────────────────────────
        // No surrogate PK — composite key on (EventId, StatType).
        // Add a Guid Id if multiple rows per (event, type) are needed.
        modelBuilder.Entity<PbpEventStatistics>(e =>
        {
            e.HasKey(x => new { x.EventId, x.StatType });

            e.HasOne<DriveEvent>()
                .WithMany()
                .HasForeignKey(x => x.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ── SyncLog ───────────────────────────────────────────────────
        modelBuilder.Entity<SyncLog>(e =>
        {
            e.HasIndex(x => x.EntityType);
            e.HasIndex(x => x.StartedAt);
        });
    }
}