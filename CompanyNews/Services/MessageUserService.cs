using CompanyNews.Models.Extended;
using CompanyNews.Models;
using CompanyNews.Repositories.MessageUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Services
{
	/// <summary>
	/// Сервис обеспечивает бизнес-логику взаимодействия с репозиторием "Сообщение пользователя"
	/// </summary>
	public class MessageUserService
	{
		private readonly IMessageUserRepository _messageUserRepository;
		public MessageUserService(IMessageUserRepository messageUserRepository)
		{
			_messageUserRepository = messageUserRepository;
		}

		#region Convert

		public async Task<MessageUserExtended?> MessageUserConvert(MessageUser? messageUser)
		{
			return _messageUserRepository.MessageUserConvert(messageUser);
		}

		public async Task<MessageUser?> MessageUserExtendetConvert(MessageUserExtended? messageUserExtendet)
		{
			return _messageUserRepository.MessageUserExtendetConvert(messageUserExtendet);
		}

		#endregion

		#region GettingData

		public async Task<MessageUserExtended?> GetMessageUserByIdAsync(int id)
		{
			return _messageUserRepository.GetMessageUserByIdAsync(id);
		}

		public async Task<IEnumerable<MessageUserExtended>?> GetAllMessageUserAsync()
		{
			return _messageUserRepository.GetAllMessageUserAsync();
		}

		#endregion

		#region CRUD Operations

		public async Task<MessageUser> AddMessageUserAsync(MessageUser messageUser)
		{
			var addedMessageUser = _messageUserRepository.AddMessageUserAsync(messageUser);
			return addedMessageUser; // Возвращаем добавленное сообщение
		}

		public async Task UpdateMessageUserAsync(MessageUser messageUser)
		{
			 _messageUserRepository.UpdateMessageUserAsync(messageUser);
		}

		public async Task DeleteMessageUserAsync(int id)
		{
			 _messageUserRepository.DeleteMessageUserAsync(id);
		}

		#endregion
	}
}
