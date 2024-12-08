using CompanyNews.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Data.Configuration
{
    public class MessageUserConfiguration : IEntityTypeConfiguration<MessageUser>
	{
		public void Configure(EntityTypeBuilder<MessageUser> builder)
		{
			builder.HasKey(messageUser => messageUser.id);
			builder.ToTable("MessageUser");
			builder.Property(messageUser => messageUser.datePublication).HasColumnName("datePublication");
			builder.Property(messageUser => messageUser.newsPostId).HasColumnName("newsPostId");
			builder.Property(messageUser => messageUser.accountId).HasColumnName("accountId");
			builder.Property(messageUser => messageUser.message).HasColumnName("message").HasMaxLength(1000);
			builder.Property(messageUser => messageUser.status).HasColumnName("status").HasMaxLength(50);
			builder.Property(messageUser => messageUser.rejectionReason).HasColumnName("rejectionReason").HasMaxLength(1000).IsRequired(false);
		}
	}
}
