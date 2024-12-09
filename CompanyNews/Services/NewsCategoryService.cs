using CompanyNews.Repositories.NewsCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Services
{
	/// <summary>
	/// Сервис обеспечивает бизнес-логику взаимодействия с репозиторием "новостная категория"
	/// </summary>
	public class NewsCategoryService
	{
		private readonly INewsCategoryRepository _newsCategoryRepository;
		public NewsCategoryService(INewsCategoryRepository newsCategoryRepository)
		{
			_newsCategoryRepository = newsCategoryRepository;
		}

		#region GettingData

		public async Task<Models.NewsCategory> GetNewsCategoryByIdAsync(int id)
		{
			return await _newsCategoryRepository.GetNewsCategoryByIdAsync(id);
		}

		public async Task<IEnumerable<Models.NewsCategory>> GetAllNewsCategoriesAsync()
		{
			return await _newsCategoryRepository.GetAllNewsCategoriesAsync();
		}

		#endregion

		#region CRUD Operations

		public async Task<Models.NewsCategory> AddNewsCategoryAsync(Models.NewsCategory newsCategory)
		{
			var addedNewsCategory = await _newsCategoryRepository.AddNewsCategoryAsync(newsCategory);
			return addedNewsCategory; // Возвращаем добавленную категорию
		}

		public async Task UpdateNewsCategoryAsync(Models.NewsCategory newsCategory)
		{
			await _newsCategoryRepository.UpdateNewsCategoryAsync(newsCategory);
		}

		public async Task DeleteNewsCategoryAsync(int id)
		{
			await _newsCategoryRepository.DeleteNewsCategoryAsync(id);	
		}

		#endregion
	}
}
