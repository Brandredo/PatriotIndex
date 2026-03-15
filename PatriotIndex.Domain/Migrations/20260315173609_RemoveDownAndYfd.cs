using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDownAndYfd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_plays_game_id_down",
                table: "plays");

            migrationBuilder.DropColumn(
                name: "distance",
                table: "plays");

            migrationBuilder.DropColumn(
                name: "down",
                table: "plays");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "distance",
                table: "plays",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "down",
                table: "plays",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_plays_game_id_down",
                table: "plays",
                columns: new[] { "game_id", "down" });
        }
    }
}
