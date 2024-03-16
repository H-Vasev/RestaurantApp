using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.ShoppingCart;
using System.Linq;

namespace RestaurantApp.Controllers
{
	public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService shoppingCartService;
		private readonly IOrderService orderService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService)
        {
			this.shoppingCartService = shoppingCartService;
			this.orderService = orderService;
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

		public async Task<IActionResult> Checkout()
		{
			var userId = GetUserId();
			var model = await orderService.GetDataForCheoutAsync(userId);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Checkout(CheckoutFormModel model)
		{
			if (model.Items.Count() < 1)
			{
				ModelState.AddModelError("", "Your shopping cart is empty!");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var userId = GetUserId();

				await orderService.CheckoutAsync(model, userId);
				await shoppingCartService.ClearCartAsync(userId);
				TempData["OrderSuccess"] = true;
			}
			catch (Exception)
			{
				return BadRequest();
			}

			return RedirectToAction(nameof(OrderSuccess));
		}

		public IActionResult OrderSuccess()
		{
			if (!TempData.ContainsKey("OrderSuccess"))
			{
                return RedirectToAction(nameof(Index));
            }

			return View();
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
