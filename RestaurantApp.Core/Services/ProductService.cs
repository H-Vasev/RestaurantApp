using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Menu;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext dbContext;

        public ProductService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddProductAsync(ProductFormModel model)
        {
            var product = new Product()
            {
                Name = model.Title,
                Description = model.Description,
                Price = model.Price,
                Image = model.ImagePath,
                CategoryId = model.CategoryId,
            };

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
        }
    }
}
