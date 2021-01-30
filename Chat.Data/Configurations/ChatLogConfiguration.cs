using Chat.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Data.Configurations
{
	public class CurrencyConfiguration : IEntityTypeConfiguration<ChatLog>
    {
        public void Configure(EntityTypeBuilder<ChatLog> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.UserName).IsRequired(false);
            builder.Property(p => p.Content).IsRequired(false);

            builder.HasKey(x => x.Id);
        }
    }
}
