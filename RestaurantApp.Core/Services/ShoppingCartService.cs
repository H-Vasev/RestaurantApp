﻿using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.ShoppingCart;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;
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

		public async Task AddToCartAsync(string userId, int id)
		{
			string shoppingCartId = await GetShoppingCartIdAsync(userId);

			var cartProduct = await dbContext.CartProducts
				.FirstOrDefaultAsync(c => c.ProductId == id && c.ShoppingCartId == Guid.Parse(shoppingCartId));

			if (cartProduct == null)
			{
				cartProduct = new CartProduct()
				{
					ProductId = id,
					ShoppingCartId = Guid.Parse(shoppingCartId),
					Quantity = 1
				};

				await dbContext.CartProducts.AddAsync(cartProduct);
			}
			else
			{
				cartProduct.Quantity += 1;
			}
			

			await dbContext.SaveChangesAsync();
		}

		private async Task<string> GetShoppingCartIdAsync(string userId)
		{
			var user = await dbContext.Users
				.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

			return user.ShoppingCartId.ToString();
		}

		public async Task<IEnumerable<ShoppingCartViewModel>> GetShoppingCartAsync(string userId)
		{
		   return await dbContext.CartProducts
				.Where(c => c.ShoppingCart.ApplicationUser.Id == Guid.Parse(userId))
				.Select(c => new ShoppingCartViewModel()
				{
					ProductId = c.ProductId,
					ProductName = c.Product.Name,
					Price = c.Product.Price,
					Quantity = c.Quantity,
					Image = c.Product.Image
				}).ToArrayAsync();
		}
	}
}
