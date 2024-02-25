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
		Task<IEnumerable<ShoppingCartViewModel>> GetShoppingCartAsync(string userId);
	}
}
