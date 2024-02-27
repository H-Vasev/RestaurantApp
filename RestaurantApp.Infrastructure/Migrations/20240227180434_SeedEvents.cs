using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class SeedEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "EndEvent", "StartEvent", "Title" },
                values: new object[] { 1, "Christmas Day", new DateTime(2024, 12, 25, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 20, 0, 0, 0, DateTimeKind.Unspecified), "Christmas Day" });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "EndEvent", "StartEvent", "Title" },
                values: new object[] { 2, "Heppy New Year", new DateTime(2024, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 31, 20, 0, 0, 0, DateTimeKind.Unspecified), "Heppy New Year" });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "EndEvent", "StartEvent", "Title" },
                values: new object[] { 3, "Easter Sunday", new DateTime(2024, 3, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 31, 20, 0, 0, 0, DateTimeKind.Unspecified), "Easter Sunday" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
