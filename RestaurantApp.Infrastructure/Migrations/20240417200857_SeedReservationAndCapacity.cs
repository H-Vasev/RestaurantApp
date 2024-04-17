using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class SeedReservationAndCapacity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d045c09b-f219-43d6-9574-cf4aa5598186", "AQAAAAEAACcQAAAAELNEWlV0PlBoMcAveO4hlJVo7NVOlErGjLYeZF5+LqHMjTy8sQJAUKcMJglsJVRerg==", "fd6701c7-3950-4e79-8f11-25358591b913" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "74adc793-39e9-4d56-87de-0d61178a7f81", "AQAAAAEAACcQAAAAEMiv7Pj15k1hSQawyw6DRoHVaWCh/Y6xZPpEYElZxM0Y9IUbnZroTs9ehsI9kktQxw==", "9ecf1bc7-70c9-4b6c-8877-b227c416adbe" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0729a89c-29a0-4135-bad0-aef3b8b29c89", "AQAAAAEAACcQAAAAEOERtMW2EycJS+K4TPb7NrV5Empz9p+chdmg0KkZj1104I2FfWj8YtoyQTq6Ami6rA==", "fd31269b-5130-4bb1-94da-249b836b74ba" });

            migrationBuilder.InsertData(
                table: "CapacitySlots",
                columns: new[] { "Id", "CurrentCapacity", "SlotDate", "TotalCapacity" },
                values: new object[,]
                {
                    { 1, 94, new DateTime(2024, 4, 27, 20, 8, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 2, 94, new DateTime(2024, 5, 7, 20, 8, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 3, 94, new DateTime(2024, 5, 17, 20, 8, 0, 0, DateTimeKind.Unspecified), 100 },
                    { 4, 94, new DateTime(2024, 6, 1, 20, 8, 0, 0, DateTimeKind.Unspecified), 100 }
                });

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 17, 21, 8, 57, 243, DateTimeKind.Local).AddTicks(1024));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 16, 21, 8, 57, 243, DateTimeKind.Local).AddTicks(1028));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 15, 21, 8, 57, 243, DateTimeKind.Local).AddTicks(1031));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 14, 21, 8, 57, 243, DateTimeKind.Local).AddTicks(1033));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 13, 21, 8, 57, 243, DateTimeKind.Local).AddTicks(1035));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 12, 21, 8, 57, 243, DateTimeKind.Local).AddTicks(1036));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 11, 21, 8, 57, 243, DateTimeKind.Local).AddTicks(1038));

            migrationBuilder.UpdateData(
                table: "GalleryImages",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 4, 10, 21, 8, 57, 243, DateTimeKind.Local).AddTicks(1040));

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "ApplicationUserId", "CapacitySlotId", "Date", "Description", "Email", "EventId", "FirstName", "LastName", "PeopleCount", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("3f37879f-31b0-44c1-bfd0-0b8f735e1eec"), new Guid("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"), 2, new DateTime(2024, 5, 7, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "paul@gmail.com", null, "Paul", "Robinson", 2, "07708064509" },
                    { new Guid("59bcf48b-9e08-4d08-96ef-db6c85b8cba7"), new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"), 4, new DateTime(2024, 6, 1, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "jack@gmail.com", null, "Jack", "Jackson", 2, "07708064509" },
                    { new Guid("63b0b804-c892-4ba3-a26a-31f0905bd3a3"), new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"), 3, new DateTime(2024, 5, 17, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "john@gmail.com", null, "John", "Walker", 2, "07708064509" },
                    { new Guid("88340b80-4900-477f-b717-b9106c886171"), new Guid("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"), 3, new DateTime(2024, 5, 17, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "paul@gmail.com", null, "Paul", "Robinson", 2, "07708064509" },
                    { new Guid("8ac38b36-3a93-4c92-add3-e1d8eaa0d1c4"), new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"), 1, new DateTime(2024, 4, 27, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "jack@gmail.com", null, "Jack", "Jackson", 2, "07708064509" },
                    { new Guid("8d39fd7a-4435-465b-a078-ac0867c8d188"), new Guid("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"), 1, new DateTime(2024, 4, 27, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "paul@gmail.com", null, "Paul", "Robinson", 2, "07708064509" },
                    { new Guid("a491dfca-5207-4d87-93a5-32cdbc335ee9"), new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"), 2, new DateTime(2024, 5, 7, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "jack@gmail.com", null, "Jack", "Jackson", 2, "07708064509" },
                    { new Guid("ae553d8c-b03b-45d9-a910-aec5b031d665"), new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"), 3, new DateTime(2024, 5, 17, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "jack@gmail.com", null, "Jack", "Jackson", 2, "07708064509" },
                    { new Guid("b6460bdc-658d-4163-b725-44514e797078"), new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"), 1, new DateTime(2024, 4, 27, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "john@gmail.com", null, "John", "Walker", 2, "07708064509" },
                    { new Guid("c6e6ca2b-7207-4005-99b3-a1e03fb40d9a"), new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"), 2, new DateTime(2024, 5, 7, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "john@gmail.com", null, "John", "Walker", 2, "07708064509" },
                    { new Guid("f4701ffc-2949-44dc-bd5c-ae96c9280c93"), new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"), 4, new DateTime(2024, 6, 1, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "john@gmail.com", null, "John", "Walker", 2, "07708064509" },
                    { new Guid("f95c01b6-c43b-401e-9a1e-8bb34b25ac2b"), new Guid("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"), 4, new DateTime(2024, 6, 1, 20, 8, 0, 0, DateTimeKind.Unspecified), null, "paul@gmail.com", null, "Paul", "Robinson", 2, "07708064509" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("3f37879f-31b0-44c1-bfd0-0b8f735e1eec"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("59bcf48b-9e08-4d08-96ef-db6c85b8cba7"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("63b0b804-c892-4ba3-a26a-31f0905bd3a3"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("88340b80-4900-477f-b717-b9106c886171"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("8ac38b36-3a93-4c92-add3-e1d8eaa0d1c4"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("8d39fd7a-4435-465b-a078-ac0867c8d188"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("a491dfca-5207-4d87-93a5-32cdbc335ee9"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("ae553d8c-b03b-45d9-a910-aec5b031d665"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("b6460bdc-658d-4163-b725-44514e797078"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("c6e6ca2b-7207-4005-99b3-a1e03fb40d9a"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("f4701ffc-2949-44dc-bd5c-ae96c9280c93"));

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: new Guid("f95c01b6-c43b-401e-9a1e-8bb34b25ac2b"));

            migrationBuilder.DeleteData(
                table: "CapacitySlots",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CapacitySlots",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CapacitySlots",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CapacitySlots",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e15c57a2-31a0-4cc1-8bd9-9f7cd398f3c9"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a49ad7e8-6202-46f7-9eda-b10428d39a09", "AQAAAAEAACcQAAAAEBOBjguWfGCFTKd8lg3X3Tf7/x86QRlRNxPdLsmuEVr5RDS8AEcMEGbstsMQ45wCDw==", "c09642bc-b9a8-44d6-9710-ced41d5e7dd5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd300f3c9"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e08c665d-0b85-49fb-88ca-5dc0afc70387", "AQAAAAEAACcQAAAAEExOrSTEYgD4H9KAr5Xd4GsDrQt+13U+LgURlP/WRCPUEhoHtkh3qScih/H8E9x8og==", "4a29ab6a-d677-4036-bcb8-88fce45b51e0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e15c76a2-31a0-4cc1-8bd9-9f7cd380f3c9"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a23322a6-2a6d-416c-8e4a-080611308812", "AQAAAAEAACcQAAAAEDtwW3L1cC4TlWc8Pnn23i5wAS71EyICREnFP4ecB4J2O+GS4i4XZlRXoCcifSg4sg==", "2aaa9720-0e9d-42fa-8300-def7d320358f" });

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
        }
    }
}
