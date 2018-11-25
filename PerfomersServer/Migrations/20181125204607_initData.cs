using Microsoft.EntityFrameworkCore.Migrations;

namespace PerfomersServer.Migrations
{
    public partial class initData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Perfomer", true);
            migrationBuilder.InsertData(
                table: "Perfomer",
                columns: new[] { "Id", "Name" },
                values: new object[,] { { 1, "MISSIO" }, { 2, "IAMX" } ,
                                        { 3, "Sir Sly" }, { 4, "The xx" },
                                        { 5, "lovelytheband" }, { 6, "Two Door Cinema Club" } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Perfomer", true);
        }
    }
}
