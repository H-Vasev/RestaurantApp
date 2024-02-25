namespace RestaurantApp.Core.Models.ShoppingCart
{
	public class ShoppingCartViewModel
	{
		public string ProductName { get; set; } = string.Empty;

        public int ProductId { get; set; }

		public string? Image { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
	}
}
