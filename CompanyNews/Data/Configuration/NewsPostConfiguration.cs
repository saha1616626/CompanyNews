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
    public class NewsPostConfiguration : IEntityTypeConfiguration<NewsPost>
	{
		public void Configure(EntityTypeBuilder<NewsPost> builder)
		{
			builder.HasKey(newsPost => newsPost.id);
			builder.ToTable("NewsPost");
			builder.Property(newsPost => newsPost.newsCategoryId).HasColumnName("newsCategoryId");
			builder.Property(newsPost => newsPost.datePublication).HasColumnName("datePublication");
			builder.Property(newsPost => newsPost.image).HasColumnName("image").IsRequired(false);
			builder.Property(newsPost => newsPost.message).HasColumnName("message").IsRequired(false);

			builder.HasMany(newsPost => newsPost.messageUsers)
				.WithOne(messageUser => messageUser.newsPost)
				.HasForeignKey(messageUser => messageUser.newsPostId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
