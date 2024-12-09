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
		public async Task<Models.NewsCategory?> GetNewsCategoryByIdAsync(int id)
		{
			Models.NewsCategory? newsCategory = await _context.NewsCategories.FindAsync(id);
			if (newsCategory == null) { return null; }

			return newsCategory;
		}

		/// <summary>
		/// Получение списка всех категорий постов
		/// </summary>
		public async Task<IEnumerable<Models.NewsCategory>?> GetAllNewsCategoriesAsync()
		{
			IEnumerable<Models.NewsCategory> newsCategories = await _context.NewsCategories.ToListAsync();
			if(newsCategories == null) { return null; }

			return newsCategories;
		}

		#endregion

		#region WorkingData

		/// <summary>
		/// Добавление новой категории поста
		/// </summary>
		public async Task AddNewsCategoryAsync(Models.NewsCategory newsCategory)
		{
			_context.NewsCategories.Add(newsCategory);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Обновление существующей категории поста
		/// </summary>
		public async Task UpdateNewsCategoryAsync(Models.NewsCategory newsCategory)
		{
			if (newsCategory == null) throw new ArgumentNullException(nameof(newsCategory));

			// Убедимся, что категория существует
			var existingNewsCategory = await _context.NewsCategories.FindAsync(newsCategory.id);
			if (existingNewsCategory == null) throw new KeyNotFoundException($"Категория с ID {existingNewsCategory.id} не найдена.");

			// Обновление данных. Данным методом можно обновить только указанные поля в newsCategory
			_context.Entry(existingNewsCategory).CurrentValues.SetValues(newsCategory);
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
