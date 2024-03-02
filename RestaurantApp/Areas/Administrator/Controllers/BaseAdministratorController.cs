using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantApp.Areas.Administrator.Controllers
{
	[Authorize(Roles = "Administrator")]
	[Area("Administrator")]
	public class BaseAdministratorController : Controller
	{
	}
}
