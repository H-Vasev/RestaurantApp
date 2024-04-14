using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class SetNullToApplicationUserIdInReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 14, 22, 10, 41, 286, DateTimeKind.Local).AddTicks(3477));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 13, 22, 10, 41, 286, DateTimeKind.Local).AddTicks(3482));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 12, 22, 10, 41, 286, DateTimeKind.Local).AddTicks(3486));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 11, 22, 10, 41, 286, DateTimeKind.Local).AddTicks(3488));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 10, 22, 10, 41, 286, DateTimeKind.Local).AddTicks(3490));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 9, 22, 10, 41, 286, DateTimeKind.Local).AddTicks(3492));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 8, 22, 10, 41, 286, DateTimeKind.Local).AddTicks(3494));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 7, 22, 10, 41, 286, DateTimeKind.Local).AddTicks(3496));

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 9, 17, 32, 26, 951, DateTimeKind.Local).AddTicks(4367));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 8, 17, 32, 26, 951, DateTimeKind.Local).AddTicks(4373));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 7, 17, 32, 26, 951, DateTimeKind.Local).AddTicks(4376));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 6, 17, 32, 26, 951, DateTimeKind.Local).AddTicks(4378));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 5, 17, 32, 26, 951, DateTimeKind.Local).AddTicks(4380));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 4, 17, 32, 26, 951, DateTimeKind.Local).AddTicks(4382));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 3, 17, 32, 26, 951, DateTimeKind.Local).AddTicks(4389));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 2, 17, 32, 26, 951, DateTimeKind.Local).AddTicks(4391));

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_ApplicationUserId",
                table: "Reservations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
