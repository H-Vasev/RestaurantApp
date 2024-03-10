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

            var model = await shoppingCartService.GetAllItemsAsync(userId);

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

		public async Task<IActionResult> RemoveFromCart(int id)
		{
			var userId = GetUserId();

			try
			{
                await shoppingCartService.RemoveFromCartAsync(userId, id);
            }
            catch (Exception)
			{
                return BadRequest();
            }

			TempData["SuccessRemove"] = "Product removed from cart successfully!";
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> GetCartItemsCount()
		{
			var userId = GetUserId();

			if (!string.IsNullOrEmpty(userId))
			{
				var cartItemsCount = await shoppingCartService.GetItamsQuantityAsync(userId);

				return Json(new {Count = cartItemsCount});
			}
			else
			{
				return Json(new {Count = 0});
			}
		}
    }
}
