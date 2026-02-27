using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class latestupdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_periods_games_game_id1",
                table: "periods");

            migrationBuilder.DropIndex(
                name: "ix_periods_game_id_number",
                table: "periods");

            migrationBuilder.DropIndex(
                name: "ix_periods_game_id1",
                table: "periods");

            migrationBuilder.DropColumn(
                name: "game_id1",
                table: "periods");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "game_id1",
                table: "periods",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_periods_game_id_number",
                table: "periods",
                columns: new[] { "game_id", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_periods_game_id1",
                table: "periods",
                column: "game_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_periods_games_game_id1",
                table: "periods",
                column: "game_id1",
                principalTable: "games",
                principalColumn: "id");
        }
    }
}
