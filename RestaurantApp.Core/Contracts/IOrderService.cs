using RestaurantApp.Core.Models.Order;
using RestaurantApp.Core.Models.ShoppingCart;

namespace RestaurantApp.Core.Contracts
{
	public interface IOrderService
	{
		Task CheckoutAsync(CheckoutFormModel model, string userId);

		Task<IEnumerable<OrderViewModel>> GetOrdersAsync(string userId);

		Task<CheckoutFormModel> GetDataForCheckoutAsync(string userId);
	}
}
