using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Infrastructure.Data.Configurations
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            if (DatabaseSeedController.SeedEnabled)
            {
                builder.HasData(GenerateProducts());
            }
        }

        private Product[] GenerateProducts()
        {
            return new Product[]
            {
                new Product
                {
                    Id = 1,
                    Name = "Lobster Bisque",
                    Description = "Lorem, deren, trataro, filede, nerada",
                    Image = "img/menu/lobster-bisque.jpg",
                    Price = 5.95m,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 2,
                    Name = "Bread Barrel",
                    Description = "Lorem, deren, trataro, filede, nerada",
                    Image = "img/menu/bread-barrel.jpg",
                    Price = 6.95m,
                    CategoryId = 3
                },
                new Product
                {
                     Id = 3,
                     Name = "Crab Cake",
                     Description = "A delicate crab cake served on a toasted roll with lettuce and tartar sauce\r\n",
                     Image = "img/menu/cake.jpg",
                     Price = 7.95m,
                     CategoryId = 3
                },
                new Product
                {
                     Id = 5,
                     Name = "Tuscan Grilled",
                     Description = "Grilled chicken with provolone, artichoke hearts, and roasted red pesto\r\n",
                     Image = "img/menu/tuscan-grilled.jpg",
                     Price = 9.95m,
                     CategoryId = 2
                },
                new Product
                {
                    Id = 6,
                    Name = "Mozzarella Stick",
                    Description = "Lorem, deren, trataro, filede, nerada",
                    Image = "img/menu/mozzarella.jpg",
                    Price = 4.95m,
                    CategoryId = 1
                },
                new Product
                {
                     Id = 7,
                     Name = "Greek Salad",
                     Description = "Fresh spinach, crisp romaine, tomatoes, and Greek olives",
                     Image = "img/menu/greek-salad.jpg",
                     Price = 9.95m,
                     CategoryId = 2
                },
                new Product
                {
                     Id = 8,
                     Name = "Spinach Salad",
                     Description = "Fresh spinach with mushrooms, hard boiled egg, and warm bacon vinaigrette",
                     Image = "img/menu/spinach-salad.jpg",
                     Price = 9.95m,
                     CategoryId = 2
                },
                new Product
                {
                      Id = 9,
                      Name = "Lobster Roll",
                      Description = "Plump lobster meat, mayo and crisp lettuce on a toasted bulky roll",
                      Image = "img/menu/lobster-roll.jpg",
                      Price = 9.95m,
                      CategoryId = 3
                }
            };
        }
    }
}
