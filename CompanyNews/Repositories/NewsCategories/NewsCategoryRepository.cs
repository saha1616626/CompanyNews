using CompanyNews.Data;
using CompanyNews.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.NewsCategories
{
	/// <summary>
	/// Реализация интерфейса для репозитория категорий новостей, обеспечивающий основные CRUD и другие операции
	/// </summary>
	public class NewsCategoryRepository : INewsCategoryRepository
	{
		private readonly CompanyNewsDbContext _context;
		public NewsCategoryRepository(CompanyNewsDbContext context)
		{
			_context = context;
		}

		#region GettingData

		/// <summary>
		/// Получение категории поста по идентификатору
		/// </summary>
		public async Task<NewsCategory?> GetNewsCategoryByIdAsync(int id)
		{
			NewsCategory? newsCategory = await _context.NewsCategories.FindAsync(id);
			if (newsCategory == null) { return null; }

			return newsCategory;
		}

		/// <summary>
		/// Получение списка всех категорий постов
		/// </summary>
		public async Task<IEnumerable<NewsCategory>?> GetAllNewsCategoriesAsync()
		{
			IEnumerable<NewsCategory> newsCategories = await _context.NewsCategories.ToListAsync();
			if(newsCategories == null) { return null; }

			return newsCategories;
		}

		#endregion

		#region WorkingData

		/// <summary>
		/// Добавление новой категории поста
		/// </summary>
		public async Task AddNewsCategoryAsync(NewsCategory newsCategory)
		{
			_context.NewsCategories.Add(newsCategory);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Обновление существующей категории поста
		/// </summary>
		public async Task UpdateNewsCategoryAsync(NewsCategory newsCategory)
		{
			_context.NewsCategories.Update(newsCategory);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Удаление категории поста по идентификатору
		/// </summary>
		public async Task DeleteNewsCategoryAsync(int id)
		{
			var newsCategory = await _context.NewsCategories.FindAsync(id);
			if (newsCategory == null) { return; }
			_context.NewsCategories.Remove(newsCategory);
			await _context.SaveChangesAsync();
		}

		#endregion

	}
}
