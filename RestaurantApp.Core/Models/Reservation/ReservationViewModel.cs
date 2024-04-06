namespace RestaurantApp.Core.Models.Reservation
{
	public class ReservationViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? EventName { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public int PeopleCount { get; set; }

        public string? Description { get; set; }

        public Guid? ApplicationUserId { get; set; }

        public int? EventId { get; set; }
    }
}
