using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CalendarMvc.Migrations
{
    public partial class Description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Event",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryAttendeeEmail",
                table: "Event",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntityId",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventId",
                table: "Event",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Event");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Event",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryAttendeeEmail",
                table: "Event",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
