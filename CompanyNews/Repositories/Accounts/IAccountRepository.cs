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
		Task<AccountExtended> AcountConvert(Account account);
		/// <summary>
		/// Получение Account из AccountExtended. 
		/// Замена значения на соответствующий идентификатор из БД 
		/// </summary>
		/// <param name="accountExtended"></param>
		/// <returns></returns>
		Task<Account> AcountConvert(AccountExtended accountExtended);

		/// <summary>
		/// Получение аккаунта
		/// </summary>
		/// <param name="id">id пользователя</param>
		/// <returns></returns>
		Task<AccountExtended> GetAccountByIdAsync(int id);

		/// <summary>
		/// Получение списка аккаунтов
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<AccountExtended>> GetAllAccountsAsync();

		/// <summary>
		/// Добавить аккаунт
		/// </summary>
		/// <param name="account">Данные нового аккаунта</param>
		/// <returns></returns>
		Task AddAccountAsync(Account account);

		/// <summary>
		/// Изменить аккаунт
		/// </summary>
		/// <param name="account">Измененные данные</param>
		/// <returns></returns>
		Task UpdateAccountAsync(Account account);

		/// <summary>
		/// Удалить аккаунт
		/// </summary>
		/// <param name="id">id пользователя</param>
		/// <returns></returns>
		Task DeleteAccountAsync(int id);
    }
}
