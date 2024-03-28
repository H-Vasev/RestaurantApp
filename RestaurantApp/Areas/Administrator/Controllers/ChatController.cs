using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Core.Contracts;

namespace RestaurantApp.Areas.Administrator.Controllers
{
	public class ChatController : BaseAdministratorController
	{
		private readonly IChatService chatService;

		public ChatController(IChatService chatService)
		{
			this.chatService = chatService;
		}

		public async Task<IActionResult> Index()
		{
			var model = await chatService.GetAllUnreadChatsAsync();
			return View(model);
		}

		public async Task<IActionResult> LiveChat(string id)
		{
			try
			{
				var model = await chatService.GetUserChatAsync(id);
				return View(model);
			}
			catch (Exception)
			{
				return BadRequest();
			}

		}
	}
}
