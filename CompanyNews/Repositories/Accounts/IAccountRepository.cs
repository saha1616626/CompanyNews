using CompanyNews.Models;
using CompanyNews.Models.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.Accounts
{
	/// <summary>
	/// Интерфейс репозитория — управляет доступом базы данных для работы с учетными записями
	/// </summary>
	public interface IAccountRepository
    {
		/// <summary>
		/// Получение AccountExtended из Account. 
		/// Замена идентификатора на соответствующее значение из БД 
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		AccountExtended AcountConvert(Account account);
		/// <summary>
		/// Получение Account из AccountExtended. 
		/// Замена значения на соответствующий идентификатор из БД 
		/// </summary>
		/// <param name="accountExtended"></param>
		/// <returns></returns>
		Account AccountExtendedConvert(AccountExtended accountExtended);

		/// <summary>
		/// Получение аккаунта
		/// </summary>
		/// <param name="id">id пользователя</param>
		Task<AccountExtended> GetAccountByIdAsync(int id);

		/// <summary>
		/// Получение списка аккаунтов
		/// </summary>
		IEnumerable<AccountExtended> GetAllAccountsAsync();

		/// <summary>
		/// Добавить аккаунт
		/// </summary>
		/// <param name="account">Данные нового аккаунта</param>
		Task<Account> AddAccountAsync(Account account);

		/// <summary>
		/// Изменить аккаунт
		/// </summary>
		/// <param name="account">Измененные данные</param>
		Task UpdateAccountAsync(Account account);

		/// <summary>
		/// Удалить аккаунт
		/// </summary>
		/// <param name="id">id пользователя</param>
		Task DeleteAccountAsync(int id);

		/// <summary>
		/// Зашифровать пароль
		/// </summary>
		/// <param name="password"> Пароль из UI. </param>
		Task<string> EncryptPassword(string password);

	}
}
