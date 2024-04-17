using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class SeedApllicationUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 17, 20, 34, 46, 555, DateTimeKind.Local).AddTicks(1876));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 16, 20, 34, 46, 555, DateTimeKind.Local).AddTicks(1881));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 15, 20, 34, 46, 555, DateTimeKind.Local).AddTicks(1884));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 14, 20, 34, 46, 555, DateTimeKind.Local).AddTicks(1886));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 13, 20, 34, 46, 555, DateTimeKind.Local).AddTicks(1887));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 12, 20, 34, 46, 555, DateTimeKind.Local).AddTicks(1889));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 11, 20, 34, 46, 555, DateTimeKind.Local).AddTicks(1891));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 10, 20, 34, 46, 555, DateTimeKind.Local).AddTicks(1893));

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "Id", "Price" },
                values: new object[,]
                {
                    { new Guid("9e3cdb70-6f53-4a89-ae00-639f5e265219"), 0m },
                    { new Guid("b574b35d-9fcb-4403-aca3-0feb4479804b"), 0m },
                    { new Guid("eaeca7c9-c81f-4d4a-aed6-8eae37e1460f"), 0m }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AddressId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirsName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "ShoppingCartId", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"), 0, new Guid("553fb0ed-0384-4aeb-f80d-08dc21d76b67"), "a49ad7e8-6202-46f7-9eda-b10428d39a09", "paul@gmail.com", true, "Paul", "Robinson", false, null, "PAUL@GMAIL.COM", "PAUL", "AQAAAAEAACcQAAAAEBOBjguWfGCFTKd8lg3X3Tf7/x86QRlRNxPdLsmuEVr5RDS8AEcMEGbstsMQ45wCDw==", null, false, "c09642bc-b9a8-44d6-9710-ced41d5e7dd5", new Guid("9e3cdb70-6f53-4a89-ae00-639f5e265219"), false, "paul" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AddressId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirsName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "ShoppingCartId", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"), 0, new Guid("553fb0ed-0384-4aeb-f80d-08dc21d76b67"), "e08c665d-0b85-49fb-88ca-5dc0afc70387", "john@gmail.com", true, "John", "Walker", false, null, "JOHN@GMAIL.COM", "JOHN", "AQAAAAEAACcQAAAAEExOrSTEYgD4H9KAr5Xd4GsDrQt+13U+LgURlP/WRCPUEhoHtkh3qScih/H8E9x8og==", null, false, "4a29ab6a-d677-4036-bcb8-88fce45b51e0", new Guid("eaeca7c9-c81f-4d4a-aed6-8eae37e1460f"), false, "john" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AddressId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirsName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "ShoppingCartId", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"), 0, new Guid("553fb0ed-0384-4aeb-f80d-08dc21d76b67"), "a23322a6-2a6d-416c-8e4a-080611308812", "jack@gmail.com", true, "Jack", "Jackson", false, null, "JACK@GMAIL.COM", "JACK", "AQAAAAEAACcQAAAAEDtwW3L1cC4TlWc8Pnn23i5wAS71EyICREnFP4ecB4J2O+GS4i4XZlRXoCcifSg4sg==", null, false, "2aaa9720-0e9d-42fa-8300-def7d320358f", new Guid("b574b35d-9fcb-4403-aca3-0feb4479804b"), false, "jack" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"));

            migrationBuilder.DeleteData(
                table: "ShoppingCarts",
                keyColumn: "Id",
                keyValue: new Guid("9e3cdb70-6f53-4a89-ae00-639f5e265219"));

            migrationBuilder.DeleteData(
                table: "ShoppingCarts",
                keyColumn: "Id",
                keyValue: new Guid("b574b35d-9fcb-4403-aca3-0feb4479804b"));

            migrationBuilder.DeleteData(
                table: "ShoppingCarts",
                keyColumn: "Id",
                keyValue: new Guid("eaeca7c9-c81f-4d4a-aed6-8eae37e1460f"));

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
        }
    }
}
