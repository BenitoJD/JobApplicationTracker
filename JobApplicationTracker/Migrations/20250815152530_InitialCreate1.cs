using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataApplied", "FollowUpDate" },
                values: new object[] { new DateTime(2025, 8, 5, 15, 25, 30, 183, DateTimeKind.Local).AddTicks(6486), new DateTime(2025, 8, 20, 15, 25, 30, 185, DateTimeKind.Local).AddTicks(9128) });

            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DataApplied", "FollowUpDate" },
                values: new object[] { new DateTime(2025, 8, 10, 15, 25, 30, 185, DateTimeKind.Local).AddTicks(9775), new DateTime(2025, 8, 18, 15, 25, 30, 185, DateTimeKind.Local).AddTicks(9779) });

            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DataApplied", "FollowUpDate" },
                values: new object[] { new DateTime(2025, 7, 31, 15, 25, 30, 185, DateTimeKind.Local).AddTicks(9781), new DateTime(2025, 8, 25, 15, 25, 30, 185, DateTimeKind.Local).AddTicks(9782) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataApplied", "FollowUpDate" },
                values: new object[] { new DateTime(2025, 8, 5, 15, 12, 22, 712, DateTimeKind.Local).AddTicks(6199), new DateTime(2025, 8, 20, 15, 12, 22, 712, DateTimeKind.Local).AddTicks(9227) });

            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DataApplied", "FollowUpDate" },
                values: new object[] { new DateTime(2025, 8, 10, 15, 12, 22, 713, DateTimeKind.Local).AddTicks(3621), new DateTime(2025, 8, 18, 15, 12, 22, 713, DateTimeKind.Local).AddTicks(3623) });

            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DataApplied", "FollowUpDate" },
                values: new object[] { new DateTime(2025, 7, 31, 15, 12, 22, 713, DateTimeKind.Local).AddTicks(3626), new DateTime(2025, 8, 25, 15, 12, 22, 713, DateTimeKind.Local).AddTicks(3627) });
        }
    }
}
