using Grpc.Core;

namespace Chat.Server
{
	public class ChatUser
	{
		public string ConnectionId { get; set; }
		public string Name { get; set; }
		public IServerStreamWriter<Message> Stream { get; set; }
	}
}
