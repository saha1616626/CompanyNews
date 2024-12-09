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
			return await _messageUserRepository.MessageUserConvert(messageUser);
		}

		public async Task<MessageUser?> MessageUserExtendetConvert(MessageUserExtended? messageUserExtendet)
		{
			return await _messageUserRepository.MessageUserExtendetConvert(messageUserExtendet);
		}

		#endregion

		#region GettingData

		public async Task<MessageUserExtended?> GetMessageUserByIdAsync(int id)
		{
			return await _messageUserRepository.GetMessageUserByIdAsync(id);
		}

		public async Task<IEnumerable<MessageUserExtended>?> GetAllMessageUserAsync()
		{
			return await _messageUserRepository.GetAllMessageUserAsync();
		}

		#endregion

		#region CRUD Operations

		public async Task<MessageUser> AddMessageUserAsync(MessageUser messageUser)
		{
			var addedMessageUser = await _messageUserRepository.AddMessageUserAsync(messageUser);
			return addedMessageUser; // Возвращаем добавленное сообщение
		}

		public async Task UpdateMessageUserAsync(MessageUser messageUser)
		{
			await _messageUserRepository.UpdateMessageUserAsync(messageUser);
		}

		public async Task DeleteMessageUserAsync(int id)
		{
			await _messageUserRepository.DeleteMessageUserAsync(id);
		}

		#endregion
	}
}
