using CompanyNews.Data;
using CompanyNews.Helpers;
using CompanyNews.Helpers.Event;
using CompanyNews.Models;
using CompanyNews.Models.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace CompanyNews.Repositories.Authorizations
{
	/// <summary>
	///  Реализация интерфейса управления авторизацией и аутентификацией пользователей через CompanyNewsDbContext
	/// </summary>
	public class AuthorizationRepository : IAuthorizationRepository
	{
		/// <summary>
		/// Путь к JSON хранящего данные авторизованного пользователя
		/// </summary>
		private readonly string authorizationStatusPath = @"..\..\..\Data\Authorization\AuthorizationStatus.json";

		private readonly CompanyNewsDbContext _context;

		public AuthorizationRepository(CompanyNewsDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Выход из аккаунта
		/// </summary>
		public async Task LogOutYourAccount()
		{
			// Состояние выхода
			UserLoginStatus userLoginStatus = new UserLoginStatus();
			userLoginStatus.isUserLoggedIn = false;
			userLoginStatus.accountId = null;
			userLoginStatus.accountRole = null;
			userLoginStatus.isProfileBlocked = null;
			userLoginStatus.reasonBlockingAccount = null;
			userLoginStatus.isCanLeaveComments = null;
			userLoginStatus.reasonBlockingMessages = null;

			// Перезапись данных в JSON
			try
			{
				var jsonLogOutYourAccount = JsonConvert.SerializeObject(userLoginStatus);
				File.WriteAllText(authorizationStatusPath, jsonLogOutYourAccount);

				// Вызов событие перехода на страницу авторизации
				AuthorizationEvent.LogOutYourAccount();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка записи в json файла /n" + ex.Message);
			}
		}

		/// <summary>
		/// Вход в аккаунт
		/// </summary>
		public async Task<bool> LogInYourAccount(string login, string password)
		{
			Account account = await _context.Accounts.FirstOrDefaultAsync(account => account.login == login); // Ищем аккаунт по логину
			if (account == null) { return false; } // Логин не найден
			if(PasswordHasher.VerifyPassword(password, account.password))// Сверяем пароли
			{
				// Новое состояние входа
				UserLoginStatus userLoginStatus = new UserLoginStatus();
				userLoginStatus.isUserLoggedIn = true;
				userLoginStatus.accountId = account.id;
				userLoginStatus.accountRole = account.accountRole;
				userLoginStatus.isProfileBlocked = account.isProfileBlocked;
				if(account.reasonBlockingAccount != null) { userLoginStatus.reasonBlockingAccount = account.reasonBlockingAccount; } 
				userLoginStatus.isCanLeaveComments = account.isCanLeaveComments;
				if (account.reasonBlockingMessages != null) { userLoginStatus.reasonBlockingMessages = account.reasonBlockingMessages; }

				try
				{
					var jsonLogInYourAccount = JsonConvert.SerializeObject(userLoginStatus);
					File.WriteAllText(authorizationStatusPath, jsonLogInYourAccount);

					return true; // Авторизация прошла успешно
				}
				catch (Exception ex)
				{
					MessageBox.Show("Ошибка записи в json файла /n" + ex.Message);
					return false; // Ошибка авторизации
				}
			}
			else { return false; } // Пароль не совпадает
		}

		/// <summary>
		/// Изменение пароля
		/// </summary>
		public async Task<bool> ChangingPassword(Account account, string newPassword)
		{
			if(account == null) { return false; } // Отказ в смене пароля из-за отсутствия аргумента
			
			// Убедимся, что учетная запись существует
			var existingAccount = await _context.Accounts.FindAsync(account.id);
			if (existingAccount == null) throw new KeyNotFoundException($"Учетная запись с ID {existingAccount.id} не найдена.");

			// Смена пароля
			existingAccount.password = PasswordHasher.HashPassword(newPassword);

			// Обновление данных. Данным методом можно обновить только указанные поля в account
			_context.Entry(existingAccount).CurrentValues.SetValues(account);
			await _context.SaveChangesAsync();

			return true; // Смена пароля прошла успешно
		}

		/// <summary>
		/// Получить состояние пользователя в системе
		/// </summary>
		public async Task<UserLoginStatus?> GetUserStatusInSystem()
		{
			// Чтение из JSON
			string JSON = File.ReadAllText(authorizationStatusPath);
			if (JSON == null) return null;
			UserLoginStatus? userLoginStatus = new UserLoginStatus();
			userLoginStatus = JsonConvert.DeserializeObject<UserLoginStatus>(JSON);

			return userLoginStatus;
		}

		/// <summary>
		/// Получить аккаунт авторизованного пользователя
		/// </summary>
		/// <remarks>Для работы с правами в системе.</remarks>
		public async Task<Account> GetUserAccount()
		{
			// Получаем данные авторизованного пользователя
			UserLoginStatus? userLoginStatus = new UserLoginStatus();
			userLoginStatus = await GetUserStatusInSystem();
			Account account = await _context.Accounts.FirstOrDefaultAsync(account => account.id == userLoginStatus.accountId);
			if (account == null) throw new KeyNotFoundException($"Учетная запись с ID {account.id} не найдена.");
			return account;
		}

		/// <summary>
		/// Кол-во пользователей сервиса
		/// </summary>
		public async Task<int> NumberUsers()
		{
			int? user = _context.Accounts.Count(account => account.accountRole == "Пользователь");
			if(user == null) { return 0; }
			return user.Value;
		}

	}
}
