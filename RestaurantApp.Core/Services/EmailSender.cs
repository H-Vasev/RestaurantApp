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

		public async Task SendEmailAsync(string name, string toEmail, string subject, string message)
		{
			var apiKey = configuration.GetSection("SendGrid:ApiKey").Value;
			var client = new SendGridClient(apiKey);

			var fromEmail = configuration.GetSection("EmailAddress:Key").Value;

			var from = new EmailAddress(fromEmail, "RestaurantApp");
			var to = new EmailAddress(toEmail);

            var plainTextContent = $"Your message {message} was succesfully send to our Restaurant.";
			var htmlContent = $"<span>Your message </span><strong>('{message}'):</strong><p>Was succesfully send to our Restaurant.</p>";

			var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

			var response = await client.SendEmailAsync(msg);

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidOperationException("Failed to send email");
			}
		}
	}
}
