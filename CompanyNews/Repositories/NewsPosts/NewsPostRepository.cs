using CompanyNews.Data;
using CompanyNews.Models.Extended;
using CompanyNews.Models;
using CompanyNews.Repositories.NewsCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CompanyNews.Repositories.NewsCategory;

namespace CompanyNews.Repositories.NewsPosts
{
	/// <summary>
	/// Реализация интерфейса для репозитория постов новостей, 
	/// обеспечивающий основные CRUD-операции и другие функции для работы с новостями
	/// </summary>
	public class NewsPostRepository : INewsPostRepository
    {
		private readonly CompanyNewsDbContext _context;

		public NewsPostRepository(CompanyNewsDbContext context)
		{
			_context = context;
		}

		#region Convert

		/// <summary>
		/// Замена идентификатора на соответствующее значение из БД 
		/// </summary>
		public async Task<NewsPostExtended?> NewsPostConvert(NewsPost? newsPost)
		{
			// Проверяем, не равен ли newsPosts null
			if (newsPost == null) { return null; }

			NewsPostExtended newsPostExtended = new NewsPostExtended();
			newsPostExtended.id = newsPost.id;
			newsPostExtended.newsCategoryId = newsPost.newsCategoryId;
			// Получение названия категории по id
			Models.NewsCategory? newsCategory = await _context.NewsCategories.FirstOrDefaultAsync(nc => nc.id == newsPost.newsCategoryId);
			if (newsCategory == null) { return null; }
			newsPostExtended.newsCategoryName = newsCategory.name;
			newsPostExtended.datePublication = newsPost.datePublication;
			if(newsPost.image != null) { newsPostExtended.image = newsPost.image; }
			if (newsPost.message != null) { newsPostExtended.message = newsPost.message; }

			return newsPostExtended;
		}

		/// <summary>
		/// Замена значения на соответствующий идентификатор из БД 
		/// </summary>
		public async Task<NewsPost?> NewsPostExtendedConvert
			(NewsPostExtended? newsPostExtended)
		{
			// Проверяем, не равен ли newsPostExtended null
			if (newsPostExtended == null) { return null; }

			NewsPost newsPost = new NewsPost();
			newsPost.id = newsPostExtended.id;
			newsPost.newsCategoryId = newsPostExtended.newsCategoryId;
			newsPost.datePublication = newsPostExtended.datePublication;
			if (newsPostExtended.image != null) { newsPost.image = newsPostExtended.image; }
			if (newsPostExtended.message != null) { newsPost.message = newsPostExtended.message; }

			return newsPost;
		}


		#endregion

		#region GettingData

		/// <summary>
		/// Получение поста новостей по идентификатору.
		/// </summary>
		public async Task<NewsPostExtended?> GetNewsPostByIdAsync(int id)
		{
			return await NewsPostConvert(await _context.NewsPosts.FindAsync(id)) ?? 
				throw new KeyNotFoundException($"Пост с ID {id} не найден.");
		}

		/// <summary>
		/// Получение списка всех постов новостей.
		/// </summary>
		public async Task<IEnumerable<NewsPostExtended>?> GetAllNewsPostsAsync()
		{
			IEnumerable<NewsPost> newsPosts = await _context.NewsPosts.ToListAsync();
			if (newsPosts == null) { return null; }

			// Список постов
			List<NewsPostExtended> newsPostExtendeds = new List<NewsPostExtended>();

			foreach (var item in newsPosts)
			{
				// Преобразование идентификатора на соответствующие значение из БД
				if(await NewsPostConvert(item) == null) { continue; }
				NewsPostExtended? newsPostExtended = await NewsPostConvert(item);
				newsPostExtendeds.Add(newsPostExtended);
			}

			return newsPostExtendeds;
		}


		#endregion

		#region CRUD Operations

		/// <summary>
		/// Добавление нового поста.
		/// </summary>
		public async Task AddNewsPostAsync(NewsPost newsPost)
		{
			if (newsPost == null) throw new ArgumentNullException(nameof(newsPost));

			await _context.NewsPosts.AddAsync(newsPost);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Обновление существующего поста новости.
		/// </summary>
		public async Task UpdateNewsPostAsync(NewsPost newsPost)
		{
			if (newsPost == null) throw new ArgumentNullException(nameof(newsPost));

			// Убедимся, что пост существует
			var existingPost = await _context.NewsPosts.FindAsync(newsPost.id);
			if (existingPost == null) throw new KeyNotFoundException($"Пост с ID {newsPost.id} не найден.");

			// Обновление данных. Данным методом можно обновить только указанные поля в newsPost
			_context.Entry(existingPost).CurrentValues.SetValues(newsPost);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Удаление поста новости по идентификатору.
		/// </summary>
		public async Task DeleteNewsPostAsync(int id)
		{
			var newsPost = await _context.NewsPosts.FindAsync(id);
			if (newsPost == null) throw new KeyNotFoundException($"Пост с ID {id} не найден.");

			_context.NewsPosts.Remove(newsPost);
			await _context.SaveChangesAsync();
		}

		#endregion
	}
}
