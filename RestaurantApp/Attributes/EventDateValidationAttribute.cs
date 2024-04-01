using Microsoft.AspNetCore.Mvc.Filters;
using RestaurantApp.Core.Models.Event;

namespace RestaurantApp.Attributes
{
	public class EventDateValidationAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			if (context.ActionArguments.TryGetValue("model", out var value) && value is EventFormModel model)
			{
				if (model.StartEvent < DateTime.Now.AddHours(-1))
				{
					context.ModelState.AddModelError("", "Start date must be a future date!");
					context.HttpContext.Items["ErrorDate"] = "Start date must be a future date!";
				}

				if (model.EndEvent <= model.StartEvent)
				{
					context.ModelState.AddModelError("", "End date must be after start date!");
					context.HttpContext.Items["ErrorDate"] = "End date must be after start date!";
				}
			}
		}
	}
}
