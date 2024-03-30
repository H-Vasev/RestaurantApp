using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Reservation;

namespace RestaurantApp.Controllers
{
	public class ReservationController : BaseController
	{
		private readonly IReservationService reservationService;
		private readonly IEventService eventService;

		public ReservationController(IReservationService reservationService, IEventService eventService)
		{
			this.reservationService = reservationService;
			this.eventService = eventService;
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

		[HttpGet]
		public async Task<IActionResult> Add(int id)
		{
			var model = await reservationService.PrepareReservationFormModelAsync(id);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(ReservationFormModel model, int id)
		{
			//if (DateTime.Parse(model.Date) < DateTime.Now)
			//{
			//	ModelState.AddModelError(model.Date, "Date must be biger than today!");
			//	TempData["Error"] = "Date must be biger than today!";
			//}

			//if (!ModelState.IsValid)
			//{
			//	return View(model);
			//}

			//var userId = GetUserId();

			//var ev = await eventService.GetEventByIdAsync(id);

			//if (ev != null)
			//{
			//	model.EventId = ev.Id;
			//	model.EventName = ev.Title;
			//	model.Date = ev.StartEvent.ToString("g");
			//}
			//var date = DateTime.Parse(model.Date);
			//var isReserved = await reservationService.IsReservedAsync(date, userId);

			//if (isReserved)
			//{
			//	TempData["Reserved"] = "You have already made a reservation for this date or check yuor Reservation!";
			//	return View(model);
			//}

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
			if (DateTime.Parse(model.Date) < DateTime.Now)
			{
				ModelState.AddModelError(model.Date, "Date must be biger than today!");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var userId = GetUserId();

			try
			{
				await reservationService.EditReservationAsync(model, userId, id);
			}
			catch (Exception)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(Index));
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
