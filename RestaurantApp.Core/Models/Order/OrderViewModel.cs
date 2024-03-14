namespace RestaurantApp.Core.Models.Order
{
	public class OrderViewModel
	{
		public string Id { get; set; } = null!;

		public DateTime OrderDate { get; set; }

		public IEnumerable<OrderItemsViewModel> OredrItemsViewModel { get; set; } = new List<OrderItemsViewModel>();
    }
}
