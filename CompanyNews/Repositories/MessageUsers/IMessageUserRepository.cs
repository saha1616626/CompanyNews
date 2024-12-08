using CompanyNews.Models.Extended;
using CompanyNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.MessageUsers
{
	/// <summary>
	/// Интерфейс для репозитория сообщения пользователя, 
	/// предоставляющий основные CRUD-операции и другие функции для работы с сообщениями.
	/// </summary>
	public interface IMessageUserRepository
    {
		/// <summary>
		/// Получение MessageUserExtendet из MessageUser. 
		/// Замена идентификатора на соответствующее значение из БД 
		/// </summary>
		Task<MessageUserExtendet?> MessageUserConvert(MessageUser? messageUser);

		/// <summary>
		/// Получение MessageUser из MessageUserExtendet. 
		/// Замена значения на соответствующий идентификатор из БД 
		/// </summary>
		Task<MessageUser?> MessageUserExtendetConvert(MessageUserExtendet? messageUserExtendet);

		/// <summary>
		/// Получение сообщения по идентификатору.
		/// </summary>
		/// <param name="id">Идентификатор сообщения.</param>
		Task<MessageUserExtendet?> GetMessageUserByIdAsync(int id);

		/// <summary>
		/// Получение списка всех сообщений.
		/// </summary>
		Task<IEnumerable<MessageUserExtendet>?> GetAllMessageUserAsync();

		/// <summary>
		/// Добавление нового сообщения к посту.
		/// </summary>
		/// <param name="messageUser">Данные нового сообщения.</param>
		Task AddMessageUserAsync(MessageUser messageUser);

		/// <summary>
		/// Обновление существующего комментария.
		/// </summary>
		/// <param name="messageUser">Обновленный текст комментария.</param>
		Task UpdateMessageUserAsync(MessageUser messageUser);

		/// <summary>
		/// Удаление комментария по идентификатору.
		/// </summary>
		/// <param name="id">Идентификатор комментария.</param>
		Task DeleteMessageUserAsync(int id);
	}
}
