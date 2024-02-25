using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class SeedMenuProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Image", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 3, "Lorem, deren, trataro, filede, nerada", "img/menu/lobster-bisque.jpg", "Lobster Bisque", 5.95m },
                    { 2, 3, "Lorem, deren, trataro, filede, nerada", "img/menu/bread-barrel.jpg", "Bread Barrel", 6.95m },
                    { 3, 3, "A delicate crab cake served on a toasted roll with lettuce and tartar sauce\r\n", "img/menu/cake.jpg", "Crab Cake", 7.95m },
                    { 5, 2, "Grilled chicken with provolone, artichoke hearts, and roasted red pesto\r\n", "img/menu/tuscan-grilled.jpg", "Tuscan Grilled", 9.95m },
                    { 6, 1, "Lorem, deren, trataro, filede, nerada", "img/menu/mozzarella.jpg", "Mozzarella Stick", 4.95m },
                    { 7, 2, "Fresh spinach, crisp romaine, tomatoes, and Greek olives", "img/menu/greek-salad.jpg", "Greek Salad", 9.95m },
                    { 8, 2, "Fresh spinach with mushrooms, hard boiled egg, and warm bacon vinaigrette", "img/menu/spinach-salad.jpg", "Spinach Salad", 9.95m },
                    { 9, 3, "Plump lobster meat, mayo and crisp lettuce on a toasted bulky roll", "img/menu/lobster-roll.jpg", "Lobster Roll", 9.95m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
