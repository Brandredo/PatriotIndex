using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddSeasonInfoToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "season_id",
                table: "games",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "week_id",
                table: "games",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "season_id",
                table: "games");

            migrationBuilder.DropColumn(
                name: "week_id",
                table: "games");
        }
    }
}
