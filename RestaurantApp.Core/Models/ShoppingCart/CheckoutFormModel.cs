namespace RestaurantApp.Core.Models.ShoppingCart
{
	public class CheckoutFormModel
	{
        public decimal TotalPrice { get; set; }

		public ICollection<ShoppingCartViewModel> Items { get; set; } = new List<ShoppingCartViewModel>();

	}
}
