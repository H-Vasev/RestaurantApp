using RestaurantApp.Core.Models.Reservation;
using System.Text;

namespace RestaurantApp.Core.Contracts
{
    public interface IReservationService
	{
		Task<string> AddReservationAsync(ReservationFormModel model, string userId, int id);

		Task<StringBuilder?> DownloadAllFilteredReservationsAsync(string? name, DateTime? startDate, DateTime? endDate);

		Task<string> EditReservationAsync(ReservationFormModel model, string userId, string id);

		Task<string[]> GetAllFullyBookedDatesInReservationAsync();

		Task<IEnumerable<ReservationViewModel>> GetAllMineReservationAsync(string userId);

		Task<ReservationQuaryModel?> GetAllReservationsAsync(int? pageNumber, DateTime? startDate, DateTime? endDate, string? name);

		Task<ReservationFormModel> GetReservationByIdAsync(string userId, string id);

        Task<bool> IsReservedAsync(DateTime date, string userId);

		Task<ReservationFormModel> PrepareReservationFormModelAsync(int id, string email);

		Task RemoveMineReservationAsync(string userId, string id);

		Task<string> TryUpdateReservationByDateAsync(ReservationFormModel model, string userId);

		Task<string?> UpdateCapacityWhenAddReservationAsync(int? capacitySlotId, int peopleCount, string userId);

	}
}
