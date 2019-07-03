using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace VenuesService.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Venue", true);
            migrationBuilder.Sql("DELETE FROM Schedule", true);
            migrationBuilder.InsertData(
                table: "Venue",
                columns: new[] { "Id", "Name", "Address", "Phone" },
                values: new object[,] { { 1, "Stadium Live", "Москва, Ленинградский проспект, 80 корпус 17", "+74955405540" },
                                        { 2, "Известия Hall", "Москва, Пушкинская площадь, 5", "+74959217337" } ,
                                        { 3, "Крокус Сити Холл", "65-66 км МКАД, м. Мякинино", "+74995500055" }, 
                                        { 4, "Arbat Hall", "Новый Арбат улица, 21", "+74952274464" },
                                        { 5, "Открытие Арена", "Москва, Волоколамское шоссе, 69", "+74955309501" } });

            migrationBuilder.InsertData(
                table: "Schedule",
                columns: new[] { "Id", "VenueId", "Date" },
                values: new object[,] { { 1, 1, DateTime.Parse("2019-05-11") },
                                        { 2, 3, DateTime.Parse("2019-06-28") } ,
                                        { 3, 5, DateTime.Parse("2019-02-10") },
                                        { 4, 2, DateTime.Parse("2019-03-19") } });
                                        

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Venue", true);
            migrationBuilder.Sql("DELETE FROM Schedule", true);
        }
    }
}
