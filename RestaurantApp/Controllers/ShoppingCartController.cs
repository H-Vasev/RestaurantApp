using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;

namespace RestaurantApp.Controllers
{
	public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
			this.shoppingCartService = shoppingCartService;
		}
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var model = await shoppingCartService.GetShoppingCartAsync(userId);

            return View(model);
        }
    }
}
