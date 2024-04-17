using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RestaurantApp.Core.Contracts;
using RestaurantApp.Core.Services;
using RestaurantApp.Data;
using RestaurantApp.Infrastructure.Data.Models;

namespace RestaurantApp.UnitTests
{
    public class ChatServeceTests
    {
        private DbContextOptions<ApplicationDbContext> dbContextOptions;
        private ApplicationDbContext dbContext;
        private IChatService chatService;

        [SetUp]
        public void Setup()
        {
            dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new ApplicationDbContext(dbContextOptions);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            chatService = new ChatService(dbContext);
        }

        //AddMessageAsync
        [Test]
        public async Task AddMessageAsync_WithValidData_ShouldNewChatAndNewMessage()
        {
            var userId = Guid.NewGuid().ToString();
            var userName = "TestUser";
            var message = "TestMessage";

            await chatService.AddMessageAsync(userId, userName, message);

            var chat = dbContext.Chats.FirstOrDefault();

             Assert.That(chat, Is.Not.Null);
             Assert.That(chat.ChatUserId, Is.EqualTo(Guid.Parse(userId)));
             Assert.That(chat.ChatMessages.Count, Is.EqualTo(1));
             Assert.That(chat.ChatMessages.First().Message, Is.EqualTo(message));
        }

        [Test]
        public async Task AddMessageAsync_WithValidData_ShouldAddNewMessage()
        {
            var userId = Guid.NewGuid().ToString();
            var userName = "TestUser";
            var message = "TestMessage";

            await dbContext.Chats.AddAsync(new Chat()
            {
                ChatUserId = Guid.Parse(userId),
                Username = userName,
                IsRead = false,
                ChatMessages = new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        Message = "OldMessage",
                        CreatedAt = DateTime.UtcNow,
                        SenderName = "OldUser",
                    }
                }
            });

            await dbContext.SaveChangesAsync();

            await chatService.AddMessageAsync(userId, userName, message);

            var chat = dbContext.Chats.FirstOrDefault();

