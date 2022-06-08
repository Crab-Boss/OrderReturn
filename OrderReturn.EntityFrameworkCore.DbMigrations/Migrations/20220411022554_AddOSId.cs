using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderReturn.Migrations
{
    public partial class AddOSId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ErrorCount",
                schema: "dbo",
                table: "OrderReturnHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                schema: "dbo",
                table: "OrderReturnHistories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSId",
                schema: "dbo",
                table: "OrderReturnHistories",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PushTime",
                schema: "dbo",
                table: "OrderReturnHistories",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorCount",
                schema: "dbo",
                table: "OrderReturnHistories");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                schema: "dbo",
                table: "OrderReturnHistories");

            migrationBuilder.DropColumn(
                name: "OSId",
                schema: "dbo",
                table: "OrderReturnHistories");

            migrationBuilder.DropColumn(
                name: "PushTime",
                schema: "dbo",
                table: "OrderReturnHistories");
        }
    }
}
