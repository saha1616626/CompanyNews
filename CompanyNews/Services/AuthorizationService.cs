using CompanyNews.Models;
using CompanyNews.Models.Authorization;
using CompanyNews.Repositories.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Services
{
	/// <summary>
	/// Сервис обеспечивает бизнес-логику взаимодействия с репозиторием "Авторизация"
	/// </summary>
	public class AuthorizationService
	{
		private readonly IAuthorizationRepository _authorizationRepository;
		public AuthorizationService(IAuthorizationRepository authorizationRepository)
		{
			_authorizationRepository = authorizationRepository;
		}

		/// <summary>
		/// Выход из аккаунта
		/// </summary>
		public async Task LogOutYourAccount()
		{
			await _authorizationRepository.LogOutYourAccount();
		}

		/// <summary>
		/// Вход в аккаунт
		/// </summary>
		public async Task<bool> LogInYourAccount(string login, string password)
		{
			return await _authorizationRepository.LogInYourAccount(login, password);
		}

		/// <summary>
		/// Изменение пароля
		/// </summary>
		public async Task<bool> ChangingPassword(Account account, string newPassword)
		{
			return await _authorizationRepository.ChangingPassword(account, newPassword);
		}


		/// <summary>
		/// Получить состояние пользователя в системе
		/// </summary>
		public async Task<UserLoginStatus> GetUserStatusInSystem()
		{
			return _authorizationRepository.GetUserStatusInSystem();
		}

		/// <summary>
		/// Получить аккаунт авторизованного пользователя
		/// </summary>
		/// <remarks>Для работы с правами в системе.</remarks>
		public async Task<Account> GetUserAccount()
		{
			return _authorizationRepository.GetUserAccount();
		}

		/// <summary>
		/// Кол-во пользователей сервиса
		/// </summary>
		public async Task<int> NumberUsers()
		{
			return await _authorizationRepository.NumberUsers();
		}
	}
}
