namespace RestaurantApp.Core.Models.Reservation
{
	public class ReservationTableViewModel
	{
		public string Id { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public string PhoneNumber { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string Date { get; set; } = string.Empty;

		public string? UserId { get; set; }

		public int PeopleCount { get; set; }
	}
}
