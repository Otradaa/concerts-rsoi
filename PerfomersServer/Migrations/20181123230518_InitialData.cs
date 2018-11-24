using Microsoft.EntityFrameworkCore.Migrations;

namespace PerfomersServer.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Perfomer",
                columns: new[] { "Id", "Name"},
                values: new object[,] { {1,"sdrtfgy" },{2,"sfgh" } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Perfomer",
                keyColumns: new[] { "Id", "Name" },
                keyValues: new object[,] { { 1, "sdrtfgy" }, { 2, "sfgh" } });
        }
    }
}
