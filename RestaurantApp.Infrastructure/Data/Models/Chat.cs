using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Chat;

namespace RestaurantApp.Infrastructure.Data.Models
{
	public class Chat
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(UsernameMaxLenght)]
		public string Username { get; set; } = null!;

		[Required]
		public bool IsRead { get; set; }

		public Guid? ChatUserId { get; set; }

		[ForeignKey(nameof(ChatUserId))]
		public ApplicationUser? ChatUser { get; set; }

		public IEnumerable<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();
	}
}
