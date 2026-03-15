using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddSeasonEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "seasons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seasons", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_seasons_status",
                table: "seasons",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_seasons_year_code",
                table: "seasons",
                columns: new[] { "year", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "seasons");
        }
    }
}
