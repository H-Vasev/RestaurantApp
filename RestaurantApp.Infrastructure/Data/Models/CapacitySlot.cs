using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class CapacitySlot
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime SlotDate { get; set; }

		[Required]
		public int TotalCapacity { get; set; } = 100;

		[Required]
		public int CurrentCapacity { get; set; }

		public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
	}
}
