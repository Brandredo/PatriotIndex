using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AlterNullified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN rush_play_stat_nullified TYPE boolean USING rush_play_stat_nullified::boolean;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN return_play_stat_nullified TYPE boolean USING return_play_stat_nullified::boolean;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN receive_play_stat_nullified TYPE boolean USING receive_play_stat_nullified::boolean;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN punt_play_stat_nullified TYPE boolean USING punt_play_stat_nullified::boolean;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN nullified TYPE boolean USING nullified::boolean;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN kick_play_stat_nullified TYPE boolean USING kick_play_stat_nullified::boolean;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN fumble_play_stat_nullified TYPE boolean USING fumble_play_stat_nullified::boolean;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN field_goal_play_stat_nullified TYPE boolean USING field_goal_play_stat_nullified::boolean;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN defense_play_stat_nullified TYPE boolean USING defense_play_stat_nullified::boolean;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN rush_play_stat_nullified TYPE integer USING rush_play_stat_nullified::integer;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN return_play_stat_nullified TYPE integer USING return_play_stat_nullified::integer;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN receive_play_stat_nullified TYPE integer USING receive_play_stat_nullified::integer;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN punt_play_stat_nullified TYPE integer USING punt_play_stat_nullified::integer;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN nullified TYPE integer USING nullified::integer;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN kick_play_stat_nullified TYPE integer USING kick_play_stat_nullified::integer;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN fumble_play_stat_nullified TYPE integer USING fumble_play_stat_nullified::integer;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN field_goal_play_stat_nullified TYPE integer USING field_goal_play_stat_nullified::integer;");

            migrationBuilder.Sql(
                "ALTER TABLE play_statistics ALTER COLUMN defense_play_stat_nullified TYPE integer USING defense_play_stat_nullified::integer;");
        }
    }
}
