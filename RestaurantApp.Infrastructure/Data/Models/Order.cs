using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class Order
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public ApplicationUser ApplicationUser { get; set; } = null!;

		[Required]
		public DateTime OrderDate { get; set; }

		[Required]
		public decimal TotalPrice { get; set; }

		public string? Status { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
	}
}
