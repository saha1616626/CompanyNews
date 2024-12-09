﻿using CompanyNews.Models;
using CompanyNews.Models.Extended;
using CompanyNews.Repositories.Accounts;
using CompanyNews.Repositories.NewsPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Services
{
	/// <summary>
	/// Сервис обеспечивает бизнес-логику взаимодействия с репозиторием "учетная запись"
	/// </summary>
	public class AccountService
	{
		private readonly IAccountRepository _accountRepository;
		public AccountService(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		#region Convert

		public async Task<AccountExtended?> AcountConvert(Account? account)
		{
			return await _accountRepository.AcountConvert(account);
		}

		public async Task<Account?> AccountExtendedConvert(AccountExtended? accountExtended)
		{
			return await _accountRepository.AccountExtendedConvert(accountExtended);
		}

		#endregion

		#region GettingData

		public async Task<AccountExtended> GetAccountByIdAsync(int id)
		{
			return await _accountRepository.GetAccountByIdAsync(id);
		}

		public async Task<IEnumerable<AccountExtended>> GetAllAccountsAsync()
		{
			return await _accountRepository.GetAllAccountsAsync();
		}

		#endregion

		#region CRUD Operations

		public async Task<Account> AddAccountAsync(Account account)
		{
			var addedAccount = await _accountRepository.AddAccountAsync(account);
			return addedAccount; // Возвращаем добавленную учетную запись
		}

		public async Task UpdateAccountAsync(Account account)
		{
			await _accountRepository.UpdateAccountAsync(account);
		}

		public async Task DeleteAccountAsync(int id)
		{
			await _accountRepository.DeleteAccountAsync(id);
		}

		#endregion

	}
}