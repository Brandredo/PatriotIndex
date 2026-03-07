using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class SeasonalStatsRebuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "def_assists",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "def_forced_fumbles",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "def_interceptions",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "def_passes_defended",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "def_qb_hits",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "def_sacks",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "def_tackles",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "fg_att",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "fg_long",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "fg_made",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_att",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_cmp",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_int",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_rating",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_sack_yds",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_sacks",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_td",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_yds",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "punt_att",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "punt_avg",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "punt_yds",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_avg",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_fumbles",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_long",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_receptions",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_targets",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_td",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_yds",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_att",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_avg",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_fumbles",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_long",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_td",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_yds",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "xp_att",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "xp_made",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "def_assists",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "def_forced_fumbles",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "def_interceptions",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "def_passes_defended",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "def_qb_hits",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "def_sacks",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "def_tackles",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "fg_att",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "fg_long",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "fg_made",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_att",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_cmp",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_int",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_rating",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_sack_yds",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_sacks",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_td",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "pass_yds",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "punt_att",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "punt_avg",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "punt_yds",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_avg",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_fumbles",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_long",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_receptions",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_targets",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_td",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rec_yds",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_att",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_avg",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_fumbles",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_long",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_td",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "rush_yds",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "xp_att",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "xp_made",
                table: "player_season_stats");

            migrationBuilder.AddColumn<string>(
                name: "opponents",
                table: "team_season_stats",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<string>(
                name: "record",
                table: "team_season_stats",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<string>(
                name: "season_sr_id",
                table: "team_season_stats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "player_sr_id",
                table: "player_season_stats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "season_sr_id",
                table: "player_season_stats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "stats",
                table: "player_season_stats",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "opponents",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "record",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "season_sr_id",
                table: "team_season_stats");

            migrationBuilder.DropColumn(
                name: "player_sr_id",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "season_sr_id",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "stats",
                table: "player_season_stats");

            migrationBuilder.AddColumn<int>(
                name: "def_assists",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_forced_fumbles",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_interceptions",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_passes_defended",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_qb_hits",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "def_sacks",
                table: "team_season_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "def_tackles",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fg_att",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fg_long",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fg_made",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_att",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_cmp",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_int",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "pass_rating",
                table: "team_season_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "pass_sack_yds",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_sacks",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_td",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_yds",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "punt_att",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "punt_avg",
                table: "team_season_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "punt_yds",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "rec_avg",
                table: "team_season_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "rec_fumbles",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_long",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_receptions",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_targets",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_td",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_yds",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_att",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "rush_avg",
                table: "team_season_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "rush_fumbles",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_long",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_td",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_yds",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "xp_att",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "xp_made",
                table: "team_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_assists",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_forced_fumbles",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_interceptions",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_passes_defended",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "def_qb_hits",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "def_sacks",
                table: "player_season_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "def_tackles",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fg_att",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fg_long",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fg_made",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_att",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_cmp",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_int",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "pass_rating",
                table: "player_season_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "pass_sack_yds",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_sacks",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_td",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pass_yds",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "punt_att",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "punt_avg",
                table: "player_season_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "punt_yds",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "rec_avg",
                table: "player_season_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "rec_fumbles",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_long",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_receptions",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_targets",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_td",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rec_yds",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_att",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "rush_avg",
                table: "player_season_stats",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "rush_fumbles",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_long",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_td",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rush_yds",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "xp_att",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "xp_made",
                table: "player_season_stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
