using Microsoft.EntityFrameworkCore.Migrations;

namespace VenuesService.Migrations
{
    public partial class Concert_in_Schedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConcertId",
                table: "Schedule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcertId",
                table: "Schedule");
        }
    }
}
