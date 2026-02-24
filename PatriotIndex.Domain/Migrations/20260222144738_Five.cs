using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Five : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_drives_games_game_id",
                table: "drives");

            migrationBuilder.DropIndex(
                name: "ix_drives_game_id_sequence",
                table: "drives");

            migrationBuilder.DropIndex(
                name: "ix_drives_period_id",
                table: "drives");

            // done via script
            // migrationBuilder.AlterColumn<long>(
            //     name: "id",
            //     table: "pbp_event_statistics",
            //     type: "bigint",
            //     nullable: false,
            //     oldClrType: typeof(Guid),
            //     oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "game_id",
                table: "drives",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "ix_drives_period_id_sequence",
                table: "drives",
                columns: new[] { "period_id", "sequence" });

            migrationBuilder.AddForeignKey(
                name: "fk_drives_games_game_id",
                table: "drives",
                column: "game_id",
                principalTable: "games",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_drives_games_game_id",
                table: "drives");

            migrationBuilder.DropIndex(
                name: "ix_drives_period_id_sequence",
                table: "drives");

            // migrationBuilder.AlterColumn<Guid>(
            //     name: "id",
            //     table: "pbp_event_statistics",
            //     type: "uuid",
            //     nullable: false,
            //     oldClrType: typeof(long),
            //     oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "game_id",
                table: "drives",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_drives_game_id_sequence",
                table: "drives",
                columns: new[] { "game_id", "sequence" });

            migrationBuilder.CreateIndex(
                name: "ix_drives_period_id",
                table: "drives",
                column: "period_id");

            migrationBuilder.AddForeignKey(
                name: "fk_drives_games_game_id",
                table: "drives",
                column: "game_id",
                principalTable: "games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
