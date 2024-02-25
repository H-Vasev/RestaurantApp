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

		public async Task<IEnumerable<MenuViewModel>> GetMenuAsync()
		{
			return await dbContext.Products
				.Select(p => new MenuViewModel()
				{
					Id = p.Id,
					Name = p.Name,
					Image = p.Image,
					Price = p.Price,
					CategoryId = p.CategoryId,
					Description = p.Description
				}).ToArrayAsync();
		}
	}
}
