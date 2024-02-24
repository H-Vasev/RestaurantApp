using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Account;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly ITownService townService;


        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITownService townService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.townService = townService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterFormModel();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var town = new Town();
            var isTownExist = await townService.GetTownByNameAsync(model.City);

            if (isTownExist != null)
            {
                town.Id = isTownExist.Id;
                town.TownName = isTownExist.Name;
            }
            else
            {
                town.TownName = model.City;
            }

            var address = new Address
            {
                Street = model.Street,
                PostalCode = model.PostalCode,
                Town = town
            };

            var shoppingCart = new ShoppingCart();

            var user = new ApplicationUser
            {
                FirsName = model.FirstName,
                LastName = model.LastName,
                AddressId = address.Id,
                Address = address,
                ShoppingCartId = shoppingCart.Id,
                ShoppingCart = shoppingCart
            };

            await userManager.SetEmailAsync(user, model.Email);
            await userManager.SetUserNameAsync(user, model.Username);

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginFormModel();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
