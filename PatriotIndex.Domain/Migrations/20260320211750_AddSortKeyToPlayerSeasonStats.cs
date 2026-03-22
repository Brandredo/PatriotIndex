using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddSortKeyToPlayerSeasonStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sort_key",
                table: "player_season_stats",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_player_season_stats_sort_key_player_id",
                table: "player_season_stats",
                columns: new[] { "sort_key", "player_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_player_season_stats_sort_key_player_id",
                table: "player_season_stats");

            migrationBuilder.DropColumn(
                name: "sort_key",
                table: "player_season_stats");
        }
    }
}
