using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Menu;
using RestaurantApp.Data;

namespace RestaurantApp.Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMemoryCache memoryCache;

        public MenuService(ApplicationDbContext dbContext, IMemoryCache memoryCache)
        {
            this.dbContext = dbContext;
            this.memoryCache = memoryCache;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {
            var cacheKey = "categories";

            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<CategoryViewModel> cahedCategories))
            {
                cahedCategories = await dbContext.Categories
                    .AsNoTracking()
                    .Select(c => new CategoryViewModel()
                    {
                        Id = c.Id,
                        Name = c.CategoryName
                    }).ToArrayAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                memoryCache.Set(cacheKey, cahedCategories, cacheEntryOptions);
            }

            return cahedCategories;
        }

        public async Task<IEnumerable<ProductViewModel>> GetMenuAsync(string? category)
        {
            var cacheKey = $"menu_{category ?? "all"}";

            if (!memoryCache.TryGetValue(cacheKey, out IEnumerable<ProductViewModel> cachedProducts))
            {
                cachedProducts = await dbContext.Products
                      .Where(p => category == null || p.Category.CategoryName == category)
                      .Select(p => new ProductViewModel()
                      {
                          Id = p.Id,
                          Name = p.Name,
                          Image = p.Image,
                          Price = p.Price,
                          CategoryId = p.CategoryId,
                          Description = p.Description
                      }).ToArrayAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                memoryCache.Set(cacheKey, cachedProducts, cacheEntryOptions);
            }

            return cachedProducts;
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var model = await dbContext.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Image = p.Image,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    Description = p.Description
                }).FirstOrDefaultAsync();

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model;
        }
    }
}
