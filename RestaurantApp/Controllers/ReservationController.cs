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

		public async Task<IActionResult> Index()
		{
			var userId = GetUserId();

			try
			{
				var model = await reservationService.GetAllMineReservationAsync(userId);
				return View(model);
			}
			catch (Exception)
			{
				return BadRequest();
			}

		}

		[HttpGet]
		public async Task<IActionResult> Add(int id)
		{
			var email = GetUserEmailAddress();
			var model = await reservationService.PrepareReservationFormModelAsync(id, email);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(ReservationFormModel model, int id)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var userId = GetUserId();
			var result = await reservationService.AddReservationAsync(model, userId, id);

			if (!string.IsNullOrEmpty(result))
			{
				TempData["Error"] = result;
				return View(model);
			}

			TempData["Success"] = "Reservation is successful!";

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var userId = GetUserId();
			try
			{
				var model = await reservationService.GetReservationByIdAsync(userId, id);
				return View(model);

			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(ReservationFormModel model, string id)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var userId = GetUserId();

			try
			{
				var result = await reservationService.EditReservationAsync(model, userId, id);

				if (!string.IsNullOrEmpty(result))
				{
					TempData["Error"] = result;
					return View(model);
				}
				else
				{
					TempData["Success"] = "Reservation is Edited successfuly!";
				}
			}
			catch (Exception)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> AllFullyBookedReservation()
		{
			var allFullyBookedReservations = await reservationService.GetAllFullyBookedDatesInReservationAsync();

			return Json(allFullyBookedReservations);
		}


		public async Task<IActionResult> Cancel(string id)
		{
			var userId = GetUserId();

			try
			{
				await reservationService.RemoveMineReservationAsync(userId, id);
			}
			catch (Exception)
			{

				return BadRequest();
			}

			TempData["Success"] = "Your reservation is canceled successfully!";

			return RedirectToAction(nameof(Index));
		}

	}
}
