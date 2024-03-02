using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class SeedAddressAndTown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Towns",
                columns: new[] { "Id", "TownName" },
                values: new object[] { 1, "London" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "PostalCode", "Street", "TownId" },
                values: new object[] { new Guid("553fb0ed-0384-4aeb-f80d-08dc21d76b67"), "NB12 JAT", "Baker Street", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: new Guid("553fb0ed-0384-4aeb-f80d-08dc21d76b67"));

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
