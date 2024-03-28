using Microsoft.AspNetCore.Mvc;

namespace RestaurantApp.Controllers
{
	public class ChatController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
