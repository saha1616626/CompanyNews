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
		MessageUserExtended? MessageUserConvert(MessageUser? messageUser);

		/// <summary>
		/// Получение MessageUser из MessageUserExtendet. 
		/// Замена значения на соответствующий идентификатор из БД 
		/// </summary>
		MessageUser? MessageUserExtendetConvert(MessageUserExtended? messageUserExtendet);

		/// <summary>
		/// Получение сообщения по идентификатору.
		/// </summary>
		/// <param name="id">Идентификатор сообщения.</param>
		MessageUserExtended? GetMessageUserByIdAsync(int id);

		/// <summary>
		/// Получение списка всех сообщений.
		/// </summary>
		IEnumerable<MessageUserExtended>? GetAllMessageUserAsync();

		/// <summary>
		/// Добавление нового сообщения к посту.
		/// </summary>
		/// <param name="messageUser">Данные нового сообщения.</param>
		MessageUser AddMessageUserAsync(MessageUser messageUser);

		/// <summary>
		/// Обновление существующего комментария.
		/// </summary>
		/// <param name="messageUser">Обновленный текст комментария.</param>
		void UpdateMessageUserAsync(MessageUser messageUser);

		/// <summary>
		/// Удаление комментария по идентификатору.
		/// </summary>
		/// <param name="id">Идентификатор комментария.</param>
		void DeleteMessageUserAsync(int id);
	}
}
