using Humanizer.Localisation.TimeToClockNotation;
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
		public async Task<IActionResult> Edit(EventFormModel model, int id)
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
