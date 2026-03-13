using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Entities;

namespace PatriotIndex.Domain.Context;

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
    public DbSet<Play>              Plays              { get; set; }
    public DbSet<GameEvent>         GameEvents         { get; set; }
    public DbSet<Period>            Periods            { get; set; }
    public DbSet<Player>            Players            { get; set; }
    public DbSet<PlayerGameStats>   PlayerGameStats    { get; set; }
    public DbSet<PlayerSeasonStats> PlayerSeasonStats  { get; set; }
    public DbSet<SyncLog>           SyncLogs           { get; set; }
    public DbSet<Team>              Teams              { get; set; }
    public DbSet<TeamSeasonStats>   TeamSeasonStats    { get; set; }
    public DbSet<Venue>             Venues             { get; set; }
    public DbSet<AppConfig>         AppConfigs         { get; set; }
    public DbSet<Season>            Seasons            { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Season>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedNever();
            
            e.Property(x => x.Year).IsRequired();
            
            e.Property(x => x.Code).IsRequired();
            
            e.Property(x => x.StartDate)
                .HasColumnType("date")
                .IsRequired();
            
            e.Property(x => x.EndDate)
                .HasColumnType("date")
                .IsRequired();
            
            e.Property(s => s.Code)
                .HasColumnName("code")
                .HasMaxLength(3)
                .IsRequired();
            
            e.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();
            
            e.HasIndex(x => new { x.Year, x.Code })
                .IsUnique();

            e.HasIndex(s => s.Status);
        });
        
        modelBuilder.Entity<AppConfig>(e =>
        {
            e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.HasIndex(x => x.Key).IsUnique();
        });

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
            e.HasKey(x => x.Id);

            e.HasOne(x => x.Game)
                .WithMany(x => x.Drives)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(x => x.Plays)
                .WithOne(x => x.Drive)
                .HasForeignKey(x => x.DriveId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(x => x.Events)
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

            e.OwnsOne(x => x.Stats, block =>
            {
                block.ToJson("stats");
                block.OwnsOne(b => b.Rushing);
                block.OwnsOne(b => b.Passing);
                block.OwnsOne(b => b.Receiving);
                block.OwnsOne(b => b.Defense);
                block.OwnsOne(b => b.FieldGoals);
                block.OwnsOne(b => b.ExtraPoints);
                block.OwnsOne(b => b.Punts);
                block.OwnsOne(b => b.Kickoffs);
                block.OwnsOne(b => b.PuntReturns);
                block.OwnsOne(b => b.KickReturns);
                block.OwnsOne(b => b.MiscReturns);
                block.OwnsOne(b => b.Fumbles);
                block.OwnsOne(b => b.Penalties);
                block.OwnsOne(b => b.IntReturns);
            });
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

            void ConfigureTeamStatBlock(
                Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder<TeamSeasonStats, TeamStatBlock> block)
            {
                block.OwnsOne(b => b.Touchdowns);
                block.OwnsOne(b => b.Rushing);
                block.OwnsOne(b => b.Passing);
                block.OwnsOne(b => b.Receiving);
                block.OwnsOne(b => b.Defense);
                block.OwnsOne(b => b.FieldGoals);
                block.OwnsOne(b => b.Kickoffs);
                block.OwnsOne(b => b.KickReturns);
                block.OwnsOne(b => b.Punts);
                block.OwnsOne(b => b.PuntReturns);
                block.OwnsOne(b => b.Interceptions);
                block.OwnsOne(b => b.IntReturns);
                block.OwnsOne(b => b.Fumbles);
                block.OwnsOne(b => b.FirstDowns);
                block.OwnsOne(b => b.Penalties);
                block.OwnsOne(b => b.MiscReturns);
                block.OwnsOne(b => b.ExtraPoints, ep =>
                {
                    ep.OwnsOne(x => x.Kicks);
                    ep.OwnsOne(x => x.Conversions);
                });
                block.OwnsOne(b => b.Efficiency, eff =>
                {
                    eff.OwnsOne(x => x.Goaltogo);
                    eff.OwnsOne(x => x.Redzone);
                    eff.OwnsOne(x => x.Thirddown);
                    eff.OwnsOne(x => x.Fourthdown);
                });
            }

            e.OwnsOne(x => x.Record, block =>
            {
                block.ToJson("record");
                ConfigureTeamStatBlock(block);
            });

            e.OwnsOne(x => x.Opponents, block =>
            {
                block.ToJson("opponents");
                ConfigureTeamStatBlock(block);
            });
        });

        // ── SyncLog ───────────────────────────────────────────────────
        modelBuilder.Entity<SyncLog>(e =>
        {
            e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.HasIndex(x => x.EntityType);
            e.HasIndex(x => x.StartedAt);
        });

        // ── Play ──────────────────────────────────────────────────────
        modelBuilder.Entity<Play>(entity =>
        {
            entity.ToTable("plays");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.PlayType).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Clock).HasMaxLength(10).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000);

            entity.OwnsOne(e => e.StartSituation, s =>
            {
                s.Property(x => x.Clock)            .HasColumnName("start_clock").HasMaxLength(10);
                s.Property(x => x.Down)             .HasColumnName("start_down");
                s.Property(x => x.YardsToFirstDown) .HasColumnName("start_yfd");
                s.Property(x => x.Yardline)         .HasColumnName("start_yardline");
                s.Property(x => x.YardlineTeam)     .HasColumnName("start_yardline_team").HasMaxLength(10);
                s.Property(x => x.PossessionTeamId) .HasColumnName("start_possession_team_id");
                s.HasOne<Team>().WithMany()
                    .HasForeignKey(x => x.PossessionTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            entity.OwnsOne(e => e.EndSituation, s =>
            {
                s.Property(x => x.Clock)            .HasColumnName("end_clock").HasMaxLength(10);
                s.Property(x => x.Down)             .HasColumnName("end_down");
                s.Property(x => x.YardsToFirstDown) .HasColumnName("end_yfd");
                s.Property(x => x.Yardline)         .HasColumnName("end_yardline");
                s.Property(x => x.YardlineTeam)     .HasColumnName("end_yardline_team").HasMaxLength(10);
                s.Property(x => x.PossessionTeamId) .HasColumnName("end_possession_team_id");
                s.HasOne<Team>().WithMany()
                    .HasForeignKey(x => x.PossessionTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            entity.OwnsMany(e => e.Statistics, stat =>
            {
                stat.ToJson("statistics");
                stat.OwnsOne(s => s.Player);
                stat.OwnsOne(s => s.Team);
            });

            entity.OwnsMany(e => e.Details, detail =>
            {
                detail.ToJson("details");
                detail.OwnsMany(d => d.Players);
            });

            entity.HasOne(e => e.Game)
                .WithMany()
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.PossessionTeam)
                .WithMany()
                .HasForeignKey(e => e.PossessionTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.GameId);
            entity.HasIndex(e => e.DriveId);
            entity.HasIndex(e => e.PossessionTeamId);
            entity.HasIndex(e => new { e.GameId, e.PlayType });
            entity.HasIndex(e => new { e.GameId, e.Down });
            // GIN indexes on statistics and details are added via raw SQL in the migration
        });

        // ── GameEvent ─────────────────────────────────────────────────
        modelBuilder.Entity<GameEvent>(entity =>
        {
            entity.ToTable("game_events");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.EventType).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Clock).HasMaxLength(10).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000);

            entity.HasOne(e => e.Game)
                .WithMany()
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.GameId);
            entity.HasIndex(e => e.DriveId);
            entity.HasIndex(e => new { e.GameId, e.EventType });
        });
    }
}