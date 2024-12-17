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
using CompanyNews.Helpers;
using System.Windows.Media;

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
		public NewsPostExtended? NewsPostConvert(NewsPost? newsPost)
		{
			// Проверяем, не равен ли newsPosts null
			if (newsPost == null) { return null; }

			NewsPostExtended newsPostExtended = new NewsPostExtended();
			newsPostExtended.id = newsPost.id;
			newsPostExtended.newsCategoryId = newsPost.newsCategoryId;
			// Получение названия категории по id
			Models.NewsCategory? newsCategory = _context.NewsCategories.FirstOrDefault(nc => nc.id == newsPost.newsCategoryId);
			if (newsCategory == null) { return null; }
			newsPostExtended.newsCategoryName = newsCategory.name;
			newsPostExtended.datePublication = newsPost.datePublication;
			if(newsPost.image != null) { newsPostExtended.image = WorkingWithImage.ResizingPhotos(newsPost.image, 500); }
			if (newsPost.message != null) { newsPostExtended.message = newsPost.message; }
			newsPostExtended.isArchived = newsPost.isArchived;

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
			if (newsPostExtended.image != null) { newsPost.image = WorkingWithImage.ConvertingImageForWritingDatabase(newsPostExtended.image); }
			if (newsPostExtended.message != null) { newsPost.message = newsPostExtended.message; }
			newsPost.isArchived = newsPostExtended.isArchived;

			return newsPost;
		}


		#endregion

		#region GettingData

		/// <summary>
		/// Получение поста новостей по идентификатору.
		/// </summary>
		public async Task<NewsPostExtended?> GetNewsPostByIdAsync(int id)
		{
			return NewsPostConvert(await _context.NewsPosts.FindAsync(id)) ?? 
				throw new KeyNotFoundException($"Пост с ID {id} не найден.");
		}

		/// <summary>
		/// Получение списка всех постов новостей.
		/// </summary>
		public IEnumerable<NewsPostExtended>? GetAllNewsPostsAsync()
		{
			IEnumerable<NewsPost> newsPosts = _context.NewsPosts.ToList();
			if (newsPosts == null) { return null; }

			// Список постов
			List<NewsPostExtended> newsPostExtendeds = new List<NewsPostExtended>();

			foreach (var item in newsPosts)
			{
				// Преобразование идентификатора на соответствующие значение из БД
				if(NewsPostConvert(item) == null) { continue; }
				NewsPostExtended? newsPostExtended = NewsPostConvert(item);
				newsPostExtendeds.Add(newsPostExtended);
			}

			return newsPostExtendeds;
		}

		/// <summary>
		/// Получение списка всех постов и сообщений к ним.
		/// </summary>
		public List<MessagesNewsPostExtended>? GettingPostsWithMessages()
		{
			List<NewsPost> newsPosts = _context.NewsPosts.ToList();
			List<MessageUser> messageUsers = _context.MessageUsers.ToList();
			List<Account> accounts = _context.Accounts.ToList();
			if (newsPosts == null) { return null; }

			// Список постов с сообщениями
			List<MessagesNewsPostExtended> messagesNewsPostExtendeds = new List<MessagesNewsPostExtended>();

			foreach(var item in newsPosts)
			{
				NewsPostExtended? newsPostExtended = NewsPostConvert(item);
				if(newsPostExtended != null)
				{
					MessagesNewsPostExtended messagesNewsPostExtended = new MessagesNewsPostExtended();
					messagesNewsPostExtended.NewsPostExtended = newsPostExtended;

					// Поиск сообщений пользователей по данному посту
					List<MessageUser> messages = messageUsers.Where(m => m.newsPostId == newsPostExtended.id).ToList();
					if(messages != null)
					{
						List<MessageUserExtended> messageUserExtendeds = new List<MessageUserExtended>();
						// Если сообщения найдены
						foreach(var message in messages)
						{
							// Ищем пользователя, который оставил комментарий
							Account account = accounts.FirstOrDefault(a => a.id == message.accountId);
							if(account != null)
							{
								MessageUserExtended messageUserExtended = new MessageUserExtended();
								messageUserExtended.Account = account;
								messageUserExtended.id = message.id;
								messageUserExtended.datePublication = message.datePublication;
								messageUserExtended.newsPostId = message.newsPostId;
								messageUserExtended.accountId = account.id;
								messageUserExtended.message = message.message;
								messageUserExtended.status = message.status;
								messageUserExtended.dateModeration = message.dateModeration;
								messageUserExtended.rejectionReason = message.rejectionReason;

								// Устанавливаем свойства для кнопок

								// Бокировка сообщений
								if(account.isCanLeaveComments) // Сообщения заблокированы
								{ 
									messageUserExtended.IsBlockAccount = false; // Кнопка заблокировать скрыта
									messageUserExtended.IsUnlockAccount = true; // Кнопка разблокиовать видна
								}
								else // Сообщения не заблокированы
								{ 
									messageUserExtended.IsBlockAccount = true; // Кнопка заблокировать видна
									messageUserExtended.IsUnlockAccount = true; // Кнопка разблокиовать скрыта
								}

								// Статус проверки сообщения
								if(message.status == "На проверке")
								{
									messageUserExtended.IsApproveMessage = true; // Кнопка одобрить сообщение видна
									messageUserExtended.IsRejectMessage = true; // Кнопка отклонить сообщение видна
									messageUserExtended.IsRestoreMessage = false; // Кнопка восстановить сообщение после отклонения скрыта
								}

								if (message.status == "Одобрено")
								{
									messageUserExtended.IsApproveMessage = false; // Кнопка одобрить сообщение скрыта
									messageUserExtended.IsRejectMessage = true; // Кнопка отклонить сообщение видна
									messageUserExtended.IsRestoreMessage = true; // Кнопка восстановить сообщение после отклонения видна
								}

								if (message.status == "Отклонено")
								{
									messageUserExtended.IsApproveMessage = true; // Кнопка одобрить сообщение видна
									messageUserExtended.IsRejectMessage = false; // Кнопка отклонить сообщение скрыта
									messageUserExtended.IsRestoreMessage = true; // Кнопка восстановить сообщение после отклонения видна
								}

								messageUserExtended.IsDeleteMessage = true; // Кнопка удалить сообщение видна

								messageUserExtendeds.Add(messageUserExtended); // Добавили в список сообщений текущего поста сообщение
							}
							else
							{
								continue;
							}
						}
						messagesNewsPostExtended.MessageUserExtendeds = messageUserExtendeds;
						messagesNewsPostExtendeds.Add(messagesNewsPostExtended);
						continue;
					}
					else
					{
						// Возвращаем пост без сообщений
						messagesNewsPostExtended.MessageUserExtendeds = null;
						messagesNewsPostExtendeds.Add(messagesNewsPostExtended);
						continue;
					}
				}
				else
				{
					continue;
				}
			}

			return messagesNewsPostExtendeds;
		}

		#endregion

		#region CRUD Operations

		/// <summary>
		/// Добавление нового поста.
		/// </summary>
		public NewsPost AddNewsPostAsync(NewsPost newsPost)
		{
			if (newsPost == null) throw new ArgumentNullException(nameof(newsPost));

			 _context.NewsPosts.Add(newsPost);
			 _context.SaveChanges();
			return newsPost; // Возвращаем объект с обновленными данными, включая Id
		}

		/// <summary>
		/// Обновление существующего поста новости.
		/// </summary>
		public void UpdateNewsPostAsync(NewsPost newsPost)
		{
			if (newsPost == null) throw new ArgumentNullException(nameof(newsPost));

			// Убедимся, что пост существует
			var existingPost = _context.NewsPosts.FirstOrDefault(p => p.id == newsPost.id);
			if (existingPost == null) throw new KeyNotFoundException($"Пост с ID {newsPost.id} не найден.");

			// Обновление данных. Данным методом можно обновить только указанные поля в newsPost
			_context.Entry(existingPost).CurrentValues.SetValues(newsPost);
		    _context.SaveChanges();
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
