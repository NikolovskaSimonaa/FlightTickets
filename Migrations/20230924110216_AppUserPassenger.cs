using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightTickets.Migrations
{
    public partial class AppUserPassenger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUser",
                table: "Passenger",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUser",
                table: "Passenger");
        }
    }
}
