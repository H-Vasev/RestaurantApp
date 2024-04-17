
using RestaurantApp.Core.Models.Chat;

namespace RestaurantApp.Core.Contracts
{
	public interface IChatService
	{
		Task AddMessageAsync(string? userId, string? userName, string message);

		Task<IEnumerable<ChatViewModel>> GetAllUnreadChatsAsync();

		Task<ChatViewModel> GetUserChatAsync(string id);

		Task<bool> IsAnyUserChatAsync(string userId);

		Task MarkAsUnReadAsync(string? userIdentifier);

		Task MarkAsReadAsync(string id);

        Task<bool> IsAnyUnReadableChatAsync();
	}
}
