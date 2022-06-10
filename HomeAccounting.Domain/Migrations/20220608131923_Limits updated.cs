using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeAccounting.Domain.Migrations
{
    public partial class Limitsupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LimitPeriod",
                table: "OutcomeLimits");

            migrationBuilder.AddColumn<DateTime>(
                name: "LimitFrom",
                table: "OutcomeLimits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LimitTo",
                table: "OutcomeLimits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LimitFrom",
                table: "OutcomeLimits");

            migrationBuilder.DropColumn(
                name: "LimitTo",
                table: "OutcomeLimits");

            migrationBuilder.AddColumn<byte>(
                name: "LimitPeriod",
                table: "OutcomeLimits",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
