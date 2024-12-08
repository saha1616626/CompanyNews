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
    public class AvailableCategoriesUserConfiguration : IEntityTypeConfiguration<AvailableCategoriesUser>
	{
		public void Configure(EntityTypeBuilder<AvailableCategoriesUser> builder)
		{
			builder.HasKey(availableCategoriesUser => availableCategoriesUser.id);
			builder.ToTable("AvailableCategoriesUser");
			builder.Property(availableCategoriesUser => availableCategoriesUser.accountId).HasColumnName("accountId");
			builder.Property(availableCategoriesUser => availableCategoriesUser.newsCategoryId).HasColumnName("newsCategoryId");
		}
	}
}
