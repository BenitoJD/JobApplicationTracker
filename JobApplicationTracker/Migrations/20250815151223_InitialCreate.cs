using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobApplicationTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DataApplied = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FollowUpDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "JobApplications",
                columns: new[] { "Id", "CompanyName", "DataApplied", "FollowUpDate", "JobTitle", "Notes", "Status" },
                values: new object[,]
                {
                    { 1, "Tech Solutions", new DateTime(2025, 8, 5, 15, 12, 22, 712, DateTimeKind.Local).AddTicks(6199), new DateTime(2025, 8, 20, 15, 12, 22, 712, DateTimeKind.Local).AddTicks(9227), "Software Engineer", "Initial application submitted.", 0 },
                    { 2, "Innovatech", new DateTime(2025, 8, 10, 15, 12, 22, 713, DateTimeKind.Local).AddTicks(3621), new DateTime(2025, 8, 18, 15, 12, 22, 713, DateTimeKind.Local).AddTicks(3623), "Data Analyst", "Interview scheduled for next week.", 1 },
                    { 3, "Global Corp", new DateTime(2025, 7, 31, 15, 12, 22, 713, DateTimeKind.Local).AddTicks(3626), new DateTime(2025, 8, 25, 15, 12, 22, 713, DateTimeKind.Local).AddTicks(3627), "Project Manager", "Received offer letter, pending decision.", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobApplications");
        }
    }
}
