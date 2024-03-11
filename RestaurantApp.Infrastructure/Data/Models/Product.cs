using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Product;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class Product
	{
		[Key]
        public int Id { get; set; }

		[Required]
		[StringLength(NameMaxLenght)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[StringLength(DescriptionMaxLenght)]
		public string Description { get; set; } = string.Empty;

		public string? Image { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		public int CategoryId { get; set;}

		[ForeignKey(nameof(CategoryId))]
		public Category Category { get; set; } = null!;

		public ICollection<CartProduct> CartProducts { get; set; } = new HashSet<CartProduct>();

		public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
