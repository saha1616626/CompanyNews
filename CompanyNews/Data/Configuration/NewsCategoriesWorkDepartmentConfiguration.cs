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
    public class NewsCategoriesWorkDepartmentConfiguration : IEntityTypeConfiguration<NewsCategoriesWorkDepartment>
	{
		public void Configure(EntityTypeBuilder<NewsCategoriesWorkDepartment> builder)
		{
			builder.HasKey(newsCategoriesWorkDepartment => newsCategoriesWorkDepartment.id);
			builder.ToTable("NewsCategoriesWorkDepartment");
			builder.Property(newsCategoriesWorkDepartment => newsCategoriesWorkDepartment.workDepartmentId).HasColumnName("workDepartmentId");
			builder.Property(newsCategoriesWorkDepartment => newsCategoriesWorkDepartment.newsCategoryId).HasColumnName("newsCategoryId");
		}
	}
}
