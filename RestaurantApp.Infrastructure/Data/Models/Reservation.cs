using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Reservation;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class Reservation
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(FirstNameMaxLenght)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[StringLength(LastNameMaxLenght)]
		public string LastName { get; set; } = string.Empty;

		[Required]
		[StringLength(PhoneNumberMaxLenght)]
		public string PhoneNumber { get; set; } = string.Empty;

		[Required]
		[StringLength(EmailMaxLenght)]
		public string Email { get; set; } = string.Empty;

		public DateTime Date { get; set; } = DateTime.Now;

		[Required]
		public int PeopleCount { get; set; }

		[StringLength(DescriptionMaxLenght)]
		public string? Description { get; set; }

		public Guid? ApplicationUserId { get; set; }

		[ForeignKey(nameof(ApplicationUserId))]
		public ApplicationUser? ApplicationUser { get; set; }

		public int? EventId { get; set; }

		[ForeignKey(nameof(EventId))]
		public Event? Event { get; set; }


		public int? CapacitySlotId { get; set; }

		[ForeignKey(nameof(CapacitySlotId))]
		public CapacitySlot? CapacitySlot { get; set; }
	}
}
