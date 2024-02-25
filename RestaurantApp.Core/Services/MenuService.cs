using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Menu;
using RestaurantApp.Data;

namespace RestaurantApp.Core.Services
{
    public class MenuService : IMenuService
	{
		private readonly ApplicationDbContext dbContext;

		public MenuService(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {
            return await dbContext.Categories
                .Select(c => new CategoryViewModel()
				{
                    Id = c.Id,
                    Name = c.CategoryName
                }).ToArrayAsync();
        }

        public async Task<IEnumerable<ProductViewModel>> GetMenuAsync(string? category)
		{
			return await dbContext.Products
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
