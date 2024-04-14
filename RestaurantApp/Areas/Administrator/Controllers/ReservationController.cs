using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;

namespace RestaurantApp.Areas.Administrator.Controllers
{
    public class ReservationController : BaseAdministratorController
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
			this.reservationService = reservationService;
		}

        public async Task<IActionResult> Index(int? pageNumber, DateTime? startDate, DateTime? endDate, string? name)
        {
            try
            {
				var reservations = await reservationService.GetAllReservationsAsync(pageNumber, startDate, endDate, name);
				if (reservations == null)
				{
					TempData["Error"] = "The end date you entered is invalid. Please enter a valid End date.";
				}
				else
				{
					reservations!.CurrentPage = pageNumber ?? 1;
				}

				return View(reservations);
			}
			catch (Exception)
            {
                return BadRequest();
            }
        }

		[HttpPost]
		public async Task<IActionResult> Remove(string id, string userId)
		{
			try
			{
				await reservationService.RemoveMineReservationAsync(userId, id);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
    }
}
