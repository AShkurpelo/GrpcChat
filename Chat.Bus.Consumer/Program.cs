using Chat.Bus.Events;
using Chat.Data;
using Chat.Data.Models;

using EasyNetQ;

namespace Chat.Bus.Consumer
{
	class Program
	{
		static void Main(string[] args)
		{
			using var bus = RabbitHutch.CreateBus("host=localhost");
			bus.PubSub.Subscribe<UserConnected>("consumer", HandleUserConnected);
			bus.PubSub.Subscribe<UserDisconnected>("consumer", HandleUserDisconnected);
			bus.PubSub.Subscribe<MessageBroadcasted>("consumer", HandleMessageBroadcasted);

			System.Console.WriteLine("Press any key to exit.");
			System.Console.ReadKey();
		}

		static void HandleUserConnected(UserConnected @event)
		{
			var log = new ChatLog
			{
				ConnectionId = @event.ConnectionId,
				Date = @event.Date,
				UserName = @event.UserName,
				LogActionType = ChatLogActionType.UserConnected
			};
			SaveLog(log);
		}

		static void HandleUserDisconnected(UserDisconnected @event)
		{
			var log = new ChatLog
			{
				ConnectionId = @event.ConnectionId,
				Date = @event.Date,
				UserName = @event.UserName,
				LogActionType = ChatLogActionType.UserDisconnected
			};
			SaveLog(log);
		}

		static void HandleMessageBroadcasted(MessageBroadcasted @event)
		{
			var log = new ChatLog
			{
				ConnectionId = @event.ConnectionId,
				Date = @event.Date,
				UserName = @event.UserName,
				LogActionType = ChatLogActionType.SendMessage,
				Content = @event.Content
			};
			SaveLog(log);
		}

		static void SaveLog(ChatLog log)
		{
			System.Console.WriteLine(log);
			using var context = new ChatContext();
			context.Add(log);
			context.SaveChanges();
		}
	}
}
