using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Add(EventFormModel model)
        {          
			if (model.StartEvent < DateTime.Now)
			{
				TempData["ErrorDate"] = "Start date must be valid date!";
				ModelState.AddModelError("", "Start date must be valid date!");
			}

			if (model.EndEvent <= model.StartEvent)
			{
				TempData["ErrorDate"] = "Start date must be after end date!";
				ModelState.AddModelError("", "Start date must be after end date!");
			}

			if (!ModelState.IsValid)
            {
				return View(model);
			}

			await eventService.AddEventAsync(model);

			return RedirectToAction(nameof(Index));
		}
    }
}
