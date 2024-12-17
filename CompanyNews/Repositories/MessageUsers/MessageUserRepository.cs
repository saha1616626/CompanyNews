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
		public MessageUserExtended? MessageUserConvert(MessageUser? messageUser)
		{
			// Проверяем, не равен ли messageUser null
			if (messageUser == null) { return null; }

			MessageUserExtended messageUserExtendet = new MessageUserExtended();
			messageUserExtendet.id = messageUser.id;	
			messageUserExtendet.datePublication = messageUser.datePublication;
			messageUserExtendet.newsPostId = messageUser.newsPostId;
			messageUserExtendet.accountId = messageUser.accountId;
			// Получение аккаунта по id
			Account? account = _context.Accounts.FirstOrDefault(a => a.id == messageUser.id);
			if (account == null) { return null; }
			messageUserExtendet.Account = account;
			messageUserExtendet.message = messageUser.message;
			messageUserExtendet.status = messageUser.status;
			if (messageUser.dateModeration != null) { messageUserExtendet.dateModeration = messageUser.dateModeration; }
			if (messageUser.rejectionReason != null) { messageUserExtendet.rejectionReason = messageUser.rejectionReason; }

			return messageUserExtendet;
		}

		/// <summary>
		/// Замена значения на соответствующий идентификатор из БД 
		/// </summary>
		public MessageUser? MessageUserExtendetConvert
			(MessageUserExtended? messageUserExtendet)
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
			if (messageUserExtendet.dateModeration != null) { messageUser.dateModeration = messageUserExtendet.dateModeration; }
			if (messageUserExtendet.rejectionReason != null) { messageUser.rejectionReason = messageUserExtendet.rejectionReason; }
			
			return messageUser;
		}

		#endregion

		#region GettingData

		/// <summary>
		/// Получение сообщения по идентификатору.
		/// </summary>
		public MessageUserExtended? GetMessageUserByIdAsync(int id)
		{
			return MessageUserConvert(_context.MessageUsers.FirstOrDefault(m => m.id == id)) ??
				throw new KeyNotFoundException($"Сообщение с ID {id} не найден.");
		}

		/// <summary>
		/// Получение списка всех сообщений.
		/// </summary>
		public IEnumerable<MessageUserExtended>? GetAllMessageUserAsync()
		{
			IEnumerable<MessageUser> messageUsers = _context.MessageUsers.ToList();
			if (messageUsers == null) { return null; }

			// Список сообщений
			List<MessageUserExtended> messageUsersExtendeds = new List<MessageUserExtended>();

			foreach (var item in messageUsers)
			{
				// Преобразование идентификатора на соответствующие значение из БД
				if (MessageUserConvert(item) == null) { continue; }
				MessageUserExtended? messageUserExtendet = MessageUserConvert(item);
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
		public MessageUser AddMessageUserAsync(MessageUser messageUser)
		{
			if (messageUser == null) throw new ArgumentNullException(nameof(messageUser));

			_context.MessageUsers.Add(messageUser);
			_context.SaveChangesAsync();
			return messageUser; // Возвращаем объект с обновленными данными, включая Id
		}

		/// <summary>
		/// Обновление существующего комментария.
		/// </summary>
		/// <param name="messageUser">Обновленный текст комментария.</param>
		public void UpdateMessageUserAsync(MessageUser messageUser)
		{
			if (messageUser == null) throw new ArgumentNullException(nameof(messageUser));

			// Убедимся, что комментарий существует
			var existingMessageUser = _context.MessageUsers.FirstOrDefault(m => m.id == messageUser.id);
			if (existingMessageUser == null) throw new KeyNotFoundException($"Комментарий с ID {messageUser.id} не найден.");

			// Обновление данных. Данным методом можно обновить только указанные поля в messageUser
			_context.Entry(existingMessageUser).CurrentValues.SetValues(messageUser);
			_context.SaveChanges();
		}

		/// <summary>
		/// Удаление комментария по идентификатору.
		/// </summary>
		/// <param name="id">Идентификатор комментария.</param>
		public void DeleteMessageUserAsync(int id)
		{
			var messageUser = _context.MessageUsers.FirstOrDefault(m => m.id == id);
			if (messageUser == null) throw new KeyNotFoundException($"Комментарий с ID {id} не найден.");

			_context.MessageUsers.Remove(messageUser);
			_context.SaveChanges();
		}

		#endregion
	}
}
