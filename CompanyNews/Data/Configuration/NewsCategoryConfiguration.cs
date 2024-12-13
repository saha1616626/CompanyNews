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
    public class NewsCategoryConfiguration : IEntityTypeConfiguration<NewsCategory>
    {
		public void Configure(EntityTypeBuilder<NewsCategory> builder)
		{
			builder.HasKey(newsCategory => newsCategory.id);
			builder.ToTable("NewsCategory");
			builder.Property(newsCategory => newsCategory.name).HasColumnName("name").HasMaxLength(100);
			builder.Property(newsCategory => newsCategory.description).HasColumnName("description").HasMaxLength(1000).IsRequired(false);
			builder.Property(newsCategory => newsCategory.isArchived).HasColumnName("isArchived");

			builder.HasMany(newsCategory => newsCategory.newsCategoriesWorkDepartments)
				.WithOne(availableCategoriesUser => availableCategoriesUser.newsCategory)
				.HasForeignKey(availableCategoriesUser => availableCategoriesUser.newsCategoryId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(newsCategory => newsCategory.newsPosts)
				.WithOne(newsPost => newsPost.newsCategory)
				.HasForeignKey(newsPost => newsPost.newsCategoryId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
