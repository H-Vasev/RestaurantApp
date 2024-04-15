namespace RestaurantApp.Core.Models.Reservation
{
	public class ReservationQuaryModel
	{
		public int TotalPageRecords { get; set; }

		public int CurrentPage { get; set; }

		public int RecordsPerPage { get; set; } = 10;

		public IEnumerable<ReservationTableViewModel> Reservations { get; set; } = new HashSet<ReservationTableViewModel>();
    }
}
