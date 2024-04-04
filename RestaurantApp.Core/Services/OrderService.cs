using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Order;
using RestaurantApp.Core.Models.ShoppingCart;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Core.Services
{
	public class OrderService : IOrderService
	{
		private readonly ApplicationDbContext dbContext;
		private readonly IShoppingCartService shoppingCartService;

		public OrderService(ApplicationDbContext dbContext, IShoppingCartService shoppingCartService)
		{
			this.dbContext = dbContext;
			this.shoppingCartService = shoppingCartService;
		}

		public async Task CheckoutAsync(CheckoutFormModel model, string userId)
		{
			if (model.Items.Count() < 1)
			{
				throw new ArgumentException(nameof(model.Items));
			}

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

		public async Task<CheckoutFormModel> GetDataForCheckoutAsync(string userId)
		{
			var userData = await dbContext.Users
				.Where(u => u.Id == Guid.Parse(userId))
				.Select(u => new CheckoutFormModel()
				{
					Name = u.FirsName,
					Address = u.Address.Street,
					PostalCode = u.Address.PostalCode,
					Email = u.Email,
					PhoneNumber = u.PhoneNumber,
				}).FirstOrDefaultAsync();

			if (userData == null)
			{
				throw new ArgumentException(nameof(userData));
			}

            var items = await shoppingCartService.GetAllItemsAsync(userId);

            userData.Items = items;
			userData.TotalPrice = items.Sum(i => i.Price * i.Quantity);

			return userData;
		}

		public async Task<IEnumerable<OrderViewModel>> GetOrdersAsync(string userId)
		{
			var orders = await dbContext.Orders
				.Where(o => o.UserId == Guid.Parse(userId))
				.OrderByDescending(o => o.OrderDate)
				.Take(3)
				.Select(o => new OrderViewModel()
				{
					Id = o.Id.ToString(),
					OrderDate = o.OrderDate,
				})
				.ToArrayAsync();

			foreach (var order in orders)
			{
				order.OredrItemsViewModel = await GetOrderItems(order.Id);
			}

			return orders;
		}

		private async Task<IEnumerable<OrderItemsViewModel>> GetOrderItems(string orderId)
		{
			var orderItems = await dbContext.OrderItems
				.Where(oi => oi.OrderId == Guid.Parse(orderId))
				.Select(o => new OrderItemsViewModel()
				{
					Id = o.Id.ToString(),
					OrderId = o.OrderId.ToString(),
					Price = o.Price,
					ProductId = o.ProductId,
					ProductName = o.Product.Name ?? "",
					ImagePath = o.Product.Image ?? "",
					ProductDescription = o.Product.Description ?? "",
				}).ToArrayAsync();

			return orderItems;
		}
	}
}
