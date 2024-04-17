using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using System.Globalization;
using System.Text;

namespace RestaurantApp.Areas.Administrator.Controllers
{
	public class ReservationController : BaseAdministratorController
	{
		private readonly IReservationService reservationService;

		public ReservationController(IReservationService reservationService)
		{
			this.reservationService = reservationService;
		}

		public async Task<IActionResult> Index(int? pageNumber, string? startDate, string? endDate, string? name)
		{
			DateTime? start = null;
			DateTime? end = null;
			try
			{
				if (!string.IsNullOrWhiteSpace(startDate))
				{
					start = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
				}

				if (!string.IsNullOrWhiteSpace(endDate))
				{
					end = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
				}

				var reservations = await reservationService.GetAllReservationsAsync(pageNumber, start, end, name);
				reservations!.CurrentPage = pageNumber ?? 1;

				return View(reservations);
			}
			catch (Exception)
			{
				TempData["Error"] = "The end date you entered is invalid. Please enter a valid End date.";
				return RedirectToAction(nameof(Index));
			}
		}

		public async Task<IActionResult> DownloadReservations(string? name, string? startDate, string? endDate)
		{
			DateTime? start = null;
			DateTime? end = null;

			if (!string.IsNullOrWhiteSpace(startDate))
			{
				start = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
			}
			
			if (!string.IsNullOrWhiteSpace(startDate))
			{
				end = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
			}

			StringBuilder? reservations = await reservationService.DownloadAllFilteredReservationsAsync(name, start, end);
			if (reservations == null)
			{
				TempData["Error"] = "The end date you entered is invalid. Please enter a valid End date.";
				return RedirectToAction(nameof(Index));
			}

			return File(Encoding.UTF8.GetBytes(reservations.ToString()), "text/csv", "reservations.csv");
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
