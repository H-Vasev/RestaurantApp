using Microsoft.Extensions.Configuration;
using RestaurantApp.Core.Contracts;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RestaurantApp.Core.Services
{
    public class EmailSender : IEmailSender
	{
		private IConfiguration configuration;

		public EmailSender(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public async Task SendConfirmEmailAsync(string toEmail, string subject, string message)
		{
			var apiKey = configuration.GetSection("SendGrid:ApiKey").Value;
			var client = new SendGridClient(apiKey);

            var fromEmail = configuration.GetSection("EmailAddress:Key").Value;

            var from = new EmailAddress(fromEmail, "RestaurantApp");
			var to = new EmailAddress(toEmail);

			var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);

			var response = await client.SendEmailAsync(msg);

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidOperationException("Failed to send email");
			}
		}

		public async Task SendEmailAsync(string name, string fromEmail, string subject, string message)
		{
			var apiKey = configuration.GetSection("SendGrid:ApiKey").Value;
			var client = new SendGridClient(apiKey);

			//Need real email addresses. Already tested with real email addresses and work.
			var from = new EmailAddress("fromYourEmail@gmail.com", "RestaurantApp");
			var to = new EmailAddress("toYourEmail@gmail.com");

			var plainTextContent = $"Message from {name} ({fromEmail}): {message}";
			var htmlContent = $"<strong>Message from {name} ({fromEmail}):</strong><p>{message}</p>";

			var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

			var response = await client.SendEmailAsync(msg);

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidOperationException("Failed to send email");
			}
		}
	}
}
