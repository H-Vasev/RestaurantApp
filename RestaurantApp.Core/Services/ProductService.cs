using Microsoft.EntityFrameworkCore;
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
            try
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
            catch (Exception)
            {
                throw new ArgumentNullException();
            }


        }

        public async Task EditProductAsync(ProductFormModel model, int id)
        {
            var product = await dbContext.Products
                .FindAsync(id);

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.Name = model.Title;
            product.Description = model.Description;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;

            await dbContext.SaveChangesAsync();
        }

        public async Task<ProductFormModel> GetProductByIdForEditAsync(int id)
        {
            var product = await dbContext.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductFormModel()
                {
                    Id = p.Id,
                    Title = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImagePath = p.Image,
                    CategoryId = p.CategoryId,
                }).FirstOrDefaultAsync();

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            return product;
        }

        public async Task<string> GetProductImagePathAsync(int id)
        {
            var product = await dbContext.Products
                .FindAsync(id);

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            return product.Image ?? "";
        }

        public async Task RemoveProductAsync(int id)
        {
            var product = await dbContext.Products
                  .FindAsync(id);

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
        }
    }
}
