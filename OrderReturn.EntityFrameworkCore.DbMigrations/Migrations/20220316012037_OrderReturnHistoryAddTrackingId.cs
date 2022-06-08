using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderReturn.Migrations
{
    public partial class OrderReturnHistoryAddTrackingId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackingId",
                schema: "dbo",
                table: "OrderReturnHistories",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingId",
                schema: "dbo",
                table: "OrderReturnHistories");
        }
    }
}
