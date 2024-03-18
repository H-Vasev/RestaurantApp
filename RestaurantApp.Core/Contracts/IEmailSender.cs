namespace RestaurantApp.Core.Contracts
{
	public interface IEmailSender
	{
		Task SendEmailAsync(string name, string toEmail, string subject, string message);
	}
}
