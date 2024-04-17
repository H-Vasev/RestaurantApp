using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Infrastructure.Data.Configurations
{
	public class ShoppingCartEntityConfiruration : IEntityTypeConfiguration<ShoppingCart>
	{
		public void Configure(EntityTypeBuilder<ShoppingCart> builder)
		{
			builder.HasData(GenerateShoppingCarts());
		}

		public static ICollection<ShoppingCart> GenerateShoppingCarts()
		{
			var shoppingCarts = new List<ShoppingCart>()
			{
				new ShoppingCart()
				{
					Id = Guid.Parse("eaeca7c9-c81f-4d4a-aed6-8eae37e1460f"),
				},
				new ShoppingCart()
				{
					Id = Guid.Parse("b574b35d-9fcb-4403-aca3-0feb4479804b"),
				},
				new ShoppingCart()
				{
					Id = Guid.Parse("9e3cdb70-6f53-4a89-ae00-639f5e265219"),
				},
			};

			return shoppingCarts;
		}
	}
}
