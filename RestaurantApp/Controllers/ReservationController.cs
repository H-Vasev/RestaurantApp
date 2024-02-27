using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Reservation;

namespace RestaurantApp.Controllers
{
	public class ReservationController : BaseController
	{
		private readonly IReservationService reservationService;

		public ReservationController(IReservationService reservationService)
		{
			this.reservationService = reservationService;
		}

		[HttpGet]
		public IActionResult Add()
		{
			var model = new ReservationFormModel();

			return View(model);
		}
	}
}
