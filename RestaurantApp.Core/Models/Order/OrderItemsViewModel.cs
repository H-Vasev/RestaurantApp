namespace RestaurantApp.Core.Models.Order
{
	public class OrderItemsViewModel
	{
		public string Id { get; set; } = null!;

		public string OrderId { get; set; } = null!;

		public decimal Price { get; set; }

        public int? ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

		public string ProductDescription { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

    }
}
