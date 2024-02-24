using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantApp.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
