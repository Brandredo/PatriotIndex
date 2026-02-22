using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Two : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_coin_tosses_games_game_id",
                table: "coin_tosses");

            migrationBuilder.RenameColumn(
                name: "game_id",
                table: "coin_tosses",
                newName: "period_id1");

            migrationBuilder.RenameIndex(
                name: "ix_coin_tosses_game_id",
                table: "coin_tosses",
                newName: "ix_coin_tosses_period_id1");

            migrationBuilder.AddColumn<Guid>(
                name: "game_id1",
                table: "periods",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "period_id1",
                table: "drives",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_periods_game_id1",
                table: "periods",
                column: "game_id1");

            migrationBuilder.CreateIndex(
                name: "ix_drives_period_id1",
                table: "drives",
                column: "period_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_coin_tosses_periods_period_id1",
                table: "coin_tosses",
                column: "period_id1",
                principalTable: "periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_drives_periods_period_id1",
                table: "drives",
                column: "period_id1",
                principalTable: "periods",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_periods_games_game_id1",
                table: "periods",
                column: "game_id1",
                principalTable: "games",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_coin_tosses_periods_period_id1",
                table: "coin_tosses");

            migrationBuilder.DropForeignKey(
                name: "fk_drives_periods_period_id1",
                table: "drives");

            migrationBuilder.DropForeignKey(
                name: "fk_periods_games_game_id1",
                table: "periods");

            migrationBuilder.DropIndex(
                name: "ix_periods_game_id1",
                table: "periods");

            migrationBuilder.DropIndex(
                name: "ix_drives_period_id1",
                table: "drives");

            migrationBuilder.DropColumn(
                name: "game_id1",
                table: "periods");

            migrationBuilder.DropColumn(
                name: "period_id1",
                table: "drives");

            migrationBuilder.RenameColumn(
                name: "period_id1",
                table: "coin_tosses",
                newName: "game_id");

            migrationBuilder.RenameIndex(
                name: "ix_coin_tosses_period_id1",
                table: "coin_tosses",
                newName: "ix_coin_tosses_game_id");

            migrationBuilder.AddForeignKey(
                name: "fk_coin_tosses_games_game_id",
                table: "coin_tosses",
                column: "game_id",
                principalTable: "games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
