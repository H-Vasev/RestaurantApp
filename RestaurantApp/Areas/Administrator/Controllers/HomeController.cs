using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;

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
    }
}
