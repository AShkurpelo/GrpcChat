using System;

namespace Chat.Bus.Events
{
	public class UserDisconnected
	{
		public DateTime Date { get; set; }
		public string UserName { get; set; }
		public string ConnectionId { get; set; }
	}
}
