using RestaurantApp.Core.Models.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Core.Contracts
{
	public interface IReservationService
	{
		Task<string> AddReservationAsync(ReservationFormModel model, string userId, int id);

		Task<string> EditReservationAsync(ReservationFormModel model, string userId, string id);

		Task<string[]> GetAllFullyBookedReservationAsync();

		Task<IEnumerable<ReservationViewModel>> GetAllReservationAsync(string userId);

        Task<ReservationFormModel> GetReservationByIdAsync(string userId, string id);

        Task<bool> IsReservedAsync(DateTime date, string userId);

		Task<ReservationFormModel> PrepareReservationFormModelAsync(int id);

		Task RemoveReservationAsync(string userId, string id);
    }
}
