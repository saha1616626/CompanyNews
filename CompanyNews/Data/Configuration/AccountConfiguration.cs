using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CompanyNews.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyNews.Data.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
		public void Configure(EntityTypeBuilder<Account> builder)
		{
			builder.HasKey(account => account.id);
			builder.ToTable("Account");
			builder.Property(account => account.login).HasColumnName("login").HasMaxLength(100);
			builder.Property(account => account.password).HasColumnName("password").HasMaxLength(100);
			builder.Property(account => account.accountRole).HasColumnName("accountRole").HasMaxLength(50);
			builder.Property(account => account.workDepartmentId).HasColumnName("workDepartmentId").IsRequired(false);
			builder.Property(account => account.phoneNumber).HasColumnName("phoneNumber").HasMaxLength(11);
			builder.Property(account => account.name).HasColumnName("name").HasMaxLength(100);
			builder.Property(account => account.surname).HasColumnName("surname").HasMaxLength(100);
			builder.Property(account => account.patronymic).HasColumnName("patronymic").HasMaxLength(100).IsRequired(false);
			builder.Property(account => account.image).HasColumnName("image").IsRequired(false);
			builder.Property(account => account.isProfileBlocked).HasColumnName("isProfileBlocked");
			builder.Property(account => account.reasonBlockingAccount).HasColumnName("reasonBlockingAccount").HasMaxLength(1000).IsRequired(false);
			builder.Property(account => account.isCanLeaveComments).HasColumnName("isCanLeaveComments");
			builder.Property(account => account.reasonBlockingMessages).HasColumnName("reasonBlockingMessages").HasMaxLength(1000).IsRequired(false);

			builder.HasMany(account => account.availableCategoriesUsers)
				.WithOne(availableCategoriesUser => availableCategoriesUser.account)
				.HasForeignKey(availableCategoriesUser => availableCategoriesUser.accountId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(account => account.messageUsers)
				.WithOne(messageUser => messageUser.account)
				.HasForeignKey(messageUser => messageUser.accountId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
