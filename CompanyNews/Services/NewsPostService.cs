using CompanyNews.Models.Extended;
using CompanyNews.Models;
using CompanyNews.Repositories.NewsCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyNews.Repositories.NewsCategory;

namespace CompanyNews.Services
{
	/// <summary>
	/// Сервис обеспечивает бизнес-логику взаимодействия с репозиторием "новый пост"
	/// </summary>
	public class NewsPostService
	{
		private readonly INewsPostRepository _newsPostRepository;
		public NewsPostService(INewsPostRepository newsPostRepository)
		{
			_newsPostRepository = newsPostRepository;
		}

		#region Convert

		public async Task<NewsPostExtended?> NewsPostConvert(NewsPost? newsPosts)
		{
			return _newsPostRepository.NewsPostConvert(newsPosts);
		}

		public async Task<NewsPost?> NewsPostExtendedConvert(NewsPostExtended? newsPostExtended)
		{
			return await _newsPostRepository.NewsPostExtendedConvert(newsPostExtended);
		}

		#endregion

		#region GettingData

		public async Task<NewsPostExtended?> GetNewsPostByIdAsync(int id)
		{
			return await _newsPostRepository.GetNewsPostByIdAsync(id);
		}

		public async Task<IEnumerable<NewsPostExtended>?> GetAllNewsPostsAsync()
		{
			return _newsPostRepository.GetAllNewsPostsAsync();
		}

		#endregion

		#region CRUD Operations

		public async Task<NewsPost> AddNewsPostAsync(NewsPost newsPost)
		{
			var addedNewsPost = _newsPostRepository.AddNewsPostAsync(newsPost);
			return addedNewsPost; // Возвращаем добавленный пост
		}

		public async Task UpdateNewsPostAsync(NewsPost newsPost)
		{
			 _newsPostRepository.UpdateNewsPostAsync(newsPost);
		}

		public async Task DeleteNewsPostAsync(int id)
		{
			await _newsPostRepository.DeleteNewsPostAsync(id);
		}

		#endregion
	}
}
