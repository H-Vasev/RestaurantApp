using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class ShoppingCart
	{
		[Key]
		public Guid Id { get; set; }

		public decimal Price { get; set; }

		public ApplicationUser ApplicationUser { get; set; } = null!;

		public ICollection<CartProduct> CartProducts { get; set; } = new HashSet<CartProduct>();
	}
}
