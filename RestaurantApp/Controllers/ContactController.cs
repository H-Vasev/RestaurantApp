using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Contact;

namespace RestaurantApp.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IEmailSender emailSender;

        public ContactController(IEmailSender emailSender)
        {
			this.emailSender = emailSender;
		}

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = new ContactFormModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendContactForm(ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
				return View(model);
			}

            try
            {
				await emailSender.SendEmailAsync(model.Name, model.Email, model.Subject, model.Message);
			}
			catch (Exception)
            {
                return BadRequest();
            }

            TempData["Success"] = "Your message has been sent successfully!";
            return RedirectToAction(nameof(Index));
		}
    }
}
