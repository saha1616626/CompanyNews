using CompanyNews.Helpers;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using CompanyNews.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.ViewModels.AdminApp
{
	public class AccountViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly AccountService _accountService;

		/// <summary>
		/// Отображаемый список учетных записей в UI
		/// </summary>
		public ObservableCollection<AccountExtended> ListAccountExtendeds;

		public AccountViewModel(AccountService accountService)
		{
			_accountService = accountService;
			ListAccountExtendeds = new ObservableCollection<AccountExtended>();

		}

		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех учетных записей в UI.
		/// </summary>
		private async void LoadAccount()
		{
			var accounts = await _accountService.GetAllAccountsAsync();
			foreach (var account in accounts)
			{
				ListAccountExtendeds.Add(account);
			}
		}

		/// <summary>
		/// Добавить аккаунт
		/// </summary>
		public async Task AddAccountAsync(Account account)
		{
			var addedAccount = await _accountService.AddAccountAsync(account); // Добавление в БД + возврат обновленного объекта
			ListAccountExtendeds.Add(await _accountService.AcountConvert(addedAccount)); // Обновление коллекции
		}

		/// <summary>
		/// Изменить аккаунт
		/// </summary>
		public async Task UpdateAccountAsync(Account account)
		{
			await _accountService.UpdateAccountAsync(account); // Обновление данных в БД

			// Находим учетную запись в списке для отображения в UI и заменяем объект
			AccountExtended? accountExtended = ListAccountExtendeds.FirstOrDefault(a => a.id == account.id);
			if (accountExtended != null) { accountExtended = await _accountService.AcountConvert(account); }
		}

		/// <summary>
		/// Удалить аккаунт
		/// </summary>
		public async Task DeleteAccountAsync(Account account)
		{
			await _accountService.DeleteAccountAsync(account.id); // Удаление из БД

			// Находим учетную запись в списке для отображения в UI и удаляем объект
			AccountExtended? accountExtended = ListAccountExtendeds.FirstOrDefault(a => a.id == account.id);
			if (accountExtended != null) { ListAccountExtendeds.Remove(accountExtended); }	
		}

		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Кнопка добавления аккаунта в UI
		/// </summary>
		private RelayCommand _addAccount { get; set; }
		public RelayCommand AddAccount
		{
			get
			{
				return _addAccount ??
					(_addAccount = new RelayCommand(async (obj) =>
					{
						
					}, (obj) => true));
			}
		}

		#endregion

		#region Features

		/// <summary>
		/// Выбранный аккаунт в UI
		/// </summary>
		private AccountExtended _selectedAccount {  get; set; }
		public AccountExtended SelectedAccount
		{
			get { return _selectedAccount; }
			set { _selectedAccount = value; OnPropertyChanged(nameof(SelectedAccount)); }
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
