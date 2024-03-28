using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.ChatMessage;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class ChatMessage
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(MessageMaxLenght)]
		public string Message { get; set; } = null!;

		public Guid? SenderId { get; set; }

		[Required]
		[StringLength(SenderNameMaxLenght)]
		public string SenderName { get; set; } = null!;

		[Required]
		public DateTime CreatedAt { get; set; }

		public Guid ChatId { get; set; }

		[ForeignKey(nameof(ChatId))]
		public Chat Chat { get; set; } = null!;
	}
}