            Assert.That(chat, Is.Not.Null);
            Assert.That(chat.ChatUserId, Is.EqualTo(Guid.Parse(userId)));
            Assert.That(chat.ChatMessages.Count, Is.EqualTo(2));
            Assert.That(chat.ChatMessages.Last().Message, Is.EqualTo(message));
        }

        //GetAllUnreadChatsAsync
        [Test]
        public async Task GetAllUnreadChatsAsync_WithUnreadChats_ShouldReturnUnreadChats()
        {
            var userId = Guid.NewGuid().ToString();
            var userName = "TestUser";
            var message = "TestMessage";

            await dbContext.Chats.AddRangeAsync(new Chat()
            {
                ChatUserId = Guid.Parse(userId),
                Username = userName,
                IsRead = false,
                ChatMessages = new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        Message = message,
                        CreatedAt = DateTime.UtcNow,
                        SenderName = userName,
                    }
                }
            },
            new Chat()
            {
                ChatUserId = Guid.Parse(userId),
                Username = "userName2",
                IsRead = true,
                ChatMessages = new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        Message = message,
                        CreatedAt = DateTime.UtcNow,
                        SenderName = userName,
                    }
                }
            });

            await dbContext.SaveChangesAsync();

            var chats = await chatService.GetAllUnreadChatsAsync();

            Assert.That(chats, Is.Not.Null);
            Assert.That(chats.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllUnreadChatsAsync_WithUnreadChats_ShouldReturnZeroCount()
        {
            var chats = await chatService.GetAllUnreadChatsAsync();

            Assert.That(chats.Count, Is.EqualTo(0));
        }

        //GetUserChatAsync
        [Test]
        public async Task GetUserChatAsync_WithValidId_ShouldReturnUserChat()
        {
            var userId = Guid.NewGuid();
            var chatId = Guid.NewGuid();
            var userName = "TestUser";
            var message = "TestMessage";

            await dbContext.Chats.AddRangeAsync(new Chat()
            {
                Id = chatId,
                ChatUserId = userId,
                Username = userName,
                IsRead = false,
                ChatMessages = new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        Message = message,
                        CreatedAt = DateTime.UtcNow,
                        SenderName = userName,
                    }
                }
            });

            await dbContext.SaveChangesAsync();

            var chat = await chatService.GetUserChatAsync(chatId.ToString().ToLower());

            Assert.That(chat, Is.Not.Null);
            Assert.That(chat.Id.ToString(), Is.EqualTo(chatId.ToString()));
            Assert.That(chat.MessagesModels.Count, Is.EqualTo(1));
            Assert.That(chat.IsRead, Is.True);
        }

        [Test]
        public async Task GetUserChatAsync_ReturnException()
        {
            var chatId = Guid.NewGuid();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await chatService.GetUserChatAsync(chatId.ToString().ToLower()));
        }

        //IsAnyUserChatAsync
        [Test]
        public async Task IsAnyUserChatAsync_WithValidId_ShouldReturnTrue()
        {
            var userId = Guid.NewGuid();
            var chatId = Guid.NewGuid();
            var userName = "TestUser";
            var message = "TestMessage";

            await dbContext.Chats.AddRangeAsync(new Chat()
            {
                Id = chatId,
                ChatUserId = userId,
                Username = userName,
                IsRead = false,
                ChatMessages = new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        Message = message,
                        CreatedAt = DateTime.UtcNow,
                        SenderName = userName,
                    }
                }
            });

            await dbContext.SaveChangesAsync();

            var result = await chatService.IsAnyUserChatAsync(userId.ToString().ToLower());

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsAnyUserChatAsync_WithInvalidId_ShouldReturnFalse()
        {
            var userId = Guid.NewGuid();

            var result = await chatService.IsAnyUserChatAsync(userId.ToString().ToLower());

            Assert.That(result, Is.False);
        }

        //MarkAsReadAsync
        [Test]
        public async Task MarkAsReadAsync_WithValidId_ShouldMarkAsRead()
        {
            var userId = Guid.NewGuid();
            var chatId = Guid.NewGuid();
            var userName = "TestUser";
            var message = "TestMessage";

            await dbContext.Chats.AddRangeAsync(new Chat()
            {
                Id = chatId,
                ChatUserId = userId,
                Username = userName,
                IsRead = false,
                ChatMessages = new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        Message = message,
                        CreatedAt = DateTime.UtcNow,
                        SenderName = userName,
                    }
                }
            });

            await dbContext.SaveChangesAsync();

            await chatService.MarkAsReadAsync(chatId.ToString().ToLower());

            var chat =  dbContext.Chats.FirstOrDefault();

            Assert.That(chat, Is.Not.Null);
            Assert.That(chat.Id, Is.EqualTo(chatId));
            Assert.That(chat.IsRead, Is.True);
        }

        [Test]
        public async Task MarkAsReadAsync_WithInvalidId_ShouldReturnException()
        {
            var chatId = Guid.NewGuid();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await chatService.MarkAsReadAsync(chatId.ToString().ToLower()));
        }

        //MarkAsUnReadAsync
        [Test]
        public async Task MarkAsUnReadAsync_WithValidId_ShouldMarkAsUnRead()
        {
            var userId = Guid.NewGuid();
            var chatId = Guid.NewGuid();
            var userName = "TestUser";
            var message = "TestMessage";

            await dbContext.Chats.AddRangeAsync(new Chat()
            {
                Id = chatId,
                ChatUserId = userId,
                Username = userName,
                IsRead = true,
                ChatMessages = new List<ChatMessage>()
                {
                    new ChatMessage()
                    {
                        Message = message,
                        CreatedAt = DateTime.UtcNow,
                        SenderName = userName,
                    }
                }
            });

            await dbContext.SaveChangesAsync();

            await chatService.MarkAsUnReadAsync(userId.ToString().ToLower());

            var chat = dbContext.Chats.FirstOrDefault();

            Assert.That(chat, Is.Not.Null);
            Assert.That(chat.Id, Is.EqualTo(chatId));
            Assert.That(chat.IsRead, Is.False);
        }

        [Test]
        public async Task MarkAsUnReadAsync_WithInvalidId_ShouldReturnException()
        {
            var userId = Guid.NewGuid();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await chatService.MarkAsUnReadAsync(userId.ToString().ToLower()));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
