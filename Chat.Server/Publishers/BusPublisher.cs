using Chat.Bus.Events;

using EasyNetQ;

using System;
using System.Threading.Tasks;

namespace Chat.Server
{
	public class BusPublisher
	{
		public async Task PublishUserConnected(ChatUser user)
		{
			using (var bus = RabbitHutch.CreateBus("host=localhost"))
			{
				await bus.PubSub.PublishAsync(new UserConnected
				{
					ConnectionId = user.ConnectionId,
					Date = DateTime.Now,
					UserName = user.Name
				});
			}
		}

		public async Task PublishUserDisconnected(ChatUser user)
		{
			using (var bus = RabbitHutch.CreateBus("host=localhost"))
			{
				await bus.PubSub.PublishAsync(new UserDisconnected
				{
					ConnectionId = user.ConnectionId,
					Date = DateTime.Now,
					UserName = user.Name
				});
			}
		}

		public async Task PublishMessageBroadcasted(Message message, ChatUser sender)
		{
			using (var bus = RabbitHutch.CreateBus("host=localhost"))
			{
				await bus.PubSub.PublishAsync(new MessageBroadcasted
				{
					ConnectionId = sender.ConnectionId,
					Date = DateTime.Now,
					UserName = message.Name,
					Content = message.Message_, 
				});
			}
		}
	}
}
