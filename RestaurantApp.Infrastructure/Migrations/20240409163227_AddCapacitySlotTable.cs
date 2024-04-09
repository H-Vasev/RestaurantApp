using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class AddCapacitySlotTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CapacitySlotId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CapacitySlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalCapacity = table.Column<int>(type: "int", nullable: false),
                    CurrentCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapacitySlots", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CapacitySlotId",
                table: "Reservations",
                column: "CapacitySlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_CapacitySlots_CapacitySlotId",
                table: "Reservations",
                column: "CapacitySlotId",
                principalTable: "CapacitySlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_CapacitySlots_CapacitySlotId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "CapacitySlots");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CapacitySlotId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CapacitySlotId",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 3, 28, 17, 25, 33, 205, DateTimeKind.Local).AddTicks(1489));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 3, 27, 17, 25, 33, 205, DateTimeKind.Local).AddTicks(1493));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 3, 26, 17, 25, 33, 205, DateTimeKind.Local).AddTicks(1495));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 3, 25, 17, 25, 33, 205, DateTimeKind.Local).AddTicks(1497));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 3, 24, 17, 25, 33, 205, DateTimeKind.Local).AddTicks(1499));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 3, 23, 17, 25, 33, 205, DateTimeKind.Local).AddTicks(1500));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 3, 22, 17, 25, 33, 205, DateTimeKind.Local).AddTicks(1502));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 3, 21, 17, 25, 33, 205, DateTimeKind.Local).AddTicks(1504));
        }
    }
}
