using Chat.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Chat.Data
{
	public class ChatContext : DbContext
	{
		DbSet<ChatLog> ChatLogs { get; set; }

		public ChatContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=(local);Database=chat;Trusted_Connection=True;");
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfigurationsFromAssembly(typeof(ChatContext).Assembly);
		}
	}
}
