using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;

namespace RestaurantApp.Controllers
{
	public class MenuController : BaseController
	{
		private readonly IMenuService menuService;
		private readonly IShoppingCartService shoppingCartService;
		private readonly IOrderService orderService;

        public MenuController(IMenuService menuService, IShoppingCartService shoppingCartService, IOrderService orderService)
		{
			this.menuService = menuService;
            this.shoppingCartService = shoppingCartService;
			this.orderService = orderService;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index(string? category)
		{
			if (User.IsInRole("Administrator"))
			{
				return RedirectToAction("Index", "Product", new { Area = "Administrator" });
			}

			var model = await menuService.GetMenuAsync(category);

            ViewBag.Categories = await menuService.GetCategoriesAsync();
            ViewBag.CurrentCategory = category;
			ViewBag.UserId = GetUserId();
			if (ViewBag.UserId != null)
			{
				ViewBag.LastOrders = await orderService.GetOrdersAsync(ViewBag.UserId);
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
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

			return RedirectToAction(nameof(Index));
        }
    }
}
