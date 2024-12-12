using CompanyNews.Data;
using CompanyNews.Helpers;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.Accounts
{
	/// <summary>
	/// Реализация интерфейса управления учетными записями через CompanyNewsDbContext
	/// </summary>
	public class AccountRepository : IAccountRepository
    {
		private readonly CompanyNewsDbContext _context;

		public AccountRepository(CompanyNewsDbContext context)
		{
			_context = context;
		}

		#region Convert

		/// <summary>
		/// Замена идентификатора на соответствующее значение из БД 
		/// </summary>
		public async Task<AccountExtended?> AcountConvert(Account? account)
		{
			// Проверяем, не равен ли account null
			if (account == null) { return null; }

			AccountExtended accountExtended = new AccountExtended();
			accountExtended.id = account.id;
			accountExtended.login = account.login;
			accountExtended.password = account.password;
			accountExtended.accountRole = account.accountRole;

			if (account.workDepartmentId != null)
			{
				// Получаем значение должности по id
				WorkDepartment? workDepartment = await _context.WorkDepartments
					.FirstOrDefaultAsync(wd => wd.id == account.workDepartmentId);

				if (workDepartment == null) { accountExtended.workDepartmentId = account.workDepartmentId; accountExtended.workDepartmentName = null; }
				else
				{
					accountExtended.workDepartmentId = account.workDepartmentId;
					accountExtended.workDepartmentName = workDepartment.name;
				}
			}
			else { accountExtended.workDepartmentId = null; accountExtended.workDepartmentName = null; }

			accountExtended.phoneNumber = account.phoneNumber;
			accountExtended.name = account.name;
			accountExtended.surname = account.surname;
			if (account.profileDescription != null) { accountExtended.profileDescription = account.profileDescription; }
			if (account.image != null) { accountExtended.image = account.image; }
			if (account.patronymic != null) { accountExtended.patronymic = account.patronymic; }
			accountExtended.isProfileBlocked = account.isProfileBlocked;
			if (account.reasonBlockingAccount != null) { accountExtended.reasonBlockingAccount = account.reasonBlockingAccount; }
			accountExtended.isCanLeaveComments = account.isCanLeaveComments;
			if (account.reasonBlockingMessages != null) { accountExtended.reasonBlockingMessages = account.reasonBlockingMessages; }

			return accountExtended;
		}

		/// <summary>
		/// Замена значения на соответствующий идентификатор из БД 
		/// </summary>
		public async Task<Account?> AccountExtendedConvert(AccountExtended? accountExtended)
		{
			// Проверяем, не равен ли accountExtended null
			if (accountExtended == null) { return null; }

			Account account = new Account();
			account.id = accountExtended.id;
			account.login = accountExtended.login;
			account.password = accountExtended.password;
			account.accountRole = accountExtended.accountRole;
			account.workDepartmentId = accountExtended.workDepartmentId;
			account.phoneNumber = accountExtended.phoneNumber;
			account.name = accountExtended.name;
			account.surname = accountExtended.surname;
			if (accountExtended.profileDescription != null) { account.profileDescription = accountExtended.profileDescription; }
			if (accountExtended.image != null) { account.image = accountExtended.image; }
			if (accountExtended.patronymic != null) { account.patronymic = accountExtended.patronymic; }
			account.isProfileBlocked = accountExtended.isProfileBlocked;
			if (accountExtended.reasonBlockingAccount != null) { account.reasonBlockingAccount = accountExtended.reasonBlockingAccount; }
			account.isCanLeaveComments = accountExtended.isCanLeaveComments;
			if (accountExtended.reasonBlockingMessages != null) { account.reasonBlockingMessages = accountExtended.reasonBlockingMessages; }

			return account;
		}

		#endregion

		#region GettingData

		/// <summary>
		/// Получение аккаунта по id
		/// </summary>
		public async Task<AccountExtended?> GetAccountByIdAsync(int id)
		{
			Account? account = await _context.Accounts.FindAsync(id);
			if (account == null) { return null; }
	
			// Преобразование идентификатора на соответствующие значение из БД 
			return await AcountConvert(account);	
		}

		/// <summary>
		/// Получение списка аккаунтов
		/// </summary>
		public async Task<IEnumerable<AccountExtended>?> GetAllAccountsAsync()
		{
			// Получаем список аккаунтов
			IEnumerable<Account> accounts = await _context.Accounts.ToListAsync();
			if (accounts == null) { return null; }

			// Список аккаунтов
			List<AccountExtended> accountExtendeds = new List<AccountExtended>();

			foreach (var account in accounts)
			{
				// Преобразование идентификатора на соответствующие значение из БД
				if(await AcountConvert(account) == null) { continue; }
				AccountExtended accountExtended = await AcountConvert(account);
				accountExtendeds.Add(accountExtended);
			}

			return accountExtendeds;
		}

		#endregion

		#region WorkingData

		/// <summary>
		/// Добавить аккаунт
		/// </summary>
		public async Task<Account> AddAccountAsync(Account account)
		{
			_context.Accounts.Add(account);
			await _context.SaveChangesAsync();
			return account; // Возвращаем объект с обновленными данными, включая Id
		}

		/// <summary>
		/// Изменить аккаунт
		/// </summary>
		public async Task UpdateAccountAsync(Account account)
		{
			if (account == null) throw new ArgumentNullException(nameof(account));

			// Убедимся, что учетная запись существует
			var existingAccount = await _context.Accounts.FindAsync(account.id);
			if (existingAccount == null) throw new KeyNotFoundException($"Учетная запись с ID {existingAccount.id} не найдена.");

			// Обновление данных. Данным методом можно обновить только указанные поля в account
			_context.Entry(existingAccount).CurrentValues.SetValues(account);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Удалить аккаунт
		/// </summary>
		public async Task DeleteAccountAsync(int id)
		{
			var account = await _context.Accounts.FindAsync(id);
			if (account == null) { return; }
			_context.Accounts.Remove(account);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Зашифровать пароль
		/// </summary>
		/// <param name="password"> Пароль из UI. </param>
		public async Task<string> EncryptPassword(string password)
		{
			return PasswordHasher.HashPassword(password);
		}

		#endregion
	}
}
