using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Account;
using RestaurantApp.Infrastructure.Data.Models;
using System.Text.Encodings.Web;

namespace RestaurantApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;

        private readonly ITownService townService;


        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITownService townService,
            IEmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.townService = townService;
            this.emailSender = emailSender;
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
            var userExist = await userManager.FindByEmailAsync(model.Email);

            if (userExist != null)
            {
                ModelState.AddModelError("Email", "This email is already registered. Please use a different email or log in.");
            }

            var passwordErrors = new List<IdentityError>();
            foreach (var validator in userManager.PasswordValidators)
            {
                var validatorresult = await validator.ValidateAsync(userManager, null, model.Password);
                if (!validatorresult.Succeeded)
                {
                    passwordErrors.AddRange(validatorresult.Errors);
                }
            }

            if (passwordErrors.Any())
            {
                foreach (var error in passwordErrors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
            }

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

			var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
			var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            try
            {
				await emailSender.SendConfirmEmailAsync(model.Email, "Confirm your account",
				$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
			}
            catch (Exception)
            {
                return BadRequest();
            }
		
           // await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
				throw new InvalidOperationException($"Unable to load user with ID '{userId}'.");
			}

            var result = await userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            ViewBag.SuccessVerified = "Your account has been successfully verified.";
            return View();
        }

		[AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var model = new LoginFormModel();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel model, string? returnUrl)
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

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
