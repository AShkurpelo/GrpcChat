using System;

namespace Chat.Bus.Events
{
	public class MessageBroadcasted
	{
		public DateTime Date { get; set; }
		public string UserName { get; set; }
		public string ConnectionId { get; set; }
		public string Content { get; set; }
	}
}
