using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.ShoppingCart;

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
			var model = await orderService.GetDataForCheckoutAsync(userId);

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
				TempData["OrderSuccess"] = "You have successfully placed your order!";
			}
			catch (Exception)
			{
				return BadRequest();
			}
			var userDeliveryInfo = GetUserDeliveryInfo(model.Address,
													   model.Name,
													   model.TotalPrice,
													   model.PostalCode,
													   model.Email);


			var json = JsonConvert.SerializeObject(userDeliveryInfo);
			SetCookieForDeliveryInfo("UserDeliveryInfo", json);

			return RedirectToAction(nameof(OrderSuccess));
		}

		public IActionResult OrderSuccess()
		{
			if (!TempData.ContainsKey("OrderSuccess"))
			{
                return RedirectToAction(nameof(Index));
            }

			var cookieDeliveryInfo = GetCookieValue("UserDeliveryInfo");
			if (!string.IsNullOrEmpty(cookieDeliveryInfo))
			{
				var model = DeserializeDeliveryInfo(cookieDeliveryInfo);

                return View(model);
            }
			else
			{
				return BadRequest();
			}
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

		private string GetCookieValue(string key)
		{
            if (Request.Cookies.TryGetValue(key, out string? value))
			{
                return value;
            }

            return string.Empty;
        }

		private void SetCookieForDeliveryInfo(string key, string value, CookieOptions? options = null)
		{
             options ??= new CookieOptions
			 {
			     HttpOnly = true,
			     Secure = true,
			 	SameSite = SameSiteMode.Strict
			 };

            Response.Cookies.Append(key, value, options);
        }

        private object DeserializeDeliveryInfo(string userDeliveryInfoJson)
        {
            var deliveryInfo = JsonConvert.DeserializeObject<UserDeliveryInfo>(userDeliveryInfoJson);

            return new UserDeliveryInfo()
            {
                Name = deliveryInfo.Name,
                EmailAddress = deliveryInfo.EmailAddress,
                Address = deliveryInfo.Address,
                PostalCode = deliveryInfo.PostalCode,
                Price = deliveryInfo.Price
            };
        }

        private object GetUserDeliveryInfo(string address, string name, decimal totalPrice, string postalCode, string email)
		{
			return new UserDeliveryInfo
			{
                Address = address,
                Name = name,
                Price = totalPrice,
                PostalCode = postalCode,
                EmailAddress = email
            };
		}
    }
}
