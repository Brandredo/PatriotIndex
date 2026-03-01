using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBDrives : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_drives_periods_period_id",
                table: "drives");

            migrationBuilder.DropIndex(
                name: "ix_drives_period_id",
                table: "drives");

            migrationBuilder.DropColumn(
                name: "period_id",
                table: "drives");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "period_id",
                table: "drives",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_drives_period_id",
                table: "drives",
                column: "period_id");

            migrationBuilder.AddForeignKey(
                name: "fk_drives_periods_period_id",
                table: "drives",
                column: "period_id",
                principalTable: "periods",
                principalColumn: "id");
        }
    }
}
