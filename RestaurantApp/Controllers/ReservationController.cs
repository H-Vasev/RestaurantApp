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

		[HttpPost]
		public async Task<IActionResult> Add(ReservationFormModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var userId = GetUserId();

			await reservationService.AddReservationAsync(model, userId);

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Index()
		{
			var userId = GetUserId();

			try
			{
                var model = await reservationService.GetAllReservationAsync(userId);
                return View(model);
            }
            catch (Exception)
			{
				return BadRequest();
			}

		}

		public async Task<IActionResult> Cancel(string id)
		{
			var userId = GetUserId();

			try
			{
                await reservationService.RemoveReservationAsync(userId, id);
            }
            catch (Exception)
			{

				return BadRequest();
			}

			return RedirectToAction(nameof(Index));
		}

    }
}
