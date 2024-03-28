using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Core.Models.Chat
{
	public class ChatMessagesViewModel
	{
		public string ChatId { get; set; } = null!;

		[StringLength(500)]
		public string Message { get; set; } = null!;

		public string? Sender { get; set; }

		public DateTime CreatedAt { get; set; }
	}
}
