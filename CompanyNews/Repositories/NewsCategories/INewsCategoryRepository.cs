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
		Task<NewsCategory> GetNewsCategoryByIdAsync(int id);

		/// <summary>
		/// Получение списка категорий постов
		/// </summary>
		Task<IEnumerable<NewsCategory>> GetAllNewsCategoriesAsync();

		/// <summary>
		/// Добавить категорию поста
		/// </summary>
		/// <param name="newsCategory">Данные новой категории</param>
		Task AddNewsCategoryAsync(NewsCategory newsCategory);

		/// <summary>
		/// Изменить категорию поста
		/// </summary>
		/// <param name="newsCategory">Измененные данные</param>
		Task UpdateNewsCategoryAsync(NewsCategory newsCategory);

		/// <summary>
		/// Удалить категорию поста
		/// </summary>
		/// <param name="id">id категории поста</param>
		Task DeleteNewsCategoryAsync(int id);
	}
}
