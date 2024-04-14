using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RestaurantApp.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
			return User.FindFirstValue(ClaimTypes.NameIdentifier);
		}

        protected string GetUserEmailAddress()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }
    }
}
