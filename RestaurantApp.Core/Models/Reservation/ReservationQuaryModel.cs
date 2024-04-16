namespace RestaurantApp.Core.Models.Reservation
{
	public class ReservationQuaryModel
	{
		public int TotalPageRecords { get; set; }

		public int CurrentPage { get; set; }

		public int RecordsPerPage { get; set; } = 10;

		public string? Name { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public IEnumerable<ReservationTableViewModel> Reservations { get; set; } = new HashSet<ReservationTableViewModel>();
    }
}
