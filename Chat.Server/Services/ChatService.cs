using Grpc.Core;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Server
{
	public class ChatService : Messaging.MessagingBase
	{
		private readonly BroadcastService broadcastService;
		public ChatService(BroadcastService broadcastService)
		{
			this.broadcastService = broadcastService;
		}

		public override async Task Join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
		{
			if(!await requestStream.MoveNext())
			{
				return;
			}

			var user = new ChatUser
			{
				ConnectionId = context.GetHttpContext().Connection.Id,
				Name = requestStream.Current.Name,
				Stream = responseStream
			};

			await broadcastService.AddUserAsync(user);

			do
			{
				await broadcastService.SendMessageAsync(requestStream.Current, user);
			} while (await requestStream.MoveNext());

			await broadcastService.RemoveUserAsync(user);
		}
	}
}
