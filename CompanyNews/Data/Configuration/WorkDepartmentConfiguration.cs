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
    public class WorkDepartmentConfiguration : IEntityTypeConfiguration<WorkDepartment>
    {
		public void Configure(EntityTypeBuilder<WorkDepartment> builder)
		{
			builder.HasKey(workDepartment => workDepartment.id);
			builder.ToTable("WorkDepartment");
			builder.Property(workDepartment => workDepartment.name).HasColumnName("name").HasMaxLength(100);
			builder.Property(workDepartment => workDepartment.description).HasColumnName("description").HasMaxLength(1000).IsRequired(false);

			// Настройка связи один ко многим между WorkDepartment и Account
			builder.HasMany(workDepartment => workDepartment.accounts) // Одна должность может быть
																	   // у нескольких аккаунтов
				.WithOne(account => account.workDepartment) // Каждый аккаунт имеет одну должность
				.HasForeignKey(account => account.workDepartmentId) // Внешний ключ для связи
				.OnDelete(DeleteBehavior.SetNull); // Удаление должности заменит у аккаунта id на Null

			builder.HasMany(workDepartment => workDepartment.newsCategoriesWorkDepartments)
				.WithOne(NewsCategoriesWorkDepartment => NewsCategoriesWorkDepartment.workDepartment)
				.HasForeignKey(NewsCategoriesWorkDepartment => NewsCategoriesWorkDepartment.workDepartmentId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
