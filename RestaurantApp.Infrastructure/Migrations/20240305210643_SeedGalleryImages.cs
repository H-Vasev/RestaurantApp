using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class SeedGalleryImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GalleryImages",
                columns: new[] { "Id", "ApplicationUserId", "Caption", "CreatedBy", "CreatedOn", "ImagePaht" },
                values: new object[,]
                {
                    { 1, null, null, null, new DateTime(2024, 3, 5, 21, 6, 42, 889, DateTimeKind.Local).AddTicks(2168), "img/gallery/gallery-1.jpg" },
                    { 2, null, null, null, new DateTime(2024, 3, 4, 21, 6, 42, 889, DateTimeKind.Local).AddTicks(2172), "img/gallery/gallery-2.jpg" },
                    { 3, null, null, null, new DateTime(2024, 3, 3, 21, 6, 42, 889, DateTimeKind.Local).AddTicks(2175), "img/gallery/gallery-3.jpg" },
                    { 4, null, null, null, new DateTime(2024, 3, 2, 21, 6, 42, 889, DateTimeKind.Local).AddTicks(2176), "img/gallery/gallery-4.jpg" },
                    { 5, null, null, null, new DateTime(2024, 3, 1, 21, 6, 42, 889, DateTimeKind.Local).AddTicks(2178), "img/gallery/gallery-5.jpg" },
                    { 6, null, null, null, new DateTime(2024, 2, 29, 21, 6, 42, 889, DateTimeKind.Local).AddTicks(2180), "img/gallery/gallery-6.jpg" },
                    { 7, null, null, null, new DateTime(2024, 2, 28, 21, 6, 42, 889, DateTimeKind.Local).AddTicks(2181), "img/gallery/gallery-7.jpg" },
                    { 8, null, null, null, new DateTime(2024, 2, 27, 21, 6, 42, 889, DateTimeKind.Local).AddTicks(2183), "img/gallery/gallery-8.jpg" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
