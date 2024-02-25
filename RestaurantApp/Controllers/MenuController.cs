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
		public async Task<IActionResult> Index(string? category)
		{
			var model = await menuService.GetMenuAsync(category);

            ViewBag.Categories = await menuService.GetCategoriesAsync();
            ViewBag.CurrentCategory = category;

			return View(model);
		}
	}
}
