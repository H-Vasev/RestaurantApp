using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Attributes;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Event;

namespace RestaurantApp.Areas.Administrator.Controllers
{
    public class HomeController : BaseAdministratorController
    {
        private readonly IEventService eventService;

        public HomeController(IEventService eventService)
        {
			this.eventService = eventService;
		}

        public async Task<IActionResult> Index()
        {
            var model = await eventService.GetAllEventsAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new EventFormModel();

            return View(model);
        }

        [HttpPost]
		[EventDateValidation]
        public async Task<IActionResult> Add(EventFormModel model)
        {          
			if (!ModelState.IsValid)
            {
				TempData["ErrorDate"] = HttpContext.Items["ErrorDate"];
				return View(model);
			}

			try
			{
				await eventService.AddEventAsync(model);
			}
			catch (Exception)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var model = await eventService.GetEventByIdForEditAsync(id);

				return View(model);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPost]
		[EventDateValidation]
		public async Task<IActionResult> Edit(EventFormModel model, int id)
		{
			if (!ModelState.IsValid)
			{
				TempData["ErrorDate"] = HttpContext.Items["ErrorDate"];
				return View(model);
			}

			try
			{
				await eventService.EditEventAsync(model, id);
			}
			catch (Exception)
			{
				return BadRequest();
			}
			

			return RedirectToAction(nameof(Index));
		}

        public async Task<IActionResult> Remove(int id)
        {
			try
			{
				await eventService.RemoveEventAsync(id);
			}
			catch (Exception)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(Index));
		}
    }
}
