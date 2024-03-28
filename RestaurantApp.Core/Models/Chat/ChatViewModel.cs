using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Core.Models.Chat
{
	public class ChatViewModel
	{
		public string Id { get; set; } = null!;

		public string ChatUserId { get; set; } = null!;

		public string Username { get; set; } = null!;

		public bool IsRead { get; set; }

		public IEnumerable<ChatMessagesViewModel> MessagesModels { get; set; } = new HashSet<ChatMessagesViewModel>();
	}
}
