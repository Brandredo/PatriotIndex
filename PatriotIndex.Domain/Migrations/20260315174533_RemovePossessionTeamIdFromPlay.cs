using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemovePossessionTeamIdFromPlay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_plays_teams_possession_team_id",
                table: "plays");

            migrationBuilder.DropIndex(
                name: "ix_plays_possession_team_id",
                table: "plays");

            migrationBuilder.DropColumn(
                name: "possession_team_id",
                table: "plays");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "possession_team_id",
                table: "plays",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_plays_possession_team_id",
                table: "plays",
                column: "possession_team_id");

            migrationBuilder.AddForeignKey(
                name: "fk_plays_teams_possession_team_id",
                table: "plays",
                column: "possession_team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
