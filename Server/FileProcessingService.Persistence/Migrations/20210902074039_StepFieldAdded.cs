using Microsoft.EntityFrameworkCore.Migrations;

namespace FileProcessingService.Persistence.Migrations
{
    public partial class StepFieldAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Step",
                table: "StatusMessages",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Step",
                table: "StatusMessages");
        }
    }
}
