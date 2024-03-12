using RestaurantApp.Core.Models.ShoppingCart;

namespace RestaurantApp.Core.Contracts
{
	public interface IOrderService
	{
		Task CheckoutAsync(CheckoutFormModel model, string userId);
	}
}
