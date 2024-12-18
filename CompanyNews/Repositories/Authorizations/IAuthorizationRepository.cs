using CompanyNews.Models;
using CompanyNews.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.Authorizations
{
	/// <summary>
	/// Интерфейс репозитория — управляет доступом базы данных для работы с авторизацией и аутентификацией пользователей.
	/// </summary>
	public interface IAuthorizationRepository
	{
		/// <summary>
		/// Выход из аккаунта
		/// </summary>
		Task LogOutYourAccount();

		/// <summary>
		/// Вход в аккаунт
		/// </summary>
		Task<bool> LogInYourAccount(string login, string password);

		/// <summary>
		/// Изменение пароля
		/// </summary>
		Task<bool> ChangingPassword(Account account, string newPassword);

		/// <summary>
		/// Получить состояние пользователя в системе
		/// </summary>
		UserLoginStatus GetUserStatusInSystem();

		/// <summary>
		/// Получить аккаунт авторизованного пользователя
		/// </summary>
		/// <remarks>Для работы с правами в системе.</remarks>
		Account GetUserAccount();

		/// <summary>
		/// Кол-во пользователей сервиса
		/// </summary>
		Task<int> NumberUsers();
	}
}
