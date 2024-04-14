namespace RestaurantApp.Core.Models.Reservation
{
	public class ReservationQuaryModel
	{
		public int TotalPageRecords { get; set; }

		public int CurrentPage { get; set; }

		public IEnumerable<ReservationTableViewModel> Reservations { get; set; } = new HashSet<ReservationTableViewModel>();
    }
}
