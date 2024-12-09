using CompanyNews.Data;
using CompanyNews.Models.Extended;
using CompanyNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CompanyNews.Repositories.MessageUsers
{
	/// <summary>
	/// Реализация интерфейса репозитория сообщения пользователя, 
	/// предоставляющий основные CRUD-операции и другие функции для работы с сообщениями.
	/// </summary>
	public class MessageUserRepository : IMessageUserRepository
    {
		private readonly CompanyNewsDbContext _context;

		public MessageUserRepository(CompanyNewsDbContext context)
		{
			_context = context;
		}

		#region Convert

		/// <summary>
		/// Замена идентификатора на соответствующее значение из БД 
		/// </summary>
		public async Task<MessageUserExtendet?> MessageUserConvert(MessageUser? messageUser)
		{
			// Проверяем, не равен ли messageUser null
			if (messageUser == null) { return null; }

			MessageUserExtendet messageUserExtendet = new MessageUserExtendet();
			messageUserExtendet.id = messageUser.id;	
			messageUserExtendet.datePublication = messageUser.datePublication;
			messageUserExtendet.newsPostId = messageUser.newsPostId;
			messageUserExtendet.accountId = messageUser.accountId;
			// Получение аккаунта по id
			Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.id == messageUser.id);
			if (account == null) { return null; }
			messageUserExtendet.Account = account;
			messageUserExtendet.message = messageUser.message;
			messageUserExtendet.status = messageUser.status;
			if(messageUser.rejectionReason != null) { messageUserExtendet.rejectionReason = messageUser.rejectionReason; }

			return messageUserExtendet;
		}

		/// <summary>
		/// Замена значения на соответствующий идентификатор из БД 
		/// </summary>
		public async Task<MessageUser?> MessageUserExtendetConvert
			(MessageUserExtendet? messageUserExtendet)
		{
			// Проверяем, не равен ли messageUserExtendet null
			if (messageUserExtendet == null) { return null; }

			MessageUser messageUser = new MessageUser();
			messageUser.id = messageUserExtendet.id;
			messageUser.datePublication = messageUserExtendet.datePublication;
			messageUser.newsPostId = messageUserExtendet.newsPostId;
			messageUser.accountId = messageUserExtendet.accountId;
			messageUser.message = messageUserExtendet.message;
			messageUser.status = messageUserExtendet.status;
			if (messageUserExtendet.rejectionReason != null) { messageUser.rejectionReason = messageUserExtendet.rejectionReason; }
			
			return messageUser;
		}

		#endregion

		#region GettingData

		/// <summary>
		/// Получение сообщения по идентификатору.
		/// </summary>
		public async Task<MessageUserExtendet?> GetMessageUserByIdAsync(int id)
		{
			return await MessageUserConvert(await _context.MessageUsers.FindAsync(id)) ??
				throw new KeyNotFoundException($"Сообщение с ID {id} не найден.");
		}

		/// <summary>
		/// Получение списка всех сообщений.
		/// </summary>
		public async Task<IEnumerable<MessageUserExtendet>?> GetAllMessageUserAsync()
		{
			IEnumerable<MessageUser> messageUsers = await _context.MessageUsers.ToListAsync();
			if (messageUsers == null) { return null; }

			// Список сообщений
			List<MessageUserExtendet> messageUsersExtendeds = new List<MessageUserExtendet>();

			foreach (var item in messageUsers)
			{
				// Преобразование идентификатора на соответствующие значение из БД
				if (await MessageUserConvert(item) == null) { continue; }
				MessageUserExtendet? messageUserExtendet = await MessageUserConvert(item);
				messageUsersExtendeds.Add(messageUserExtendet);
			}

			return messageUsersExtendeds;
		}

		#endregion

		#region CRUD Operations

		/// <summary>
		/// Добавление нового сообщения к посту.
		/// </summary>
		/// <param name="messageUser">Данные нового сообщения.</param>
		public async Task AddMessageUserAsync(MessageUser messageUser)
		{
			if (messageUser == null) throw new ArgumentNullException(nameof(messageUser));

			await _context.MessageUsers.AddAsync(messageUser);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Обновление существующего комментария.
		/// </summary>
		/// <param name="messageUser">Обновленный текст комментария.</param>
		public async Task UpdateMessageUserAsync(MessageUser messageUser)
		{
			if (messageUser == null) throw new ArgumentNullException(nameof(messageUser));

			// Убедимся, что комментарий существует
			var existingMessageUser = await _context.MessageUsers.FindAsync(messageUser);
			if (existingMessageUser == null) throw new KeyNotFoundException($"Комментарий с ID {messageUser.id} не найден.");

			// Обновление данных. Данным методом можно обновить только указанные поля в messageUser
			_context.Entry(existingMessageUser).CurrentValues.SetValues(messageUser);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Удаление комментария по идентификатору.
		/// </summary>
		/// <param name="id">Идентификатор комментария.</param>
		public async Task DeleteMessageUserAsync(int id)
		{
			var messageUser = await _context.MessageUsers.FindAsync(id);
			if (messageUser == null) throw new KeyNotFoundException($"Комментарий с ID {id} не найден.");

			_context.MessageUsers.Remove(messageUser);
			await _context.SaveChangesAsync();
		}

		#endregion
	}
}
