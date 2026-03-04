using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class HybridPlayModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pbp_event_statistics");

            migrationBuilder.DropTable(
                name: "play_statistics");

            migrationBuilder.DropTable(
                name: "pbp_drive_events");

            migrationBuilder.CreateTable(
                name: "game_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    drive_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_id = table.Column<Guid>(type: "uuid", nullable: true),
                    event_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sequence = table.Column<long>(type: "bigint", nullable: false),
                    clock = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_game_events_drives_drive_id",
                        column: x => x.drive_id,
                        principalTable: "drives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_events_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_events_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "plays",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    drive_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_id = table.Column<Guid>(type: "uuid", nullable: true),
                    play_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sequence = table.Column<long>(type: "bigint", nullable: false),
                    clock = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    wall_clock = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    down = table.Column<int>(type: "integer", nullable: true),
                    distance = table.Column<int>(type: "integer", nullable: true),
                    possession_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    home_points = table.Column<int>(type: "integer", nullable: false),
                    away_points = table.Column<int>(type: "integer", nullable: false),
                    qb_snap = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    huddle = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    men_in_box = table.Column<int>(type: "integer", nullable: true),
                    left_tight_ends = table.Column<int>(type: "integer", nullable: true),
                    right_tight_ends = table.Column<int>(type: "integer", nullable: true),
                    hash_mark = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    blitz = table.Column<bool>(type: "boolean", nullable: true),
                    play_action = table.Column<bool>(type: "boolean", nullable: true),
                    run_pass_option = table.Column<bool>(type: "boolean", nullable: true),
                    screen_pass = table.Column<bool>(type: "boolean", nullable: true),
                    fake_punt = table.Column<bool>(type: "boolean", nullable: true),
                    fake_field_goal = table.Column<bool>(type: "boolean", nullable: true),
                    play_direction = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    pass_route = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    start_clock = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    start_down = table.Column<int>(type: "integer", nullable: true),
                    start_yfd = table.Column<int>(type: "integer", nullable: false),
                    start_yardline = table.Column<int>(type: "integer", nullable: false),
                    start_yardline_team = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    start_possession_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    end_clock = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    end_down = table.Column<int>(type: "integer", nullable: true),
                    end_yfd = table.Column<int>(type: "integer", nullable: false),
                    end_yardline = table.Column<int>(type: "integer", nullable: false),
                    end_yardline_team = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    end_possession_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    details = table.Column<string>(type: "jsonb", nullable: true),
                    statistics = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_plays", x => x.id);
                    table.ForeignKey(
                        name: "fk_plays_drives_drive_id",
                        column: x => x.drive_id,
                        principalTable: "drives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_plays_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_plays_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_plays_teams_end_possession_team_id",
                        column: x => x.end_possession_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_plays_teams_possession_team_id",
                        column: x => x.possession_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_plays_teams_start_possession_team_id",
                        column: x => x.start_possession_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_game_events_drive_id",
                table: "game_events",
                column: "drive_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_events_game_id",
                table: "game_events",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_events_game_id_event_type",
                table: "game_events",
                columns: new[] { "game_id", "event_type" });

            migrationBuilder.CreateIndex(
                name: "ix_game_events_period_id",
                table: "game_events",
                column: "period_id");

            migrationBuilder.CreateIndex(
                name: "ix_plays_drive_id",
                table: "plays",
                column: "drive_id");

            migrationBuilder.CreateIndex(
                name: "ix_plays_end_possession_team_id",
                table: "plays",
                column: "end_possession_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_plays_game_id",
                table: "plays",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_plays_game_id_down",
                table: "plays",
                columns: new[] { "game_id", "down" });

            migrationBuilder.CreateIndex(
                name: "ix_plays_game_id_play_type",
                table: "plays",
                columns: new[] { "game_id", "play_type" });

            migrationBuilder.CreateIndex(
                name: "ix_plays_period_id",
                table: "plays",
                column: "period_id");

            migrationBuilder.CreateIndex(
                name: "ix_plays_possession_team_id",
                table: "plays",
                column: "possession_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_plays_start_possession_team_id",
                table: "plays",
                column: "start_possession_team_id");

            migrationBuilder.Sql(
                "CREATE INDEX ix_plays_statistics_gin ON plays USING GIN (statistics jsonb_path_ops);");

            migrationBuilder.Sql(
                "CREATE INDEX ix_plays_details_gin ON plays USING GIN (details jsonb_path_ops);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS ix_plays_statistics_gin;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS ix_plays_details_gin;");

            migrationBuilder.DropTable(
                name: "game_events");

            migrationBuilder.DropTable(
                name: "plays");

            migrationBuilder.CreateTable(
                name: "pbp_drive_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    drive_id = table.Column<Guid>(type: "uuid", nullable: false),
                    end_possession_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_possession_team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    away_score = table.Column<int>(type: "integer", nullable: false),
                    blitz = table.Column<bool>(type: "boolean", nullable: true),
                    clock = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    end_clock = table.Column<string>(type: "text", nullable: true),
                    end_down = table.Column<int>(type: "integer", nullable: true),
                    end_location_yard_line = table.Column<int>(type: "integer", nullable: true),
                    end_yards_to_gain = table.Column<int>(type: "integer", nullable: true),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    fake_field_goal = table.Column<bool>(type: "boolean", nullable: true),
                    fake_punt = table.Column<bool>(type: "boolean", nullable: true),
                    hash_mark = table.Column<string>(type: "text", nullable: true),
                    home_score = table.Column<int>(type: "integer", nullable: false),
                    huddle = table.Column<string>(type: "text", nullable: true),
                    left_tight_ends = table.Column<int>(type: "integer", nullable: true),
                    men_in_box = table.Column<int>(type: "integer", nullable: true),
                    pass_route = table.Column<string>(type: "text", nullable: true),
                    play_action = table.Column<bool>(type: "boolean", nullable: true),
                    play_direction = table.Column<string>(type: "text", nullable: true),
                    play_type = table.Column<string>(type: "text", nullable: true),
                    players_rushed = table.Column<int>(type: "integer", nullable: true),
                    pocket_location = table.Column<string>(type: "text", nullable: true),
                    qb_snap = table.Column<string>(type: "text", nullable: true),
                    right_tight_ends = table.Column<int>(type: "integer", nullable: true),
                    run_pass_option = table.Column<bool>(type: "boolean", nullable: true),
                    screen_pass = table.Column<bool>(type: "boolean", nullable: true),
                    sequence = table.Column<decimal>(type: "numeric", nullable: false),
                    start_clock = table.Column<string>(type: "text", nullable: true),
                    start_down = table.Column<int>(type: "integer", nullable: true),
                    start_location_yard_line = table.Column<int>(type: "integer", nullable: true),
                    start_yards_to_gain = table.Column<int>(type: "integer", nullable: true),
                    wall_clock = table.Column<string>(type: "text", nullable: false)
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
                        name: "fk_pbp_drive_events_periods_period_id",
                        column: x => x.period_id,
                        principalTable: "periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pbp_drive_events_teams_end_possession_team_id",
                        column: x => x.end_possession_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pbp_drive_events_teams_start_possession_team_id",
                        column: x => x.start_possession_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pbp_event_statistics",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    stat_type = table.Column<string>(type: "text", nullable: false),
                    drive_event_id = table.Column<Guid>(type: "uuid", nullable: true),
                    player_id = table.Column<Guid>(type: "uuid", nullable: true),
                    team_id = table.Column<Guid>(type: "uuid", nullable: true),
                    attempt = table.Column<int>(type: "integer", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    complete = table.Column<int>(type: "integer", nullable: true),
                    extra_data = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    first_down = table.Column<int>(type: "integer", nullable: true),
                    fumble = table.Column<int>(type: "integer", nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: false),
                    interception = table.Column<int>(type: "integer", nullable: true),
                    penalty = table.Column<int>(type: "integer", nullable: true),
                    penalty_yards = table.Column<int>(type: "integer", nullable: true),
                    return_yards = table.Column<int>(type: "integer", nullable: true),
                    sack = table.Column<int>(type: "integer", nullable: true),
                    touchback = table.Column<int>(type: "integer", nullable: true),
                    touchdown = table.Column<int>(type: "integer", nullable: true),
                    yards = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "play_statistics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    play_id = table.Column<Guid>(type: "uuid", nullable: false),
                    stat_type = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    player_id = table.Column<string>(type: "text", nullable: true),
                    player_jersey = table.Column<string>(type: "text", nullable: true),
                    player_name = table.Column<string>(type: "text", nullable: true),
                    player_position = table.Column<string>(type: "text", nullable: true),
                    player_sr_id = table.Column<string>(type: "text", nullable: true),
                    team_alias = table.Column<string>(type: "text", nullable: true),
                    team_id = table.Column<string>(type: "text", nullable: true),
                    team_market = table.Column<string>(type: "text", nullable: true),
                    team_name = table.Column<string>(type: "text", nullable: true),
                    team_sr_id = table.Column<string>(type: "text", nullable: true),
                    blocks = table.Column<int>(type: "integer", nullable: true),
                    block_play_stat_category = table.Column<string>(type: "text", nullable: true),
                    conversion_play_stat_attempt = table.Column<int>(type: "integer", nullable: true),
                    conversion_play_stat_category = table.Column<string>(type: "text", nullable: true),
                    conversion_play_stat_complete = table.Column<int>(type: "integer", nullable: true),
                    conversion_play_stat_safety = table.Column<int>(type: "integer", nullable: true),
                    defense_conversion_play_stat_attempt = table.Column<int>(type: "integer", nullable: true),
                    defense_conversion_play_stat_category = table.Column<string>(type: "text", nullable: true),
                    defense_conversion_play_stat_complete = table.Column<int>(type: "integer", nullable: true),
                    assist = table.Column<int>(type: "integer", nullable: true),
                    assisted_sack = table.Column<double>(type: "double precision", nullable: true),
                    assisted_tackle = table.Column<int>(type: "integer", nullable: true),
                    assisted_tackle_for_loss = table.Column<double>(type: "double precision", nullable: true),
                    batted_pass = table.Column<int>(type: "integer", nullable: true),
                    defense_play_stat_blitz = table.Column<int>(type: "integer", nullable: true),
                    defense_play_stat_category = table.Column<string>(type: "text", nullable: true),
                    def_comp = table.Column<int>(type: "integer", nullable: true),
                    def_target = table.Column<int>(type: "integer", nullable: true),
                    forced_fumble = table.Column<int>(type: "integer", nullable: true),
                    fumble_recovery = table.Column<int>(type: "integer", nullable: true),
                    defense_play_stat_hurry = table.Column<int>(type: "integer", nullable: true),
                    interception = table.Column<int>(type: "integer", nullable: true),
                    interception_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    interception_yards = table.Column<int>(type: "integer", nullable: true),
                    defense_play_stat_knockdown = table.Column<int>(type: "integer", nullable: true),
                    misc_assist = table.Column<int>(type: "integer", nullable: true),
                    misc_forced_fumble = table.Column<int>(type: "integer", nullable: true),
                    misc_fumble_recovery = table.Column<int>(type: "integer", nullable: true),
                    misc_tackle = table.Column<int>(type: "integer", nullable: true),
                    missed_tackle = table.Column<int>(type: "integer", nullable: true),
                    defense_play_stat_nullified = table.Column<bool>(type: "boolean", nullable: true),
                    pass_defended = table.Column<int>(type: "integer", nullable: true),
                    primary = table.Column<int>(type: "integer", nullable: true),
                    qb_hit = table.Column<int>(type: "integer", nullable: true),
                    defense_play_stat_sack = table.Column<double>(type: "double precision", nullable: true),
                    defense_play_stat_sack_yards = table.Column<double>(type: "double precision", nullable: true),
                    defense_play_stat_safety = table.Column<int>(type: "integer", nullable: true),
                    sp_assist = table.Column<int>(type: "integer", nullable: true),
                    sp_block = table.Column<int>(type: "integer", nullable: true),
                    sp_forced_fumble = table.Column<int>(type: "integer", nullable: true),
                    sp_fumble_recovery = table.Column<int>(type: "integer", nullable: true),
                    sp_tackle = table.Column<int>(type: "integer", nullable: true),
                    tackle = table.Column<int>(type: "integer", nullable: true),
                    tackle_for_loss = table.Column<double>(type: "double precision", nullable: true),
                    tackle_for_loss_yards = table.Column<double>(type: "double precision", nullable: true),
                    down_conversion_play_stat_attempt = table.Column<int>(type: "integer", nullable: true),
                    down_conversion_play_stat_complete = table.Column<int>(type: "integer", nullable: true),
                    down = table.Column<int>(type: "integer", nullable: true),
                    aborted = table.Column<int>(type: "integer", nullable: true),
                    extra_point_play_stat_attempt = table.Column<int>(type: "integer", nullable: true),
                    extra_point_play_stat_blocked = table.Column<int>(type: "integer", nullable: true),
                    extra_point_play_stat_made = table.Column<int>(type: "integer", nullable: true),
                    extra_point_play_stat_missed = table.Column<int>(type: "integer", nullable: true),
                    extra_point_play_stat_returned = table.Column<int>(type: "integer", nullable: true),
                    extra_point_play_stat_safety = table.Column<int>(type: "integer", nullable: true),
                    field_goal_play_stat_attempt = table.Column<int>(type: "integer", nullable: true),
                    field_goal_play_stat_attempt_yards = table.Column<int>(type: "integer", nullable: true),
                    blocked = table.Column<int>(type: "integer", nullable: true),
                    field_goal_play_stat_inside20 = table.Column<int>(type: "integer", nullable: true),
                    made = table.Column<int>(type: "integer", nullable: true),
                    missed = table.Column<int>(type: "integer", nullable: true),
                    field_goal_play_stat_nullified = table.Column<bool>(type: "boolean", nullable: true),
                    field_goal_play_stat_returned = table.Column<int>(type: "integer", nullable: true),
                    field_goal_play_stat_yards = table.Column<int>(type: "integer", nullable: true),
                    category = table.Column<string>(type: "text", nullable: true),
                    pass = table.Column<int>(type: "integer", nullable: true),
                    penalty = table.Column<int>(type: "integer", nullable: true),
                    rush = table.Column<int>(type: "integer", nullable: true),
                    total = table.Column<int>(type: "integer", nullable: true),
                    end_zone_recovery_tds = table.Column<int>(type: "integer", nullable: true),
                    forced_fumbles = table.Column<int>(type: "integer", nullable: true),
                    fumbles = table.Column<int>(type: "integer", nullable: true),
                    lost_fumbles = table.Column<int>(type: "integer", nullable: true),
                    fumble_play_stat_nullified = table.Column<bool>(type: "boolean", nullable: true),
                    opp_recoveries = table.Column<int>(type: "integer", nullable: true),
                    opp_recovery_tds = table.Column<int>(type: "integer", nullable: true),
                    opp_recovery_yards = table.Column<int>(type: "integer", nullable: true),
                    fumble_play_stat_out_of_bounds = table.Column<int>(type: "integer", nullable: true),
                    own_recoveries = table.Column<int>(type: "integer", nullable: true),
                    own_recovery_tds = table.Column<int>(type: "integer", nullable: true),
                    own_recovery_yards = table.Column<int>(type: "integer", nullable: true),
                    play_category = table.Column<string>(type: "text", nullable: true),
                    longest = table.Column<int>(type: "integer", nullable: true),
                    int_return_play_stat_returns = table.Column<int>(type: "integer", nullable: true),
                    int_return_play_stat_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    int_return_play_stat_yards = table.Column<int>(type: "integer", nullable: true),
                    kick_play_stat_attempt = table.Column<int>(type: "integer", nullable: true),
                    endzone = table.Column<int>(type: "integer", nullable: true),
                    kick_play_stat_inside20 = table.Column<int>(type: "integer", nullable: true),
                    kick_play_stat_net_yards = table.Column<int>(type: "integer", nullable: true),
                    kick_play_stat_nullified = table.Column<bool>(type: "boolean", nullable: true),
                    onside_attempt = table.Column<int>(type: "integer", nullable: true),
                    onside_success = table.Column<int>(type: "integer", nullable: true),
                    out_of_bounds = table.Column<int>(type: "integer", nullable: true),
                    own_recovery = table.Column<int>(type: "integer", nullable: true),
                    own_recovery_touchdown = table.Column<int>(type: "integer", nullable: true),
                    returned = table.Column<int>(type: "integer", nullable: true),
                    squib_kick = table.Column<int>(type: "integer", nullable: true),
                    touchback = table.Column<int>(type: "integer", nullable: true),
                    kick_play_stat_yards = table.Column<int>(type: "integer", nullable: true),
                    blk_fg_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    blk_punt_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    ez_rec_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    fg_return_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    longest_touchdown = table.Column<int>(type: "integer", nullable: true),
                    returns = table.Column<int>(type: "integer", nullable: true),
                    misc_return_play_stat_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    misc_return_play_stat_yards = table.Column<int>(type: "integer", nullable: true),
                    air_yards = table.Column<int>(type: "integer", nullable: true),
                    attempt = table.Column<int>(type: "integer", nullable: true),
                    attempt_yards = table.Column<int>(type: "integer", nullable: true),
                    batted = table.Column<int>(type: "integer", nullable: true),
                    blitz = table.Column<int>(type: "integer", nullable: true),
                    complete = table.Column<int>(type: "integer", nullable: true),
                    defended = table.Column<int>(type: "integer", nullable: true),
                    dropped = table.Column<int>(type: "integer", nullable: true),
                    pass_play_stat_first_down = table.Column<int>(type: "integer", nullable: true),
                    goal_to_go = table.Column<int>(type: "integer", nullable: true),
                    gross_yards = table.Column<int>(type: "integer", nullable: true),
                    hurry = table.Column<int>(type: "integer", nullable: true),
                    incompletion_type = table.Column<string>(type: "text", nullable: true),
                    inside20 = table.Column<int>(type: "integer", nullable: true),
                    int_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    interceptions = table.Column<int>(type: "integer", nullable: true),
                    knockdown = table.Column<int>(type: "integer", nullable: true),
                    net_yards = table.Column<int>(type: "integer", nullable: true),
                    nullified = table.Column<bool>(type: "boolean", nullable: true),
                    on_target = table.Column<int>(type: "integer", nullable: true),
                    pocket_time = table.Column<double>(type: "double precision", nullable: true),
                    poor_throw = table.Column<int>(type: "integer", nullable: true),
                    sack = table.Column<int>(type: "integer", nullable: true),
                    sack_yards = table.Column<int>(type: "integer", nullable: true),
                    safety = table.Column<int>(type: "integer", nullable: true),
                    spike = table.Column<int>(type: "integer", nullable: true),
                    throw_away = table.Column<int>(type: "integer", nullable: true),
                    touchdowns = table.Column<int>(type: "integer", nullable: true),
                    pass_play_stat_yards = table.Column<int>(type: "integer", nullable: true),
                    declined = table.Column<int>(type: "integer", nullable: true),
                    first_down = table.Column<int>(type: "integer", nullable: true),
                    no_play = table.Column<int>(type: "integer", nullable: true),
                    offsetting = table.Column<int>(type: "integer", nullable: true),
                    penalties = table.Column<int>(type: "integer", nullable: true),
                    penalty_type = table.Column<string>(type: "text", nullable: true),
                    yards = table.Column<int>(type: "integer", nullable: true),
                    punt_play_stat_attempt = table.Column<int>(type: "integer", nullable: true),
                    punt_play_stat_blocked = table.Column<int>(type: "integer", nullable: true),
                    downed = table.Column<int>(type: "integer", nullable: true),
                    end_zone = table.Column<int>(type: "integer", nullable: true),
                    fair_catch = table.Column<int>(type: "integer", nullable: true),
                    hang_time = table.Column<double>(type: "double precision", nullable: true),
                    punt_play_stat_inside20 = table.Column<int>(type: "integer", nullable: true),
                    punt_play_stat_net_yards = table.Column<int>(type: "integer", nullable: true),
                    punt_play_stat_nullified = table.Column<bool>(type: "boolean", nullable: true),
                    return_yards = table.Column<int>(type: "integer", nullable: true),
                    punt_play_stat_touchback = table.Column<int>(type: "integer", nullable: true),
                    punt_play_stat_yards = table.Column<int>(type: "integer", nullable: true),
                    receive_play_stat_air_yards = table.Column<int>(type: "integer", nullable: true),
                    broken_tackles = table.Column<int>(type: "integer", nullable: true),
                    catchable = table.Column<int>(type: "integer", nullable: true),
                    receive_play_stat_dropped = table.Column<int>(type: "integer", nullable: true),
                    receive_play_stat_first_down = table.Column<int>(type: "integer", nullable: true),
                    receive_play_stat_goal_to_go = table.Column<int>(type: "integer", nullable: true),
                    receive_play_stat_inside20 = table.Column<int>(type: "integer", nullable: true),
                    receive_play_stat_nullified = table.Column<bool>(type: "boolean", nullable: true),
                    reception = table.Column<int>(type: "integer", nullable: true),
                    redzone_target = table.Column<int>(type: "integer", nullable: true),
                    receive_play_stat_safety = table.Column<int>(type: "integer", nullable: true),
                    target = table.Column<int>(type: "integer", nullable: true),
                    receive_play_stat_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    receive_play_stat_yards = table.Column<int>(type: "integer", nullable: true),
                    yards_after_catch = table.Column<int>(type: "integer", nullable: true),
                    yards_after_contact = table.Column<int>(type: "integer", nullable: true),
                    return_play_stat_category = table.Column<string>(type: "text", nullable: true),
                    return_play_stat_downed = table.Column<int>(type: "integer", nullable: true),
                    return_play_stat_fair_catch = table.Column<int>(type: "integer", nullable: true),
                    return_play_stat_first_down = table.Column<int>(type: "integer", nullable: true),
                    lateral = table.Column<int>(type: "integer", nullable: true),
                    return_play_stat_longest = table.Column<int>(type: "integer", nullable: true),
                    return_play_stat_nullified = table.Column<bool>(type: "boolean", nullable: true),
                    return_play_stat_out_of_bounds = table.Column<int>(type: "integer", nullable: true),
                    return_play_stat_play_category = table.Column<string>(type: "text", nullable: true),
                    return_play_stat_touchback = table.Column<int>(type: "integer", nullable: true),
                    return_play_stat_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    return_play_stat_yards = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_attempt = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_broken_tackles = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_first_down = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_goal_to_go = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_inside20 = table.Column<int>(type: "integer", nullable: true),
                    kneel_down = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_lateral = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_nullified = table.Column<bool>(type: "boolean", nullable: true),
                    rush_play_stat_safety = table.Column<int>(type: "integer", nullable: true),
                    scramble = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_tackle_for_loss = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_tackle_for_loss_yards = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_touchdowns = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_yards = table.Column<int>(type: "integer", nullable: true),
                    rush_play_stat_yards_after_contact = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_play_statistics", x => x.id);
                    table.ForeignKey(
                        name: "fk_play_statistics_pbp_drive_events_play_id",
                        column: x => x.play_id,
                        principalTable: "pbp_drive_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_drive_id",
                table: "pbp_drive_events",
                column: "drive_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_end_possession_team_id",
                table: "pbp_drive_events",
                column: "end_possession_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_period_id",
                table: "pbp_drive_events",
                column: "period_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_sequence",
                table: "pbp_drive_events",
                column: "sequence");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_start_possession_team_id",
                table: "pbp_drive_events",
                column: "start_possession_team_id");

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
                name: "ix_play_statistics_play_id_stat_type",
                table: "play_statistics",
                columns: new[] { "play_id", "stat_type" });
        }
    }
}
