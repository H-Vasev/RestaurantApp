using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Services;

namespace RestaurantApp.Controllers
{
	public class MenuController : BaseController
	{
		private readonly IMenuService menuService;
		private readonly IShoppingCartService shoppingCartService;

        public MenuController(IMenuService menuService, IShoppingCartService shoppingCartService)
		{
			this.menuService = menuService;
            this.shoppingCartService = shoppingCartService;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index(string? category)
		{
			var model = await menuService.GetMenuAsync(category);

            ViewBag.Categories = await menuService.GetCategoriesAsync();
            ViewBag.CurrentCategory = category;

			return View(model);
		}

        public async Task<IActionResult> AddToCart(int id)
        {
            var userId = GetUserId();

            try
            {
                await shoppingCartService.AddToCartAsync(userId, id);
            }
            catch (Exception)
            {
                return BadRequest();
            }

			TempData["SuccessAdd"] = "Successfully add product to your basket!";
			return RedirectToAction(nameof(Index));
        }
    }
}
