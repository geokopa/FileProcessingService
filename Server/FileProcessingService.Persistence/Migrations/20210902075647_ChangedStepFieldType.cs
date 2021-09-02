using Microsoft.EntityFrameworkCore.Migrations;

namespace FileProcessingService.Persistence.Migrations
{
    public partial class ChangedStepFieldType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Step",
                table: "StatusMessages");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "StatusMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "StatusMessages");

            migrationBuilder.AddColumn<string>(
                name: "Step",
                table: "StatusMessages",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
