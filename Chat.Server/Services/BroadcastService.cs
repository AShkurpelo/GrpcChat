using System;
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
		}

		public async Task RemoveUserAsync(ChatUser user)
		{
			var success = this.subscribers.TryRemove(user.ConnectionId, out _);
		}

		public async Task SendMessageAsync(Message message)
		{
			foreach (var user in this.subscribers.Values)
			{
				try
				{
					await user.Stream.WriteAsync(message).ConfigureAwait(false);
				}
				catch(Exception ex)
				{
					await this.RemoveUserAsync(user);
				}
			}
		}
	}
}
