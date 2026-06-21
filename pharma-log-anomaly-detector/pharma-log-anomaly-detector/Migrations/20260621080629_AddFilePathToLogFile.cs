using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharma_log_anomaly_detector.Migrations
{
    /// <inheritdoc />
    public partial class AddFilePathToLogFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "LogFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "LogFiles");
        }
    }
}
