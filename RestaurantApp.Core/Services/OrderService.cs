using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.ShoppingCart;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Core.Services
{
	public class OrderService : IOrderService
	{
		private readonly ApplicationDbContext dbContext;

		public OrderService(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task CheckoutAsync(CheckoutFormModel model, string userId)
		{
			var orderItems = new List<OrderItem>();

			var order = new Order()
			{
				UserId = Guid.Parse(userId),
				TotalPrice = model.TotalPrice,
				OrderDate = DateTime.UtcNow,
			};

			foreach (var item in model.Items)
			{
				orderItems.Add(new OrderItem()
				{
					OrderId = order.Id,
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					Price = item.Price,
				});
			}

			order.OrderItems = orderItems;

			await dbContext.Orders.AddAsync(order);
			await dbContext.SaveChangesAsync();
		}
	}
}
