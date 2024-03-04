using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Areas.Administrator.Models.Product;
using RestaurantApp.Core.Contracts;

namespace RestaurantApp.Areas.Administrator.Controllers
{
	public class ProductController : BaseAdministratorController
	{
		private readonly IMenuService menuService;
		private readonly IProductService productService;

        public ProductController(IMenuService menuService, IProductService productService)
        {
			this.menuService = menuService;
			this.productService = productService;
        }

        public async Task<IActionResult> Index(string? category)
		{
			var model = await menuService.GetMenuAsync(category);

			ViewBag.Categories = await menuService.GetCategoriesAsync();
			ViewBag.CurrentCategory = category;

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var model = new ProductFormModel();
			model.Categories = await menuService.GetCategoriesAsync();

			return View(model);
		}
	}
}
