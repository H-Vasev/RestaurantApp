using Microsoft.EntityFrameworkCore;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Models.Chat;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.Core.Services
{
	public class ChatService : IChatService
	{
		private readonly ApplicationDbContext dbContext;

		public ChatService(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task AddMessageAsync(string? userId, string? userName, string message)
		{
				var chat = await dbContext.Chats
					.Where(c => c.ChatUserId == Guid.Parse(userId))
					.FirstOrDefaultAsync();

				if (chat == null)
				{
					chat = new Chat()
					{
						ChatUserId = Guid.Parse(userId),
						Username = userName,
						IsRead = false,
					};

					await dbContext.AddAsync(chat);
				}

				var chatMessages = await dbContext.ChatMessages
					.Where(m => m.ChatId == chat.Id)
					.ToListAsync();

				chatMessages.Add(new ChatMessage()
				{
					Message = message,
					CreatedAt = DateTime.UtcNow,
					SenderName = userName,
				});

				chat.ChatMessages = chatMessages;

				await dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<ChatViewModel>> GetAllUnreadChatsAsync()
		{
			return await dbContext.Chats
				.Where(c => c.IsRead == false)
				.Select(c => new ChatViewModel()
				{
					Id = c.Id.ToString(),
					ChatUserId = c.ChatUserId.ToString(),
					Username = c.Username,
					IsRead = c.IsRead,
				}).ToArrayAsync();
		}

		public async Task<ChatViewModel> GetUserChatAsync(string id)
		{
			var chat = await dbContext.Chats
				.Where(c => c.Id == Guid.Parse(id))
				.Select(c => new ChatViewModel()
				{
					Id = c.Id.ToString(),
					ChatUserId = c.ChatUserId.ToString(),
					Username = c.Username,
					IsRead = true,
					MessagesModels = c.ChatMessages
						.OrderBy(m => m.CreatedAt)
						.Select(m => new ChatMessagesViewModel()
						{
							ChatId = m.ChatId.ToString(),
							Message = m.Message,
							CreatedAt = m.CreatedAt,
							Sender = m.SenderName,
						}).ToArray(),
				}).FirstOrDefaultAsync();

			if (chat == null)
			{
				throw new ArgumentNullException(nameof(chat));
			}

			await MarkAsReadAsync(id);

			return chat;
		}

		public async Task<bool> IsAnyUserChatAsync(string userId)
		{
			return await dbContext.Chats
				.AsNoTracking()
				.AnyAsync(u => u.ChatUserId == Guid.Parse(userId));
		}

		public async Task MarkAsReadAsync(string id)
		{
			var chat = await dbContext.Chats
				.FindAsync(Guid.Parse(id));

			if (chat == null)
			{
				throw new ArgumentNullException(nameof(chat));
			}

			chat.IsRead = true;

			await dbContext.SaveChangesAsync();
		}

		public async Task MarkAsUnReadAsync(string? userId)
		{
			var chat = dbContext.Chats
				.Where(c => c.ChatUserId == Guid.Parse(userId))
				.FirstOrDefault();

			if (chat == null)
			{
				throw new ArgumentNullException(nameof(chat));
			}

			chat.IsRead = false;

			await dbContext.SaveChangesAsync();
		}

		public async Task<bool> IsAnyUnReadableChatAsync()
		{
			return await dbContext.Chats
				.AsNoTracking()
				.AnyAsync(c => c.IsRead == false);
		}
	}
}
