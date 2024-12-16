using CompanyNews.Data;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
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
		public IEnumerable<Models.NewsCategory>? GetAllNewsCategoriesAsync()
		{
			IEnumerable<Models.NewsCategory> newsCategories = _context.NewsCategories.ToList();
			if(newsCategories == null) { return null; }

			return newsCategories;
		}

		/// <summary>
		/// Получение списка всех постов новостей с группировкой по категории.
		/// </summary>
		public async Task<CategoryPostsExtended?> GetListNewsPostGroupedByCategory(Models.NewsCategory newsCategory)
		{
			// Проверяем, не равен ли newsCategory null
			if (newsCategory == null) { return null; }

			// Получаем из БД актуальную версию NewsCategory
			Models.NewsCategory? category = await _context.NewsCategories.FirstOrDefaultAsync(category => category.id == newsCategory.id);
			if (category == null) { return null; }

			CategoryPostsExtended categoryPostsExtended = new CategoryPostsExtended();
			categoryPostsExtended.id = category.id;
			categoryPostsExtended.name = category.name;
			categoryPostsExtended.description = category.description;

			// Получаем список постов для данной категории
			categoryPostsExtended.newsPosts = _context.NewsPosts.Where(post => post.newsCategoryId == category.id).ToList();

			// Получение кол-ва пользователей, которые подписанны на данную категорию
			categoryPostsExtended.numberSubscribers = _context.NewsCategoriesWorkDepartments.Count(category => category.newsCategoryId == category.id);
			categoryPostsExtended.isArchived = newsCategory.isArchived;

			return categoryPostsExtended;
		}

		#endregion

		#region WorkingData

		/// <summary>
		/// Добавление новой категории поста
		/// </summary>
		public async Task<Models.NewsCategory> AddNewsCategoryAsync(Models.NewsCategory newsCategory)
		{
			_context.NewsCategories.Add(newsCategory);
			await _context.SaveChangesAsync();
			return newsCategory; // Возвращаем объект с обновленными данными, включая Id
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
			var newsCategory = await _context.NewsCategories.FirstOrDefaultAsync(c => c.id == id);
			if (newsCategory == null) { return; }
			_context.NewsCategories.Remove(newsCategory);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Добавляет или убирает из архива (инверсия действия)
		/// </summary>
		public async Task WorkingWithArchive(Models.NewsCategory newsCategory)
		{
			if (newsCategory == null) throw new ArgumentNullException(nameof(newsCategory));
			
			// Убедимся, что категория существует
			var existingNewsCategory = await _context.NewsCategories.FindAsync(newsCategory.id);
			if (existingNewsCategory == null) throw new KeyNotFoundException($"Категория с ID {existingNewsCategory.id} не найдена.");

			// Обновление данных. Данным методом можно обновить только указанные поля в newsCategory
			newsCategory.isArchived = !newsCategory.isArchived; // инверсия значения
			_context.Entry(existingNewsCategory).CurrentValues.SetValues(newsCategory);
			await _context.SaveChangesAsync();
		}

		#endregion

	}
}
