using CompanyNews.Data.Configuration;
using CompanyNews.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Data
{
    public class CompanyNewsDbContext : DbContext
    {
		public CompanyNewsDbContext(DbContextOptions<CompanyNewsDbContext> options)
		: base(options)
		{
			// Применяем все миграции, если они не были применены.
			//Database.EnsureCreated();

			Database.Migrate();
		}

		public DbSet<Account> Accounts { get; set; }
		public DbSet<WorkDepartment> WorkDepartments { get; set; }
		public DbSet<NewsCategory> NewsCategories { get; set; }
		public DbSet<NewsCategoriesWorkDepartment> NewsCategoriesWorkDepartments { get; set; }
		public DbSet<NewsPost> NewsPosts { get; set; }
		public DbSet<MessageUser> MessageUsers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new AccountConfiguration());
			modelBuilder.ApplyConfiguration(new WorkDepartmentConfiguration());
			modelBuilder.ApplyConfiguration(new NewsCategoryConfiguration());
			modelBuilder.ApplyConfiguration(new NewsCategoriesWorkDepartmentConfiguration());
			modelBuilder.ApplyConfiguration(new NewsPostConfiguration());
			modelBuilder.ApplyConfiguration(new MessageUserConfiguration());

			base.OnModelCreating(modelBuilder);
		}

		/// <summary>
		/// Строка подключения БД
		/// </summary>
		/// <param name="optionsBuilder"></param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=ALEX_BEREZKIN\SQLEXPRESS;Database=CompanyNewsDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True; encrypt=false");
		}
	}
}
