using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElearningPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class q11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VideoProgresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VideoProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastWatchedSecond",
                table: "VideoProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ProgressPercentage",
                table: "VideoProgresses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "VideoProgresses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VideoDuration",
                table: "VideoProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VideoProgresses");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VideoProgresses");

            migrationBuilder.DropColumn(
                name: "LastWatchedSecond",
                table: "VideoProgresses");

            migrationBuilder.DropColumn(
                name: "ProgressPercentage",
                table: "VideoProgresses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VideoProgresses");

            migrationBuilder.DropColumn(
                name: "VideoDuration",
                table: "VideoProgresses");
        }
    }
}
