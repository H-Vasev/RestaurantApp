using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;

namespace RestaurantApp.Controllers
{
	public class MenuController : BaseController
	{
		private readonly IMenuService menuService;

		public MenuController(IMenuService menuService)
		{
			this.menuService = menuService;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var model = await menuService.GetMenuAsync();

			return View(model);
		}
	}
}
