using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;

namespace RestaurantApp.Areas.Administrator.Controllers
{
	public class ProductController : BaseAdministratorController
	{
		private readonly IMenuService menuService;

        public ProductController(IMenuService menuService)
        {
			this.menuService = menuService;
        }

        public async Task<IActionResult> Index(string? category)
		{
			var model = await menuService.GetMenuAsync(category);

			ViewBag.Categories = await menuService.GetCategoriesAsync();
			ViewBag.CurrentCategory = category;

			return View(model);
		}
	}
}
