using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Relationshipupdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE pbp_drive_events DROP CONSTRAINT IF EXISTS fk_pbp_drive_events_teams_end_team_id;");
            migrationBuilder.Sql("ALTER TABLE pbp_drive_events DROP CONSTRAINT IF EXISTS fk_pbp_drive_events_teams_start_team_id;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS ix_pbp_drive_events_end_team_id;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS ix_pbp_drive_events_start_team_id;");
            migrationBuilder.Sql("ALTER TABLE pbp_drive_events DROP COLUMN IF EXISTS end_team_id;");
            migrationBuilder.Sql("ALTER TABLE pbp_drive_events DROP COLUMN IF EXISTS start_team_id;");
            migrationBuilder.Sql("ALTER TABLE pbp_drive_events DROP COLUMN IF EXISTS drive_type;");
            migrationBuilder.Sql("ALTER TABLE drives DROP COLUMN IF EXISTS defensive_start_points;");
            migrationBuilder.Sql("ALTER TABLE drives DROP COLUMN IF EXISTS offensive_start_points;");
            migrationBuilder.Sql("ALTER TABLE drives DROP COLUMN IF EXISTS period_number;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "drive_type",
                table: "pbp_drive_events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "end_team_id",
                table: "pbp_drive_events",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "start_team_id",
                table: "pbp_drive_events",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "defensive_start_points",
                table: "drives",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "offensive_start_points",
                table: "drives",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "period_number",
                table: "drives",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_end_team_id",
                table: "pbp_drive_events",
                column: "end_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_start_team_id",
                table: "pbp_drive_events",
                column: "start_team_id");

            migrationBuilder.AddForeignKey(
                name: "fk_pbp_drive_events_teams_end_team_id",
                table: "pbp_drive_events",
                column: "end_team_id",
                principalTable: "teams",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_pbp_drive_events_teams_start_team_id",
                table: "pbp_drive_events",
                column: "start_team_id",
                principalTable: "teams",
                principalColumn: "id");
        }
    }
}
