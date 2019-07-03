using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ConcertsService.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Concert", true);
            migrationBuilder.InsertData(
                table: "Concert",
                columns: new[] { "Id", "VenueId", "PerfomerId", "Date" },
                values: new object[,] { { 1, 1, 4, DateTime.Parse("2019-05-11") },
                                        { 2, 5, 2, DateTime.Parse("2019-02-10") },
                                        { 3, 2, 5, DateTime.Parse("2019-03-19") } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Concert", true);
        }
    }
}
