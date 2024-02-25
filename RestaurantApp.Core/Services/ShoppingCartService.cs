using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.ShoppingCart;
using RestaurantApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Core.Services
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly ApplicationDbContext dbContext;

		public ShoppingCartService(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<IEnumerable<ShoppingCartViewModel>> GetShoppingCartAsync(string userId)
		{
		   return await dbContext.CartProducts
				.Where(c => c.ShoppingCart.ApplicationUser.Id == Guid.Parse(userId))
				.Select(c => new ShoppingCartViewModel()
				{
					ProductId = c.ProductId,
					ProductName = c.Product.Name,
					TotalPrice = c.Product.Price,
					Quantity = c.Quantity,
					Image = c.Product.Image
				}).ToArrayAsync();
		}
	}
}
