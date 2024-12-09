﻿using CompanyNews.Data;
using CompanyNews.Repositories.Accounts;
using CompanyNews.Repositories.AvailableCategoriesUsers;
using CompanyNews.Repositories.MessageUsers;
using CompanyNews.Repositories.NewsCategories;
using CompanyNews.Repositories.NewsCategory;
using CompanyNews.Repositories.NewsPosts;
using CompanyNews.Repositories.WorkDepartments;
using CompanyNews.ViewModels.AdminApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Services
{
	/// <summary>
	/// Регистратор зависимостей
	/// </summary>
	public static class ServiceLocator
	{
		private static IServiceProvider _serviceProvider;

		public static void ConfigureServices()
		{
			var services = new ServiceCollection();

			// Регистрация контекста базы данных и репозитория
			services.AddDbContext<CompanyNewsDbContext>(options =>
				options.UseSqlServer(@"Server=ALEX_BEREZKIN\SQLEXPRESS;Database=CompanyNewsDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True; encrypt=false"));

			// Scoped создаётся один экземпляр класса на период запроса 
			services.AddScoped<IAccountRepository, AccountRepository>();
			services.AddScoped<AccountService>();
			services.AddScoped<AccountViewModel>();

			services.AddScoped<IWorkDepartmentRepository, WorkDepartmentRepository>();
			services.AddScoped<WorkDepartmentService>();
			services.AddScoped<WorkDepartmentViewModel>();

			services.AddScoped<INewsCategoryRepository, NewsCategoryRepository>();
			services.AddScoped<NewsCategoryService>();
			services.AddScoped<NewsCategoryViewModel>();

			services.AddScoped<IAvailableCategoriesUserRepository, AvailableCategoriesUserRepository>();
			services.AddScoped<AvailableCategoriesUserService>();
			services.AddScoped<AvailableCategoriesUserViewModel>();

			services.AddScoped<INewsPostRepository, NewsPostRepository>();
			services.AddScoped<NewsPostService>();
			services.AddScoped<NewsCategoryViewModel>();

			services.AddScoped<IMessageUserRepository, MessageUserRepository>();
			services.AddScoped<MessageUserService>();
			services.AddScoped<MessageUserViewModel>();
		}

		public static T GetService<T>() => _serviceProvider.GetRequiredService<T>();
	}
}