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
		Task AddReservationAsync(ReservationFormModel model, string userId);
		Task EditReservationAsync(ReservationFormModel model, string userId, string id);
		Task<IEnumerable<ReservationViewModel>> GetAllReservationAsync(string userId);
        Task<ReservationFormModel> GetReservationByIdAsync(string userId, string id);
        Task<bool> IsReservedAsync(DateTime date, string userId);
		Task RemoveReservationAsync(string userId, string id);
    }
}
