using RestaurantApp.Core.Models.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Core.Contracts
{
	public interface IShoppingCartService
	{
		Task<int> GetItamsQuantityAsync(string userId);

		Task AddToCartAsync(string userId, int id);

		Task<IEnumerable<ShoppingCartViewModel>> GetAllItemsAsync(string userId);

        Task RemoveFromCartAsync(string userId, int id);

		Task ClearCartAsync(string userId);
	}
}
