using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriotIndex.Domain.Migrations
{
    /// <inheritdoc />
    public partial class FixPositionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE players SET position = CASE position
                    WHEN '0'  THEN 'C'   WHEN '1'  THEN 'CB'  WHEN '2'  THEN 'DB'
                    WHEN '3'  THEN 'DE'  WHEN '4'  THEN 'DL'  WHEN '5'  THEN 'DT'
                    WHEN '6'  THEN 'FB'  WHEN '7'  THEN 'FS'  WHEN '8'  THEN 'G'
                    WHEN '9'  THEN 'LB'  WHEN '10' THEN 'LS'  WHEN '11' THEN 'MLB'
                    WHEN '12' THEN 'NT'  WHEN '13' THEN 'OG'  WHEN '14' THEN 'OT'
                    WHEN '15' THEN 'OL'  WHEN '16' THEN 'OLB' WHEN '17' THEN 'P'
                    WHEN '18' THEN 'QB'  WHEN '19' THEN 'RB'  WHEN '20' THEN 'SAF'
                    WHEN '21' THEN 'SS'  WHEN '22' THEN 'T'   WHEN '23' THEN 'TE'
                    WHEN '24' THEN 'WR'  WHEN '25' THEN 'K'
                    ELSE position
                END
                WHERE position ~ '^[0-9]+$';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE players SET position = CASE position
                    WHEN 'C'   THEN '0'  WHEN 'CB'  THEN '1'  WHEN 'DB'  THEN '2'
                    WHEN 'DE'  THEN '3'  WHEN 'DL'  THEN '4'  WHEN 'DT'  THEN '5'
                    WHEN 'FB'  THEN '6'  WHEN 'FS'  THEN '7'  WHEN 'G'   THEN '8'
                    WHEN 'LB'  THEN '9'  WHEN 'LS'  THEN '10' WHEN 'MLB' THEN '11'
                    WHEN 'NT'  THEN '12' WHEN 'OG'  THEN '13' WHEN 'OT'  THEN '14'
                    WHEN 'OL'  THEN '15' WHEN 'OLB' THEN '16' WHEN 'P'   THEN '17'
                    WHEN 'QB'  THEN '18' WHEN 'RB'  THEN '19' WHEN 'SAF' THEN '20'
                    WHEN 'SS'  THEN '21' WHEN 'T'   THEN '22' WHEN 'TE'  THEN '23'
                    WHEN 'WR'  THEN '24' WHEN 'K'   THEN '25'
                    ELSE position
                END
                WHERE position ~ '^[A-Z]+$';
                """);
        }
    }
}
