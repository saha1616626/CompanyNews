using CompanyNews.Models.Extended;
using CompanyNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.NewsCategories
{
	/// <summary>
	/// Интерфейс для репозитория категорий новостей, обеспечивающий 
	/// основные CRUD и другие операции
	/// </summary>
	public interface INewsCategoryRepository
    {
		/// <summary>
		/// Получение категории поста
		/// </summary>
		/// <param name="id">id категории</param>
		Task<Models.NewsCategory> GetNewsCategoryByIdAsync(int id);

		/// <summary>
		/// Получение списка категорий постов
		/// </summary>
		Task<IEnumerable<Models.NewsCategory>> GetAllNewsCategoriesAsync();

		/// <summary>
		/// Получение списка всех постов новостей с группировкой по категории.
		/// </summary>
		Task<CategoryPostsExtended?> GetListNewsPostGroupedByCategory(Models.NewsCategory newsCategory);

		/// <summary>
		/// Добавить категорию поста
		/// </summary>
		/// <param name="newsCategory">Данные новой категории</param>
		Task<Models.NewsCategory> AddNewsCategoryAsync(Models.NewsCategory newsCategory);

		/// <summary>
		/// Изменить категорию поста
		/// </summary>
		/// <param name="newsCategory">Измененные данные</param>
		Task UpdateNewsCategoryAsync(Models.NewsCategory newsCategory);

		/// <summary>
		/// Удалить категорию поста
		/// </summary>
		/// <param name="id">id категории поста</param>
		Task DeleteNewsCategoryAsync(int id);

		/// <summary>
		/// Добавляет или убирает из архива (инверсия действия)
		/// </summary>
		Task WorkingWithArchive(Models.NewsCategory newsCategory);
	}
}
