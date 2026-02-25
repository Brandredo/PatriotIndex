using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PatriotIndex.Domain.Migrations;

namespace PatriotIndex.Domain.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<CoinToss> CoinTosses { get; set; }

    public virtual DbSet<Conference> Conferences { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<Drife> Drives { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<PbpDriveEvent> PbpDriveEvents { get; set; }

    public virtual DbSet<PbpEventStatistic> PbpEventStatistics { get; set; }

    public virtual DbSet<Period> Periods { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerGameStat> PlayerGameStats { get; set; }

    public virtual DbSet<PlayerSeasonStat> PlayerSeasonStats { get; set; }

    public virtual DbSet<SyncLog> SyncLogs { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamColor> TeamColors { get; set; }

    public virtual DbSet<TeamSeasonStat> TeamSeasonStats { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=patriotindex_dev;Username=brandredo;Password=yAvQuTH9WTxZjq.yHX@RjP78.Hgb;Timeout=30;Command Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_coaches");

            entity.ToTable("coaches");

            entity.HasIndex(e => e.TeamId, "ix_coaches_team_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.TeamId).HasColumnName("team_id");

            entity.HasOne(d => d.Team).WithMany(p => p.Coaches)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("fk_coaches_teams_team_id");
        });

        modelBuilder.Entity<CoinToss>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_coin_tosses");

            entity.ToTable("coin_tosses");

            entity.HasIndex(e => e.PeriodId, "ix_coin_tosses_period_id");

            entity.HasIndex(e => e.PeriodId1, "ix_coin_tosses_period_id1");

            entity.HasIndex(e => e.WinnerId, "ix_coin_tosses_winner_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Decision).HasColumnName("decision");
            entity.Property(e => e.Direction).HasColumnName("direction");
            entity.Property(e => e.PeriodId).HasColumnName("period_id");
            entity.Property(e => e.PeriodId1).HasColumnName("period_id1");
            entity.Property(e => e.WinnerId).HasColumnName("winner_id");

            entity.HasOne(d => d.Period).WithMany(p => p.CoinTossPeriods)
                .HasForeignKey(d => d.PeriodId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_coin_tosses_periods_period_id");

            entity.HasOne(d => d.PeriodId1Navigation).WithMany(p => p.CoinTossPeriodId1Navigations)
                .HasForeignKey(d => d.PeriodId1)
                .HasConstraintName("fk_coin_tosses_periods_period_id1");

            entity.HasOne(d => d.Winner).WithMany(p => p.CoinTosses)
                .HasForeignKey(d => d.WinnerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_coin_tosses_teams_winner_id");
        });

        modelBuilder.Entity<Conference>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_conferences");

            entity.ToTable("conferences");

            entity.HasIndex(e => e.Alias, "ix_conferences_alias").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Alias).HasColumnName("alias");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_divisions");

            entity.ToTable("divisions");

            entity.HasIndex(e => e.ConferenceId, "ix_divisions_conference_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Alias).HasColumnName("alias");
            entity.Property(e => e.ConferenceId).HasColumnName("conference_id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.Conference).WithMany(p => p.Divisions)
                .HasForeignKey(d => d.ConferenceId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_divisions_conferences_conference_id");
        });

        modelBuilder.Entity<Drife>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_drives");

            entity.ToTable("drives");

            entity.HasIndex(e => e.DefensiveTeamId, "ix_drives_defensive_team_id");

            entity.HasIndex(e => e.GameId, "ix_drives_game_id");

            entity.HasIndex(e => e.OffensiveTeamId, "ix_drives_offensive_team_id");

            entity.HasIndex(e => e.PeriodId, "ix_drives_period_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DefensivePoints).HasColumnName("defensive_points");
            entity.Property(e => e.DefensiveStartPoints).HasColumnName("defensive_start_points");
            entity.Property(e => e.DefensiveTeamId).HasColumnName("defensive_team_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.EndClock).HasColumnName("end_clock");
            entity.Property(e => e.EndReason).HasColumnName("end_reason");
            entity.Property(e => e.FarthestDriveYardLine).HasColumnName("farthest_drive_yard_line");
            entity.Property(e => e.FirstDowns).HasColumnName("first_downs");
            entity.Property(e => e.FirstDriveYardLine).HasColumnName("first_drive_yard_line");
            entity.Property(e => e.GainedYards).HasColumnName("gained_yards");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.LastDriveYardLine).HasColumnName("last_drive_yard_line");
            entity.Property(e => e.NetYards).HasColumnName("net_yards");
            entity.Property(e => e.OffensivePoints).HasColumnName("offensive_points");
            entity.Property(e => e.OffensiveStartPoints).HasColumnName("offensive_start_points");
            entity.Property(e => e.OffensiveTeamId).HasColumnName("offensive_team_id");
            entity.Property(e => e.PatPointsAttempted).HasColumnName("pat_points_attempted");
            entity.Property(e => e.PenaltyYards).HasColumnName("penalty_yards");
            entity.Property(e => e.PeriodId).HasColumnName("period_id");
            entity.Property(e => e.PeriodNumber).HasColumnName("period_number");
            entity.Property(e => e.PlayCount).HasColumnName("play_count");
            entity.Property(e => e.Sequence).HasColumnName("sequence");
            entity.Property(e => e.StartClock).HasColumnName("start_clock");
            entity.Property(e => e.StartReason).HasColumnName("start_reason");
            entity.Property(e => e.TeamSequence).HasColumnName("team_sequence");

            entity.HasOne(d => d.DefensiveTeam).WithMany(p => p.DrifeDefensiveTeams)
                .HasForeignKey(d => d.DefensiveTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_drives_teams_defensive_team_id");

            entity.HasOne(d => d.Game).WithMany(p => p.Drives)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("fk_drives_games_game_id");

            entity.HasOne(d => d.OffensiveTeam).WithMany(p => p.DrifeOffensiveTeams)
                .HasForeignKey(d => d.OffensiveTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_drives_teams_offensive_team_id");

            entity.HasOne(d => d.Period).WithMany(p => p.Drives)
                .HasForeignKey(d => d.PeriodId)
                .HasConstraintName("fk_drives_periods_period_id");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_games");

            entity.ToTable("games");

            entity.HasIndex(e => e.AwayTeamId, "ix_games_away_team_id");

            entity.HasIndex(e => e.HomeTeamId, "ix_games_home_team_id");

            entity.HasIndex(e => e.Scheduled, "ix_games_scheduled");

            entity.HasIndex(e => new { e.SeasonYear, e.SeasonType, e.WeekSequence }, "ix_games_season_year_season_type_week_sequence");

            entity.HasIndex(e => e.SrId, "ix_games_sr_id").IsUnique();

            entity.HasIndex(e => e.VenueId, "ix_games_venue_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Attendance).HasColumnName("attendance");
            entity.Property(e => e.AwayPoints).HasColumnName("away_points");
            entity.Property(e => e.AwayTeamId).HasColumnName("away_team_id");
            entity.Property(e => e.BroadcastNetwork).HasColumnName("broadcast_network");
            entity.Property(e => e.ConferenceGame).HasColumnName("conference_game");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.GameType).HasColumnName("game_type");
            entity.Property(e => e.HomePoints).HasColumnName("home_points");
            entity.Property(e => e.HomeTeamId).HasColumnName("home_team_id");
            entity.Property(e => e.NeutralSite).HasColumnName("neutral_site");
            entity.Property(e => e.Scheduled).HasColumnName("scheduled");
            entity.Property(e => e.SeasonType).HasColumnName("season_type");
            entity.Property(e => e.SeasonYear).HasColumnName("season_year");
            entity.Property(e => e.SrId).HasColumnName("sr_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.VenueId).HasColumnName("venue_id");
            entity.Property(e => e.WeatherCondition).HasColumnName("weather_condition");
            entity.Property(e => e.WeatherHumidity).HasColumnName("weather_humidity");
            entity.Property(e => e.WeatherTemp).HasColumnName("weather_temp");
            entity.Property(e => e.WeatherWindDirection).HasColumnName("weather_wind_direction");
            entity.Property(e => e.WeatherWindSpeed).HasColumnName("weather_wind_speed");
            entity.Property(e => e.WeekSequence).HasColumnName("week_sequence");
            entity.Property(e => e.WeekTitle).HasColumnName("week_title");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.GameAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_games_teams_away_team_id");

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.GameHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_games_teams_home_team_id");

            entity.HasOne(d => d.Venue).WithMany(p => p.Games)
                .HasForeignKey(d => d.VenueId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_games_venues_venue_id");
        });

        modelBuilder.Entity<PbpDriveEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_pbp_drive_events");

            entity.ToTable("pbp_drive_events");

            entity.HasIndex(e => e.DriveId, "ix_pbp_drive_events_drive_id");

            entity.HasIndex(e => e.EndPossessionTeamId, "ix_pbp_drive_events_end_possession_team_id");

            entity.HasIndex(e => e.PeriodId, "ix_pbp_drive_events_period_id");

            entity.HasIndex(e => e.Sequence, "ix_pbp_drive_events_sequence");

            entity.HasIndex(e => e.StartPossessionTeamId, "ix_pbp_drive_events_start_possession_team_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AwayScore).HasColumnName("away_score");
            entity.Property(e => e.Blitz).HasColumnName("blitz");
            entity.Property(e => e.Clock).HasColumnName("clock");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DriveId).HasColumnName("drive_id");
            entity.Property(e => e.DriveType).HasColumnName("drive_type");
            entity.Property(e => e.EndClock).HasColumnName("end_clock");
            entity.Property(e => e.EndDown).HasColumnName("end_down");
            entity.Property(e => e.EndLocationYardLine).HasColumnName("end_location_yard_line");
            entity.Property(e => e.EndPossessionTeamId).HasColumnName("end_possession_team_id");
            entity.Property(e => e.EndYardsToGain).HasColumnName("end_yards_to_gain");
            entity.Property(e => e.EventType).HasColumnName("event_type");
            entity.Property(e => e.FakeFieldGoal).HasColumnName("fake_field_goal");
            entity.Property(e => e.FakePunt).HasColumnName("fake_punt");
            entity.Property(e => e.HashMark).HasColumnName("hash_mark");
            entity.Property(e => e.HomeScore).HasColumnName("home_score");
            entity.Property(e => e.Huddle).HasColumnName("huddle");
            entity.Property(e => e.LeftTightEnds).HasColumnName("left_tight_ends");
            entity.Property(e => e.MenInBox).HasColumnName("men_in_box");
            entity.Property(e => e.PassRoute).HasColumnName("pass_route");
            entity.Property(e => e.PeriodId)
                .HasDefaultValueSql("'00000000-0000-0000-0000-000000000000'::uuid")
                .HasColumnName("period_id");
            entity.Property(e => e.PlayAction).HasColumnName("play_action");
            entity.Property(e => e.PlayDirection).HasColumnName("play_direction");
            entity.Property(e => e.PlayType).HasColumnName("play_type");
            entity.Property(e => e.PlayersRushed).HasColumnName("players_rushed");
            entity.Property(e => e.PocketLocation).HasColumnName("pocket_location");
            entity.Property(e => e.QbSnap).HasColumnName("qb_snap");
            entity.Property(e => e.RightTightEnds).HasColumnName("right_tight_ends");
            entity.Property(e => e.RunPassOption).HasColumnName("run_pass_option");
            entity.Property(e => e.ScreenPass).HasColumnName("screen_pass");
            entity.Property(e => e.Sequence).HasColumnName("sequence");
            entity.Property(e => e.StartClock).HasColumnName("start_clock");
            entity.Property(e => e.StartDown).HasColumnName("start_down");
            entity.Property(e => e.StartLocationYardLine).HasColumnName("start_location_yard_line");
            entity.Property(e => e.StartPossessionTeamId).HasColumnName("start_possession_team_id");
            entity.Property(e => e.StartYardsToGain).HasColumnName("start_yards_to_gain");
            entity.Property(e => e.WallClock).HasColumnName("wall_clock");

            entity.HasOne(d => d.Drive).WithMany(p => p.PbpDriveEvents)
                .HasForeignKey(d => d.DriveId)
                .HasConstraintName("fk_pbp_drive_events_drives_drive_id");

            entity.HasOne(d => d.EndPossessionTeam).WithMany(p => p.PbpDriveEventEndPossessionTeams)
                .HasForeignKey(d => d.EndPossessionTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pbp_drive_events_teams_end_possession_team_id");

            entity.HasOne(d => d.Period).WithMany(p => p.PbpDriveEvents)
                .HasForeignKey(d => d.PeriodId)
                .HasConstraintName("fk_pbp_drive_events_periods_period_id");

            entity.HasOne(d => d.StartPossessionTeam).WithMany(p => p.PbpDriveEventStartPossessionTeams)
                .HasForeignKey(d => d.StartPossessionTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pbp_drive_events_teams_start_possession_team_id");
        });

        modelBuilder.Entity<PbpEventStatistic>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.StatType }).HasName("pk_pbp_event_statistics");

            entity.ToTable("pbp_event_statistics");

            entity.HasIndex(e => e.DriveEventId, "ix_pbp_event_statistics_drive_event_id");

            entity.HasIndex(e => e.PlayerId, "ix_pbp_event_statistics_player_id");

            entity.HasIndex(e => e.TeamId, "ix_pbp_event_statistics_team_id");

            entity.HasIndex(e => e.IdBigint, "ux_pbp_event_statistics_id_bigint").IsUnique();

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.StatType).HasColumnName("stat_type");
            entity.Property(e => e.Attempt).HasColumnName("attempt");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Complete).HasColumnName("complete");
            entity.Property(e => e.DriveEventId).HasColumnName("drive_event_id");
            entity.Property(e => e.ExtraData)
                .HasColumnType("jsonb")
                .HasColumnName("extra_data");
            entity.Property(e => e.FirstDown).HasColumnName("first_down");
            entity.Property(e => e.Fumble).HasColumnName("fumble");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdBigint)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_bigint");
            entity.Property(e => e.Interception).HasColumnName("interception");
            entity.Property(e => e.Penalty).HasColumnName("penalty");
            entity.Property(e => e.PenaltyYards).HasColumnName("penalty_yards");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.ReturnYards).HasColumnName("return_yards");
            entity.Property(e => e.Sack).HasColumnName("sack");
            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.Touchback).HasColumnName("touchback");
            entity.Property(e => e.Touchdown).HasColumnName("touchdown");
            entity.Property(e => e.Yards).HasColumnName("yards");

            entity.HasOne(d => d.DriveEvent).WithMany(p => p.PbpEventStatisticDriveEvents)
                .HasForeignKey(d => d.DriveEventId)
                .HasConstraintName("fk_pbp_event_statistics_pbp_drive_events_drive_event_id");

            entity.HasOne(d => d.Event).WithMany(p => p.PbpEventStatisticEvents)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("fk_pbp_event_statistics_pbp_drive_events_event_id");

            entity.HasOne(d => d.Player).WithMany(p => p.PbpEventStatistics)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("fk_pbp_event_statistics_players_player_id");

            entity.HasOne(d => d.Team).WithMany(p => p.PbpEventStatistics)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("fk_pbp_event_statistics_teams_team_id");
        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_periods");

            entity.ToTable("periods");

            entity.HasIndex(e => e.GameId, "ix_periods_game_id");

            entity.HasIndex(e => e.GameId1, "ix_periods_game_id1");

            entity.HasIndex(e => new { e.GameId, e.Number }, "ix_periods_game_id_number").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AwayScore).HasColumnName("away_score");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.GameId1).HasColumnName("game_id1");
            entity.Property(e => e.HomeScore).HasColumnName("home_score");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Sequence).HasColumnName("sequence");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.Game).WithMany(p => p.PeriodGames)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("fk_periods_games_game_id");

            entity.HasOne(d => d.GameId1Navigation).WithMany(p => p.PeriodGameId1Navigations)
                .HasForeignKey(d => d.GameId1)
                .HasConstraintName("fk_periods_games_game_id1");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_players");

            entity.ToTable("players");

            entity.HasIndex(e => e.DraftTeamId, "ix_players_draft_team_id");

            entity.HasIndex(e => e.SrId, "ix_players_sr_id").IsUnique();

            entity.HasIndex(e => e.TeamId, "ix_players_team_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.College).HasColumnName("college");
            entity.Property(e => e.DraftPick).HasColumnName("draft_pick");
            entity.Property(e => e.DraftRound).HasColumnName("draft_round");
            entity.Property(e => e.DraftTeamId).HasColumnName("draft_team_id");
            entity.Property(e => e.DraftYear).HasColumnName("draft_year");
            entity.Property(e => e.Experience).HasColumnName("experience");
            entity.Property(e => e.FirstName)
                .HasDefaultValueSql("''::text")
                .HasColumnName("first_name");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Jersey).HasColumnName("jersey");
            entity.Property(e => e.LastName)
                .HasDefaultValueSql("''::text")
                .HasColumnName("last_name");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.RookieYear).HasColumnName("rookie_year");
            entity.Property(e => e.Salary).HasColumnName("salary");
            entity.Property(e => e.SrId).HasColumnName("sr_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.DraftTeam).WithMany(p => p.PlayerDraftTeams)
                .HasForeignKey(d => d.DraftTeamId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_players_teams_draft_team_id");

            entity.HasOne(d => d.Team).WithMany(p => p.PlayerTeams)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_players_teams_team_id");
        });

        modelBuilder.Entity<PlayerGameStat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_player_game_stats");

            entity.ToTable("player_game_stats");

            entity.HasIndex(e => e.GameId, "ix_player_game_stats_game_id");

            entity.HasIndex(e => e.PlayerId, "ix_player_game_stats_player_id");

            entity.HasIndex(e => new { e.PlayerId, e.GameId }, "ix_player_game_stats_player_id_game_id").IsUnique();

            entity.HasIndex(e => e.TeamId, "ix_player_game_stats_team_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DefAssists).HasColumnName("def_assists");
            entity.Property(e => e.DefForcedFumbles).HasColumnName("def_forced_fumbles");
            entity.Property(e => e.DefInterceptions).HasColumnName("def_interceptions");
            entity.Property(e => e.DefPassesDefended).HasColumnName("def_passes_defended");
            entity.Property(e => e.DefQbHits).HasColumnName("def_qb_hits");
            entity.Property(e => e.DefSacks).HasColumnName("def_sacks");
            entity.Property(e => e.DefTackles).HasColumnName("def_tackles");
            entity.Property(e => e.FgAtt).HasColumnName("fg_att");
            entity.Property(e => e.FgLong).HasColumnName("fg_long");
            entity.Property(e => e.FgMade).HasColumnName("fg_made");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PassAtt).HasColumnName("pass_att");
            entity.Property(e => e.PassCmp).HasColumnName("pass_cmp");
            entity.Property(e => e.PassInt).HasColumnName("pass_int");
            entity.Property(e => e.PassRating).HasColumnName("pass_rating");
            entity.Property(e => e.PassSackYds).HasColumnName("pass_sack_yds");
            entity.Property(e => e.PassSacks).HasColumnName("pass_sacks");
            entity.Property(e => e.PassTd).HasColumnName("pass_td");
            entity.Property(e => e.PassYds).HasColumnName("pass_yds");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.PuntAtt).HasColumnName("punt_att");
            entity.Property(e => e.PuntAvg).HasColumnName("punt_avg");
            entity.Property(e => e.PuntYds).HasColumnName("punt_yds");
            entity.Property(e => e.RecAvg).HasColumnName("rec_avg");
            entity.Property(e => e.RecFumbles).HasColumnName("rec_fumbles");
            entity.Property(e => e.RecLong).HasColumnName("rec_long");
            entity.Property(e => e.RecReceptions).HasColumnName("rec_receptions");
            entity.Property(e => e.RecTargets).HasColumnName("rec_targets");
            entity.Property(e => e.RecTd).HasColumnName("rec_td");
            entity.Property(e => e.RecYds).HasColumnName("rec_yds");
            entity.Property(e => e.RushAtt).HasColumnName("rush_att");
            entity.Property(e => e.RushAvg).HasColumnName("rush_avg");
            entity.Property(e => e.RushFumbles).HasColumnName("rush_fumbles");
            entity.Property(e => e.RushLong).HasColumnName("rush_long");
            entity.Property(e => e.RushTd).HasColumnName("rush_td");
            entity.Property(e => e.RushYds).HasColumnName("rush_yds");
            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.XpAtt).HasColumnName("xp_att");
            entity.Property(e => e.XpMade).HasColumnName("xp_made");

            entity.HasOne(d => d.Game).WithMany(p => p.PlayerGameStats)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_player_game_stats_games_game_id");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerGameStats)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("fk_player_game_stats_players_player_id");

            entity.HasOne(d => d.Team).WithMany(p => p.PlayerGameStats)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_player_game_stats_teams_team_id");
        });

        modelBuilder.Entity<PlayerSeasonStat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_player_season_stats");

            entity.ToTable("player_season_stats");

            entity.HasIndex(e => e.PlayerId, "ix_player_season_stats_player_id");

            entity.HasIndex(e => new { e.PlayerId, e.SeasonYear, e.SeasonType }, "ix_player_season_stats_player_id_season_year_season_type").IsUnique();

            entity.HasIndex(e => e.TeamId, "ix_player_season_stats_team_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DefAssists).HasColumnName("def_assists");
            entity.Property(e => e.DefForcedFumbles).HasColumnName("def_forced_fumbles");
            entity.Property(e => e.DefInterceptions).HasColumnName("def_interceptions");
            entity.Property(e => e.DefPassesDefended).HasColumnName("def_passes_defended");
            entity.Property(e => e.DefQbHits).HasColumnName("def_qb_hits");
            entity.Property(e => e.DefSacks).HasColumnName("def_sacks");
            entity.Property(e => e.DefTackles).HasColumnName("def_tackles");
            entity.Property(e => e.FgAtt).HasColumnName("fg_att");
            entity.Property(e => e.FgLong).HasColumnName("fg_long");
            entity.Property(e => e.FgMade).HasColumnName("fg_made");
            entity.Property(e => e.GamesPlayed).HasColumnName("games_played");
            entity.Property(e => e.GamesStarted).HasColumnName("games_started");
            entity.Property(e => e.PassAtt).HasColumnName("pass_att");
            entity.Property(e => e.PassCmp).HasColumnName("pass_cmp");
            entity.Property(e => e.PassInt).HasColumnName("pass_int");
            entity.Property(e => e.PassRating).HasColumnName("pass_rating");
            entity.Property(e => e.PassSackYds).HasColumnName("pass_sack_yds");
            entity.Property(e => e.PassSacks).HasColumnName("pass_sacks");
            entity.Property(e => e.PassTd).HasColumnName("pass_td");
            entity.Property(e => e.PassYds).HasColumnName("pass_yds");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.PuntAtt).HasColumnName("punt_att");
            entity.Property(e => e.PuntAvg).HasColumnName("punt_avg");
            entity.Property(e => e.PuntYds).HasColumnName("punt_yds");
            entity.Property(e => e.RecAvg).HasColumnName("rec_avg");
            entity.Property(e => e.RecFumbles).HasColumnName("rec_fumbles");
            entity.Property(e => e.RecLong).HasColumnName("rec_long");
            entity.Property(e => e.RecReceptions).HasColumnName("rec_receptions");
            entity.Property(e => e.RecTargets).HasColumnName("rec_targets");
            entity.Property(e => e.RecTd).HasColumnName("rec_td");
            entity.Property(e => e.RecYds).HasColumnName("rec_yds");
            entity.Property(e => e.RushAtt).HasColumnName("rush_att");
            entity.Property(e => e.RushAvg).HasColumnName("rush_avg");
            entity.Property(e => e.RushFumbles).HasColumnName("rush_fumbles");
            entity.Property(e => e.RushLong).HasColumnName("rush_long");
            entity.Property(e => e.RushTd).HasColumnName("rush_td");
            entity.Property(e => e.RushYds).HasColumnName("rush_yds");
            entity.Property(e => e.SeasonType).HasColumnName("season_type");
            entity.Property(e => e.SeasonYear).HasColumnName("season_year");
            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.XpAtt).HasColumnName("xp_att");
            entity.Property(e => e.XpMade).HasColumnName("xp_made");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerSeasonStats)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("fk_player_season_stats_players_player_id");

            entity.HasOne(d => d.Team).WithMany(p => p.PlayerSeasonStats)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_player_season_stats_teams_team_id");
        });

        modelBuilder.Entity<SyncLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_sync_logs");

            entity.ToTable("sync_logs");

            entity.HasIndex(e => e.EntityType, "ix_sync_logs_entity_type");

            entity.HasIndex(e => e.StartedAt, "ix_sync_logs_started_at");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.EntityType).HasColumnName("entity_type");
            entity.Property(e => e.ErrorMessage).HasColumnName("error_message");
            entity.Property(e => e.RawResponse)
                .HasColumnType("jsonb")
                .HasColumnName("raw_response");
            entity.Property(e => e.RecordCount).HasColumnName("record_count");
            entity.Property(e => e.StartedAt).HasColumnName("started_at");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_teams");

            entity.ToTable("teams");

            entity.HasIndex(e => e.Alias, "ix_teams_alias").IsUnique();

            entity.HasIndex(e => e.DivisionId, "ix_teams_division_id");

            entity.HasIndex(e => e.SrId, "ix_teams_sr_id").IsUnique();

            entity.HasIndex(e => e.VenueId, "ix_teams_venue_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Alias).HasColumnName("alias");
            entity.Property(e => e.ChampionshipSeasons).HasColumnName("championship_seasons");
            entity.Property(e => e.ChampionshipsWon).HasColumnName("championships_won");
            entity.Property(e => e.ConferenceTitles).HasColumnName("conference_titles");
            entity.Property(e => e.DivisionId).HasColumnName("division_id");
            entity.Property(e => e.DivisionTitles).HasColumnName("division_titles");
            entity.Property(e => e.Founded).HasColumnName("founded");
            entity.Property(e => e.GeneralManager).HasColumnName("general_manager");
            entity.Property(e => e.Market).HasColumnName("market");
            entity.Property(e => e.Mascot).HasColumnName("mascot");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Owner).HasColumnName("owner");
            entity.Property(e => e.PlayoffAppearances).HasColumnName("playoff_appearances");
            entity.Property(e => e.President).HasColumnName("president");
            entity.Property(e => e.SecondaryColor).HasColumnName("secondary_color");
            entity.Property(e => e.SrId).HasColumnName("sr_id");
            entity.Property(e => e.VenueId).HasColumnName("venue_id");

            entity.HasOne(d => d.Division).WithMany(p => p.Teams)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_teams_divisions_division_id");

            entity.HasOne(d => d.Venue).WithMany(p => p.Teams)
                .HasForeignKey(d => d.VenueId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_teams_venues_venue_id");
        });

        modelBuilder.Entity<TeamColor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_team_colors");

            entity.ToTable("team_colors");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.PrimaryColor).HasColumnName("primary_color");
            entity.Property(e => e.SecondaryColor).HasColumnName("secondary_color");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.TeamColor)
                .HasForeignKey<TeamColor>(d => d.Id)
                .HasConstraintName("fk_team_colors_teams_id");
        });

        modelBuilder.Entity<TeamSeasonStat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_team_season_stats");

            entity.ToTable("team_season_stats");

            entity.HasIndex(e => e.TeamId, "ix_team_season_stats_team_id");

            entity.HasIndex(e => new { e.TeamId, e.SeasonYear, e.SeasonType }, "ix_team_season_stats_team_id_season_year_season_type").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DefAssists).HasColumnName("def_assists");
            entity.Property(e => e.DefForcedFumbles).HasColumnName("def_forced_fumbles");
            entity.Property(e => e.DefInterceptions).HasColumnName("def_interceptions");
            entity.Property(e => e.DefPassesDefended).HasColumnName("def_passes_defended");
            entity.Property(e => e.DefQbHits).HasColumnName("def_qb_hits");
            entity.Property(e => e.DefSacks).HasColumnName("def_sacks");
            entity.Property(e => e.DefTackles).HasColumnName("def_tackles");
            entity.Property(e => e.FgAtt).HasColumnName("fg_att");
            entity.Property(e => e.FgLong).HasColumnName("fg_long");
            entity.Property(e => e.FgMade).HasColumnName("fg_made");
            entity.Property(e => e.GamesPlayed).HasColumnName("games_played");
            entity.Property(e => e.PassAtt).HasColumnName("pass_att");
            entity.Property(e => e.PassCmp).HasColumnName("pass_cmp");
            entity.Property(e => e.PassInt).HasColumnName("pass_int");
            entity.Property(e => e.PassRating).HasColumnName("pass_rating");
            entity.Property(e => e.PassSackYds).HasColumnName("pass_sack_yds");
            entity.Property(e => e.PassSacks).HasColumnName("pass_sacks");
            entity.Property(e => e.PassTd).HasColumnName("pass_td");
            entity.Property(e => e.PassYds).HasColumnName("pass_yds");
            entity.Property(e => e.PuntAtt).HasColumnName("punt_att");
            entity.Property(e => e.PuntAvg).HasColumnName("punt_avg");
            entity.Property(e => e.PuntYds).HasColumnName("punt_yds");
            entity.Property(e => e.RecAvg).HasColumnName("rec_avg");
            entity.Property(e => e.RecFumbles).HasColumnName("rec_fumbles");
            entity.Property(e => e.RecLong).HasColumnName("rec_long");
            entity.Property(e => e.RecReceptions).HasColumnName("rec_receptions");
            entity.Property(e => e.RecTargets).HasColumnName("rec_targets");
            entity.Property(e => e.RecTd).HasColumnName("rec_td");
            entity.Property(e => e.RecYds).HasColumnName("rec_yds");
            entity.Property(e => e.RushAtt).HasColumnName("rush_att");
            entity.Property(e => e.RushAvg).HasColumnName("rush_avg");
            entity.Property(e => e.RushFumbles).HasColumnName("rush_fumbles");
            entity.Property(e => e.RushLong).HasColumnName("rush_long");
            entity.Property(e => e.RushTd).HasColumnName("rush_td");
            entity.Property(e => e.RushYds).HasColumnName("rush_yds");
            entity.Property(e => e.SeasonType).HasColumnName("season_type");
            entity.Property(e => e.SeasonYear).HasColumnName("season_year");
            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.XpAtt).HasColumnName("xp_att");
            entity.Property(e => e.XpMade).HasColumnName("xp_made");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamSeasonStats)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("fk_team_season_stats_teams_team_id");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_venues");

            entity.ToTable("venues");

            entity.HasIndex(e => e.SrId, "ix_venues_sr_id").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.Lat).HasColumnName("lat");
            entity.Property(e => e.Lng).HasColumnName("lng");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.RoofType).HasColumnName("roof_type");
            entity.Property(e => e.SrId).HasColumnName("sr_id");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.Surface).HasColumnName("surface");
            entity.Property(e => e.Zip).HasColumnName("zip");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
