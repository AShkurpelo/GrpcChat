﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Server
{
	public class BroadcastService
	{
		private readonly ConcurrentDictionary<string, ChatUser> subscribers = new ConcurrentDictionary<string, ChatUser>();

		public async Task AddUserAsync(ChatUser user)
		{
			var success = this.subscribers.TryAdd(user.ConnectionId, user);
			if (success) 
			{
				var message = new Message { Message_ = $"{user.Name} joined chat" };
				await this.SendMessageAsync(message);
			}
		}

		public async Task RemoveUserAsync(ChatUser user)
		{
			var success = this.subscribers.TryRemove(user.ConnectionId, out _);
			if (success)
			{
				var message = new Message { Message_ = $"{user.Name} left chat" };
				await this.SendMessageAsync(message);
			}
		}

		public async Task SendMessageAsync(Message message)
		{
			if (string.IsNullOrEmpty(message.Message_))
			{
				return;
			}

			foreach (var user in this.subscribers.Values)
			{
				try
				{
					await user.Stream.WriteAsync(message);
				}
				catch (Exception ex)
				{
					await this.RemoveUserAsync(user);
				}
			}
		}
	}
}
