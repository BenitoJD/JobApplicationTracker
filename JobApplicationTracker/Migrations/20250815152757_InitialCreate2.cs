using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "FollowUpDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 2,
                column: "FollowUpDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 3,
                column: "FollowUpDate",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "FollowUpDate",
                value: new DateTime(2025, 8, 20, 15, 25, 30, 185, DateTimeKind.Local).AddTicks(9128));

            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 2,
                column: "FollowUpDate",
                value: new DateTime(2025, 8, 18, 15, 25, 30, 185, DateTimeKind.Local).AddTicks(9779));

            migrationBuilder.UpdateData(
                table: "JobApplications",
                keyColumn: "Id",
                keyValue: 3,
                column: "FollowUpDate",
                value: new DateTime(2025, 8, 25, 15, 25, 30, 185, DateTimeKind.Local).AddTicks(9782));
        }
    }
}
