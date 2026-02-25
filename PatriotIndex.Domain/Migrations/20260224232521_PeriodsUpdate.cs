using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class PeriodsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_drives_periods_period_id",
                table: "drives");

            migrationBuilder.DropForeignKey(
                name: "fk_drives_periods_period_id1",
                table: "drives");

            migrationBuilder.DropIndex(
                name: "ix_drives_period_id_sequence",
                table: "drives");

            migrationBuilder.DropIndex(
                name: "ix_drives_period_id1",
                table: "drives");

            migrationBuilder.DropColumn(
                name: "period_id1",
                table: "drives");

            migrationBuilder.AddColumn<Guid>(
                name: "period_id",
                table: "pbp_drive_events",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "period_id",
                table: "drives",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "ix_pbp_drive_events_period_id",
                table: "pbp_drive_events",
                column: "period_id");

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

            migrationBuilder.AddForeignKey(
                name: "fk_pbp_drive_events_periods_period_id",
                table: "pbp_drive_events",
                column: "period_id",
                principalTable: "periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_drives_periods_period_id",
                table: "drives");

            migrationBuilder.DropForeignKey(
                name: "fk_pbp_drive_events_periods_period_id",
                table: "pbp_drive_events");

            migrationBuilder.DropIndex(
                name: "ix_pbp_drive_events_period_id",
                table: "pbp_drive_events");

            migrationBuilder.DropIndex(
                name: "ix_drives_period_id",
                table: "drives");

            migrationBuilder.DropColumn(
                name: "period_id",
                table: "pbp_drive_events");

            migrationBuilder.AlterColumn<Guid>(
                name: "period_id",
                table: "drives",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "period_id1",
                table: "drives",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_drives_period_id_sequence",
                table: "drives",
                columns: new[] { "period_id", "sequence" });

            migrationBuilder.CreateIndex(
                name: "ix_drives_period_id1",
                table: "drives",
                column: "period_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_drives_periods_period_id",
                table: "drives",
                column: "period_id",
                principalTable: "periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_drives_periods_period_id1",
                table: "drives",
                column: "period_id1",
                principalTable: "periods",
                principalColumn: "id");
        }
    }
}
