﻿using Microsoft.AspNetCore.SignalR;
using RestaurantApp.Core.Contracts;
using System.Globalization;

namespace RestaurantApp.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IChatService chatService;

		public ChatHub(IChatService chatService)
		{
			this.chatService = chatService;
		}

		public async Task SendMessageToRestaurantService(string message)
		{
			var userId = Context.UserIdentifier;
			var userName = Context.User!.Identity!.Name;

			var adminId = "47ADC156-068F-4BC1-9D2F-F63C3586DA7F".ToLower();
			var date = DateTime.UtcNow.ToString("g", CultureInfo.InvariantCulture);

			await Clients.User(adminId).SendAsync("ReceiveMessage", userName, message, date);
			await Clients.Caller.SendAsync("ReceiveMessage", "You", message);

			await chatService.AddMessageAsync(userId, userName, message);
		}

		public async Task SendMessageToUser(string userId, string message)
		{
			await Clients.User(userId.ToLower()).SendAsync("ReceiveMessage", "RestaurantService", message);
			await Clients.Caller.SendAsync("ReceiveMessage", "You", message);

			await chatService.AddMessageAsync(userId, "RestaurantService", message);
		}

		public override async Task OnConnectedAsync()
		{
			if (!Context.User.IsInRole("Administrator") && await chatService.IsAnyChatAsync(Context.UserIdentifier))
			{

				await chatService.MarkAsUnReadAsync(Context.UserIdentifier);
			}

			await base.OnConnectedAsync();
		}
	}
}
