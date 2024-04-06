using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Models;
using System.Diagnostics;

namespace RestaurantApp.Controllers
{
	public class HomeController : BaseController
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IEventService eventService;

		public HomeController(ILogger<HomeController> logger, IEventService eventService)
		{
			_logger = logger;
			this.eventService = eventService;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			if (User.IsInRole("Administrator"))
			{
				return RedirectToAction("Index", "Home", new { Area = "Administrator" });
			}
			var userId = GetUserId();

			var model = await eventService.GetAllEventsAsync();
			if (userId != null)
			{
				var eventsIds = await eventService.GetAllBoockedEventIdsAsync(userId);

				ViewBag.EventsIds = eventsIds;
			}

			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[AllowAnonymous]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(int statusCode)
		{
			if (statusCode == 404)
			{
				return View("Error404");
			}

			if (statusCode == 500)
			{
				return View("Error500");
			}

			return View();
		}
	}
}
