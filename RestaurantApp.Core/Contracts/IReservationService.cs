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

        Task<IEnumerable<ReservationViewModel>> GetAllReservationAsync(string userId);
		Task<bool> IsReservedAsync(DateTime date, string userId);
		Task RemoveReservationAsync(string userId, string id);
    }
}
