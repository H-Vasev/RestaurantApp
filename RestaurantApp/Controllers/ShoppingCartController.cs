using Microsoft.AspNetCore.Http.Extensions;
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

			return RedirectToAction("Index", "Menu");
		}
    }
}
