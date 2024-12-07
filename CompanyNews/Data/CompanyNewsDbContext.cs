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
		public CompanyNewsDbContext()
		{
			Database.EnsureCreated(); // Проверка наличия БД
		}

        public DbSet<Account> Accounts { get; set; }
		public DbSet<WorkDepartment> WorkDepartments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new AccountConfiguration());

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
