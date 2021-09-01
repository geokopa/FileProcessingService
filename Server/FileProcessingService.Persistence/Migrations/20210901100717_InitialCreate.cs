using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileProcessingService.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessedFileContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElementName = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedFileContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DuplicateWordStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuplicateWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuplicateCount = table.Column<int>(type: "int", nullable: false),
                    ProcessedFileContentId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuplicateWordStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuplicateWordStatistics_ProcessedFileContents_ProcessedFileContentId",
                        column: x => x.ProcessedFileContentId,
                        principalTable: "ProcessedFileContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuplicateWordStatistics_ProcessedFileContentId",
                table: "DuplicateWordStatistics",
                column: "ProcessedFileContentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuplicateWordStatistics");

            migrationBuilder.DropTable(
                name: "ProcessedFileContents");
        }
    }
}
