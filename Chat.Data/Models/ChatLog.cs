using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.Data.Models
{
	public class ChatLog
	{
		public Guid Id { get; set; }
		public DateTime Date { get; set; }
		public ChatLogActionType LogActionType { get; set; }
		public string? UserName { get; set; }
		public string ConnectionId { get; set; }
		public string? Content { get; set; }
	}
}
