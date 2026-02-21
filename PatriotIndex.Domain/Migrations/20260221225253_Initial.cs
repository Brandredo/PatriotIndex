using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "conferences",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conferences", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sync_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    record_count = table.Column<int>(type: "integer", nullable: false),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    raw_response = table.Column<JsonDocument>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sync_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venues",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    zip = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    capacity = table.Column<int>(type: "integer", nullable: true),
                    surface = table.Column<string>(type: "text", nullable: true),
                    roof_type = table.Column<string>(type: "text", nullable: true),
                    sr_id = table.Column<string>(type: "text", nullable: true),
                    lat = table.Column<string>(type: "text", nullable: true),
                    lng = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_venues", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "divisions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: false),
                    conference_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_divisions", x => x.id);
                    table.ForeignKey(
                        name: "fk_divisions_conferences_conference_id",
                        column: x => x.conference_id,
                        principalTable: "conferences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    market = table.Column<string>(type: "text", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: false),
                    sr_id = table.Column<string>(type: "text", nullable: true),
                    founded = table.Column<short>(type: "smallint", nullable: true),
                    owner = table.Column<string>(type: "text", nullable: true),
                    general_manager = table.Column<string>(type: "text", nullable: true),
                    president = table.Column<string>(type: "text", nullable: true),
                    mascot = table.Column<string>(type: "text", nullable: true),
                    venue_id = table.Column<Guid>(type: "uuid", nullable: true),
                    division_id = table.Column<Guid>(type: "uuid", nullable: true),
                    secondary_color = table.Column<string>(type: "text", nullable: true),
                    championships_won = table.Column<int>(type: "integer", nullable: true),
                    conference_titles = table.Column<int>(type: "integer", nullable: true),
                    division_titles = table.Column<int>(type: "integer", nullable: true),
                    playoff_appearances = table.Column<int>(type: "integer", nullable: true),
                    championship_seasons = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teams", x => x.id);
                    table.ForeignKey(
                        name: "fk_teams_divisions_division_id",
                        column: x => x.division_id,
                        principalTable: "divisions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_teams_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "coaches",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    position = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coaches", x => x.id);
                    table.ForeignKey(
                        name: "fk_coaches_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sr_id = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    scheduled = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    attendance = table.Column<int>(type: "integer", nullable: true),
                    game_type = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    duration = table.Column<string>(type: "text", nullable: true),
                    season_year = table.Column<int>(type: "integer", nullable: true),
                    season_type = table.Column<string>(type: "text", nullable: true),
                    week_sequence = table.Column<int>(type: "integer", nullable: true),
                    week_title = table.Column<string>(type: "text", nullable: true),
                    home_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    away_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    home_points = table.Column<int>(type: "integer", nullable: true),
                    away_points = table.Column<int>(type: "integer", nullable: true),
                    venue_id = table.Column<Guid>(type: "uuid", nullable: true),
                    weather_condition = table.Column<string>(type: "text", nullable: true),
                    weather_temp = table.Column<int>(type: "integer", nullable: true),
                    weather_humidity = table.Column<int>(type: "integer", nullable: true),
                    weather_wind_speed = table.Column<int>(type: "integer", nullable: true),
                    weather_wind_direction = table.Column<string>(type: "text", nullable: true),
                    broadcast_network = table.Column<string>(type: "text", nullable: true),
                    neutral_site = table.Column<bool>(type: "boolean", nullable: false),
                    conference_game = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                    table.ForeignKey(
                        name: "fk_games_teams_away_team_id",
                        column: x => x.away_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_games_teams_home_team_id",
                        column: x => x.home_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_games_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    jersey = table.Column<string>(type: "text", nullable: true),
                    position = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    weight = table.Column<int>(type: "integer", nullable: true),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                    college = table.Column<string>(type: "text", nullable: true),
                    rookie_year = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    experience = table.Column<int>(type: "integer", nullable: true),
                    salary = table.Column<long>(type: "bigint", nullable: true),
                    sr_id = table.Column<string>(type: "text", nullable: true),
                    draft_year = table.Column<int>(type: "integer", nullable: true),
                    draft_round = table.Column<int>(type: "integer", nullable: true),
                    draft_pick = table.Column<int>(type: "integer", nullable: true),
                    draft_team_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_players", x => x.id);
                    table.ForeignKey(
                        name: "fk_players_teams_draft_team_id",
                        column: x => x.draft_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_players_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "team_season_stats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_id = table.Column<Guid>(type: "uuid", nullable: false),
                    season_year = table.Column<int>(type: "integer", nullable: false),
                    season_type = table.Column<string>(type: "text", nullable: false),
                    games_played = table.Column<int>(type: "integer", nullable: false),
                    pass_att = table.Column<int>(type: "integer", nullable: false),
                    pass_cmp = table.Column<int>(type: "integer", nullable: false),
                    pass_yds = table.Column<int>(type: "integer", nullable: false),
                    pass_td = table.Column<int>(type: "integer", nullable: false),
                    pass_int = table.Column<int>(type: "integer", nullable: false),
                    pass_rating = table.Column<double>(type: "double precision", nullable: false),
                    pass_sacks = table.Column<int>(type: "integer", nullable: false),
                    pass_sack_yds = table.Column<int>(type: "integer", nullable: false),
                    rush_att = table.Column<int>(type: "integer", nullable: false),
                    rush_yds = table.Column<int>(type: "integer", nullable: false),
                    rush_td = table.Column<int>(type: "integer", nullable: false),
                    rush_avg = table.Column<double>(type: "double precision", nullable: false),
                    rush_long = table.Column<int>(type: "integer", nullable: false),
                    rush_fumbles = table.Column<int>(type: "integer", nullable: false),
                    rec_targets = table.Column<int>(type: "integer", nullable: false),
                    rec_receptions = table.Column<int>(type: "integer", nullable: false),
                    rec_yds = table.Column<int>(type: "integer", nullable: false),
                    rec_td = table.Column<int>(type: "integer", nullable: false),
                    rec_avg = table.Column<double>(type: "double precision", nullable: false),
                    rec_long = table.Column<int>(type: "integer", nullable: false),
                    rec_fumbles = table.Column<int>(type: "integer", nullable: false),
                    def_tackles = table.Column<int>(type: "integer", nullable: false),
                    def_assists = table.Column<int>(type: "integer", nullable: false),
                    def_sacks = table.Column<double>(type: "double precision", nullable: false),
                    def_interceptions = table.Column<int>(type: "integer", nullable: false),
                    def_forced_fumbles = table.Column<int>(type: "integer", nullable: false),
                    def_passes_defended = table.Column<int>(type: "integer", nullable: false),
                    def_qb_hits = table.Column<int>(type: "integer", nullable: false),
                    fg_att = table.Column<int>(type: "integer", nullable: false),
                    fg_made = table.Column<int>(type: "integer", nullable: false),
                    fg_long = table.Column<int>(type: "integer", nullable: false),
                    xp_att = table.Column<int>(type: "integer", nullable: false),
                    xp_made = table.Column<int>(type: "integer", nullable: false),
                    punt_att = table.Column<int>(type: "integer", nullable: false),
                    punt_yds = table.Column<int>(type: "integer", nullable: false),
                    punt_avg = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_team_season_stats", x => x.id);
                    table.ForeignKey(
                        name: "fk_team_season_stats_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamColors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    primary = table.Column<string>(type: "text", nullable: false),
                    secondary = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_team_colors", x => x.id);
                    table.ForeignKey(
                        name: "fk_team_colors_teams_id",
                        column: x => x.id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "periods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    sequence = table.Column<long>(type: "bigint", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    home_score = table.Column<int>(type: "integer", nullable: false),
                    away_score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_periods", x => x.id);
                    table.ForeignKey(
                        name: "fk_periods_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_game_stats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    pass_att = table.Column<int>(type: "integer", nullable: false),
                    pass_cmp = table.Column<int>(type: "integer", nullable: false),
                    pass_yds = table.Column<int>(type: "integer", nullable: false),
                    pass_td = table.Column<int>(type: "integer", nullable: false),
                    pass_int = table.Column<int>(type: "integer", nullable: false),
                    pass_rating = table.Column<double>(type: "double precision", nullable: false),
                    pass_sacks = table.Column<int>(type: "integer", nullable: false),
                    pass_sack_yds = table.Column<int>(type: "integer", nullable: false),
                    rush_att = table.Column<int>(type: "integer", nullable: false),
                    rush_yds = table.Column<int>(type: "integer", nullable: false),
                    rush_td = table.Column<int>(type: "integer", nullable: false),
                    rush_avg = table.Column<double>(type: "double precision", nullable: false),
                    rush_long = table.Column<int>(type: "integer", nullable: false),
                    rush_fumbles = table.Column<int>(type: "integer", nullable: false),
                    rec_targets = table.Column<int>(type: "integer", nullable: false),
                    rec_receptions = table.Column<int>(type: "integer", nullable: false),
                    rec_yds = table.Column<int>(type: "integer", nullable: false),
                    rec_td = table.Column<int>(type: "integer", nullable: false),
                    rec_avg = table.Column<double>(type: "double precision", nullable: false),
                    rec_long = table.Column<int>(type: "integer", nullable: false),
                    rec_fumbles = table.Column<int>(type: "integer", nullable: false),
                    def_tackles = table.Column<int>(type: "integer", nullable: false),
                    def_assists = table.Column<int>(type: "integer", nullable: false),
                    def_sacks = table.Column<double>(type: "double precision", nullable: false),
                    def_interceptions = table.Column<int>(type: "integer", nullable: false),
                    def_forced_fumbles = table.Column<int>(type: "integer", nullable: false),
                    def_passes_defended = table.Column<int>(type: "integer", nullable: false),
                    def_qb_hits = table.Column<int>(type: "integer", nullable: false),
                    fg_att = table.Column<int>(type: "integer", nullable: false),
                    fg_made = table.Column<int>(type: "integer", nullable: false),
                    fg_long = table.Column<int>(type: "integer", nullable: false),
                    xp_att = table.Column<int>(type: "integer", nullable: false),
                    xp_made = table.Column<int>(type: "integer", nullable: false),
                    punt_att = table.Column<int>(type: "integer", nullable: false),
                    punt_yds = table.Column<int>(type: "integer", nullable: false),
                    punt_avg = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_game_stats", x => x.id);
                    table.ForeignKey(
                        name: "fk_player_game_stats_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_player_game_stats_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_player_game_stats_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "player_season_stats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    season_year = table.Column<int>(type: "integer", nullable: false),
                    season_type = table.Column<string>(type: "text", nullable: false),
                    games_played = table.Column<int>(type: "integer", nullable: false),
                    games_started = table.Column<int>(type: "integer", nullable: false),
                    pass_att = table.Column<int>(type: "integer", nullable: false),
                    pass_cmp = table.Column<int>(type: "integer", nullable: false),
                    pass_yds = table.Column<int>(type: "integer", nullable: false),
                    pass_td = table.Column<int>(type: "integer", nullable: false),
                    pass_int = table.Column<int>(type: "integer", nullable: false),
                    pass_rating = table.Column<double>(type: "double precision", nullable: false),
                    pass_sacks = table.Column<int>(type: "integer", nullable: false),
                    pass_sack_yds = table.Column<int>(type: "integer", nullable: false),
                    rush_att = table.Column<int>(type: "integer", nullable: false),
                    rush_yds = table.Column<int>(type: "integer", nullable: false),
                    rush_td = table.Column<int>(type: "integer", nullable: false),
                    rush_avg = table.Column<double>(type: "double precision", nullable: false),
                    rush_long = table.Column<int>(type: "integer", nullable: false),
                    rush_fumbles = table.Column<int>(type: "integer", nullable: false),
                    rec_targets = table.Column<int>(type: "integer", nullable: false),
                    rec_receptions = table.Column<int>(type: "integer", nullable: false),
                    rec_yds = table.Column<int>(type: "integer", nullable: false),
                    rec_td = table.Column<int>(type: "integer", nullable: false),
                    rec_avg = table.Column<double>(type: "double precision", nullable: false),
                    rec_long = table.Column<int>(type: "integer", nullable: false),
                    rec_fumbles = table.Column<int>(type: "integer", nullable: false),
                    def_tackles = table.Column<int>(type: "integer", nullable: false),
                    def_assists = table.Column<int>(type: "integer", nullable: false),
                    def_sacks = table.Column<double>(type: "double precision", nullable: false),
                    def_interceptions = table.Column<int>(type: "integer", nullable: false),
                    def_forced_fumbles = table.Column<int>(type: "integer", nullable: false),
                    def_passes_defended = table.Column<int>(type: "integer", nullable: false),
                    def_qb_hits = table.Column<int>(type: "integer", nullable: false),
                    fg_att = table.Column<int>(type: "integer", nullable: false),
                    fg_made = table.Column<int>(type: "integer", nullable: false),
                    fg_long = table.Column<int>(type: "integer", nullable: false),
                    xp_att = table.Column<int>(type: "integer", nullable: false),
                    xp_made = table.Column<int>(type: "integer", nullable: false),
                    punt_att = table.Column<int>(type: "integer", nullable: false),
                    punt_yds = table.Column<int>(type: "integer", nullable: false),
                    punt_avg = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_season_stats", x => x.id);
                    table.ForeignKey(
                        name: "fk_player_season_stats_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_player_season_stats_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "coin_tosses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    winner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    decision = table.Column<string>(type: "text", nullable: false),
                    direction = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coin_tosses", x => x.id);
                    table.ForeignKey(
                        name: "fk_coin_tosses_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_coin_tosses_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_coin_tosses_teams_winner_id",
                        column: x => x.winner_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "drives",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sequence = table.Column<int>(type: "integer", nullable: true),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_number = table.Column<int>(type: "integer", nullable: true),
                    team_sequence = table.Column<int>(type: "integer", nullable: true),
                    start_reason = table.Column<string>(type: "text", nullable: true),
                    end_reason = table.Column<string>(type: "text", nullable: true),
                    play_count = table.Column<int>(type: "integer", nullable: true),
                    duration = table.Column<string>(type: "text", nullable: true),
                    first_downs = table.Column<int>(type: "integer", nullable: true),
                    gained_yards = table.Column<int>(type: "integer", nullable: true),
                    penalty_yards = table.Column<int>(type: "integer", nullable: true),
                    net_yards = table.Column<int>(type: "integer", nullable: true),
                    start_clock = table.Column<string>(type: "text", nullable: true),
                    end_clock = table.Column<string>(type: "text", nullable: true),
                    offensive_team_id = table.Column<Guid>(type: "uuid", nullable: false),
                    defensive_team_id = table.Column<Guid>(type: "uuid", nullable: false),
                    offensive_points = table.Column<int>(type: "integer", nullable: true),
                    defensive_points = table.Column<int>(type: "integer", nullable: true),
                    first_drive_yard_line = table.Column<int>(type: "integer", nullable: true),
                    last_drive_yard_line = table.Column<int>(type: "integer", nullable: true),
                    farthest_drive_yard_line = table.Column<int>(type: "integer", nullable: true),
                    pat_points_attempted = table.Column<int>(type: "integer", nullable: true),
                    offensive_start_points = table.Column<int>(type: "integer", nullable: true),
                    defensive_start_points = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drives", x => x.id);
                    table.ForeignKey(
                        name: "fk_drives_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_drives_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_drives_teams_defensive_team_id",
                        column: x => x.defensive_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_drives_teams_offensive_team_id",
                        column: x => x.offensive_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pbp_drive_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    drive_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type = table.Column<int>(type: "integer", nullable: false),
                    drive_type = table.Column<int>(type: "integer", nullable: false),
                    sequence = table.Column<long>(type: "bigint", nullable: false),
                    clock = table.Column<string>(type: "text", nullable: false),
                    wall_clock = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    home_score = table.Column<int>(type: "integer", nullable: false),
                    away_score = table.Column<int>(type: "integer", nullable: false),
                    play_type = table.Column<int>(type: "integer", nullable: true),
                    pass_route = table.Column<string>(type: "text", nullable: false),
                    qb_snap = table.Column<string>(type: "text", nullable: true),
                    huddle = table.Column<string>(type: "text", nullable: true),
                    men_in_box = table.Column<int>(type: "integer", nullable: true),
                    left_tight_ends = table.Column<int>(type: "integer", nullable: true),
                    right_tight_ends = table.Column<int>(type: "integer", nullable: true),
                    hash_mark = table.Column<string>(type: "text", nullable: true),
                    players_rushed = table.Column<int>(type: "integer", nullable: true),
                    play_direction = table.Column<string>(type: "text", nullable: true),
                    pocket_location = table.Column<string>(type: "text", nullable: true),
                    fake_punt = table.Column<bool>(type: "boolean", nullable: false),
                    fake_field_goal = table.Column<bool>(type: "boolean", nullable: false),
                    screen_pass = table.Column<bool>(type: "boolean", nullable: false),
                    blitz = table.Column<bool>(type: "boolean", nullable: false),
                    play_action = table.Column<bool>(type: "boolean", nullable: false),
                    run_pass_option = table.Column<bool>(type: "boolean", nullable: false),
                    start_clock = table.Column<string>(type: "text", nullable: true),
                    start_down = table.Column<int>(type: "integer", nullable: false),
                    start_yards_to_gain = table.Column<int>(type: "integer", nullable: false),
                    start_location_yard_line = table.Column<int>(type: "integer", nullable: false),
                    start_possession_team_id = table.Column<Guid>(type: "uuid", nullable: false),
                    end_clock = table.Column<string>(type: "text", nullable: true),
                    end_down = table.Column<int>(type: "integer", nullable: false),
                    end_yards_to_gain = table.Column<int>(type: "integer", nullable: false),
                    end_location_yard_line = table.Column<int>(type: "integer", nullable: false),
                    end_possession_team_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    end_team_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pbp_drive_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_pbp_drive_events_drives_drive_id",
                        column: x => x.drive_id,
                        principalTable: "drives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pbp_drive_events_teams_end_possession_team_id",
                        column: x => x.end_possession_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pbp_drive_events_teams_end_team_id",
                        column: x => x.end_team_id,
                        principalTable: "teams",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_pbp_drive_events_teams_start_possession_team_id",
                        column: x => x.start_possession_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pbp_drive_events_teams_start_team_id",
                        column: x => x.start_team_id,
                        principalTable: "teams",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "pbp_event_statistics",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    stat_type = table.Column<string>(type: "text", nullable: false),
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_id = table.Column<Guid>(type: "uuid", nullable: true),
                    team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    yards = table.Column<int>(type: "integer", nullable: true),
                    attempt = table.Column<int>(type: "integer", nullable: true),
                    complete = table.Column<int>(type: "integer", nullable: true),
                    touchdown = table.Column<int>(type: "integer", nullable: true),
                    interception = table.Column<int>(type: "integer", nullable: true),
                    fumble = table.Column<int>(type: "integer", nullable: true),
                    sack = table.Column<int>(type: "integer", nullable: true),
                    first_down = table.Column<int>(type: "integer", nullable: true),
                    penalty = table.Column<int>(type: "integer", nullable: true),
                    penalty_yards = table.Column<int>(type: "integer", nullable: true),
                    return_yards = table.Column<int>(type: "integer", nullable: true),
                    touchback = table.Column<int>(type: "integer", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    extra_data = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    drive_event_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pbp_event_statistics", x => new { x.event_id, x.stat_type });
                    table.ForeignKey(
                        name: "fk_pbp_event_statistics_pbp_drive_events_drive_event_id",
                        column: x => x.drive_event_id,
                        principalTable: "pbp_drive_events",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_pbp_event_statistics_pbp_drive_events_event_id",
                        column: x => x.event_id,
                        principalTable: "pbp_drive_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pbp_event_statistics_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_pbp_event_statistics_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_coaches_team_id",
                table: "coaches",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_coin_tosses_game_id",
                table: "coin_tosses",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_coin_tosses_period_id",
                table: "coin_tosses",
                column: "period_id");

            migrationBuilder.CreateIndex(
                name: "ix_coin_tosses_winner_id",
                table: "coin_tosses",
                column: "winner_id");

            migrationBuilder.CreateIndex(
                name: "ix_conferences_alias",
                table: "conferences",
                column: "alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_divisions_conference_id",
                table: "divisions",
                column: "conference_id");

            migrationBuilder.CreateIndex(
                name: "ix_drives_defensive_team_id",
                table: "drives",
                column: "defensive_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_drives_game_id",
                table: "drives",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_drives_game_id_sequence",
                table: "drives",
                columns: new[] { "game_id", "sequence" });

            migrationBuilder.CreateIndex(
                name: "ix_drives_offensive_team_id",
                table: "drives",
                column: "offensive_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_drives_period_id",
                table: "drives",
                column: "period_id");

            migrationBuilder.CreateIndex(
                name: "ix_games_away_team_id",
                table: "games",
                column: "away_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_games_home_team_id",
                table: "games",
                column: "home_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_games_scheduled",
                table: "games",
                column: "scheduled");

            migrationBuilder.CreateIndex(
                name: "ix_games_season_year_season_type_week_sequence",
                table: "games",
                columns: new[] { "season_year", "season_type", "week_sequence" });

            migrationBuilder.CreateIndex(
                name: "ix_games_sr_id",
                table: "games",
                column: "sr_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_games_venue_id",
                table: "games",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_drive_id",
                table: "pbp_drive_events",
                column: "drive_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_end_possession_team_id",
                table: "pbp_drive_events",
                column: "end_possession_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_end_team_id",
                table: "pbp_drive_events",
                column: "end_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_sequence",
                table: "pbp_drive_events",
                column: "sequence");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_start_possession_team_id",
                table: "pbp_drive_events",
                column: "start_possession_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_start_team_id",
                table: "pbp_drive_events",
                column: "start_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_event_statistics_drive_event_id",
                table: "pbp_event_statistics",
                column: "drive_event_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_event_statistics_player_id",
                table: "pbp_event_statistics",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_event_statistics_team_id",
                table: "pbp_event_statistics",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_periods_game_id",
                table: "periods",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_periods_game_id_number",
                table: "periods",
                columns: new[] { "game_id", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_player_game_stats_game_id",
                table: "player_game_stats",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_game_stats_player_id",
                table: "player_game_stats",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_game_stats_player_id_game_id",
                table: "player_game_stats",
                columns: new[] { "player_id", "game_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_player_game_stats_team_id",
                table: "player_game_stats",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_season_stats_player_id",
                table: "player_season_stats",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_season_stats_player_id_season_year_season_type",
                table: "player_season_stats",
                columns: new[] { "player_id", "season_year", "season_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_player_season_stats_team_id",
                table: "player_season_stats",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_players_draft_team_id",
                table: "players",
                column: "draft_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_players_sr_id",
                table: "players",
                column: "sr_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_players_team_id",
                table: "players",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_sync_logs_entity_type",
                table: "sync_logs",
                column: "entity_type");

            migrationBuilder.CreateIndex(
                name: "ix_sync_logs_started_at",
                table: "sync_logs",
                column: "started_at");

            migrationBuilder.CreateIndex(
                name: "ix_team_season_stats_team_id",
                table: "team_season_stats",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "ix_team_season_stats_team_id_season_year_season_type",
                table: "team_season_stats",
                columns: new[] { "team_id", "season_year", "season_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_teams_alias",
                table: "teams",
                column: "alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_teams_division_id",
                table: "teams",
                column: "division_id");

            migrationBuilder.CreateIndex(
                name: "ix_teams_sr_id",
                table: "teams",
                column: "sr_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_teams_venue_id",
                table: "teams",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "ix_venues_sr_id",
                table: "venues",
                column: "sr_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coaches");

            migrationBuilder.DropTable(
                name: "coin_tosses");

            migrationBuilder.DropTable(
                name: "pbp_event_statistics");

            migrationBuilder.DropTable(
                name: "player_game_stats");

            migrationBuilder.DropTable(
                name: "player_season_stats");

            migrationBuilder.DropTable(
                name: "sync_logs");

            migrationBuilder.DropTable(
                name: "team_season_stats");

            migrationBuilder.DropTable(
                name: "TeamColors");

            migrationBuilder.DropTable(
                name: "pbp_drive_events");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "drives");

            migrationBuilder.DropTable(
                name: "periods");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "divisions");

            migrationBuilder.DropTable(
                name: "venues");

            migrationBuilder.DropTable(
                name: "conferences");
        }
    }
}
