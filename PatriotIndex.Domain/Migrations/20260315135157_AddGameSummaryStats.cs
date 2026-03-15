using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddGameSummaryStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "def_assists",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "def_forced_fumbles",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "def_interceptions",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "def_passes_defended",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "def_qb_hits",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "def_sacks",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "def_tackles",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "fg_att",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "fg_long",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "fg_made",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "pass_att",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "pass_cmp",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "pass_int",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "pass_rating",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "pass_sack_yds",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "pass_sacks",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "pass_td",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "pass_yds",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "punt_att",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "punt_avg",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "punt_yds",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rec_avg",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rec_fumbles",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rec_long",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rec_receptions",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rec_targets",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rec_td",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rec_yds",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rush_att",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rush_avg",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rush_fumbles",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rush_long",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rush_td",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "rush_yds",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "xp_att",
                table: "player_game_stats");

            migrationBuilder.DropColumn(
                name: "xp_made",
                table: "player_game_stats");

            migrationBuilder.AddColumn<string>(
                name: "stats",
                table: "player_game_stats",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.CreateTable(
                name: "team_game_stats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_home = table.Column<bool>(type: "boolean", nullable: false),
                    stats = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_team_game_stats", x => x.id);
                    table.ForeignKey(
                        name: "fk_team_game_stats_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_team_game_stats_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_team_game_stats_game_id",
                table: "team_game_stats",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_team_game_stats_game_id_team_id",
                table: "team_game_stats",
                columns: new[] { "game_id", "team_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_team_game_stats_team_id",
                table: "team_game_stats",
                column: "team_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "team_game_stats");

            migrationBuilder.DropColumn(
                name: "stats",
                table: "player_game_stats");

            migrationBuilder.AddColumn<int>(
                name: "def_assists",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_forced_fumbles",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_interceptions",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_passes_defended",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_qb_hits",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "def_sacks",
                table: "player_game_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "def_tackles",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fg_att",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fg_long",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fg_made",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_att",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_cmp",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_int",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "pass_rating",
                table: "player_game_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "pass_sack_yds",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_sacks",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_td",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_yds",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "punt_att",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "punt_avg",
                table: "player_game_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "punt_yds",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "rec_avg",
                table: "player_game_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "rec_fumbles",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_long",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_receptions",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_targets",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_td",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_yds",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_att",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "rush_avg",
                table: "player_game_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "rush_fumbles",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_long",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_td",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_yds",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "xp_att",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "xp_made",
                table: "player_game_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
