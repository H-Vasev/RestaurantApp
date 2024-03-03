using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Event;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class Event
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(TitleMaxLenght)]
		public string Title { get; set; } = string.Empty;

		[StringLength(DescriptionMaxLenght)]
		public string? Description { get; set; }

		public DateTime StartEvent { get; set; } = DateTime.Now;

		public DateTime EndEvent { get; set; } = DateTime.Now;

		public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
	}
}
