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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace CompanyNews.ViewModels.AdminApp
{
	public class AccountViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервисы для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly AccountService _accountService; //Учетные записи

		private readonly AuthorizationService _authorizationService; // Авторизация

		public ObservableCollection<AccountExtended> _listAccountExtendeds { get; set; }
		/// <summary>
		/// Отображаемый список учетных записей в UI
		/// </summary>
		public ObservableCollection<AccountExtended> ListAccountExtendeds
		{
			get { return _listAccountExtendeds; }
			set { _listAccountExtendeds = value; OnPropertyChanged(nameof(ListAccountExtendeds)); }
		}

		public AccountViewModel()
		{
			_accountService = ServiceLocator.GetService<AccountService>();
			ListAccountExtendeds = new ObservableCollection<AccountExtended>();
			LoadAccount(); // Выводим список на экран
		}

		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех учетных записей в UI.
		/// </summary>
		private async Task LoadAccount()
		{
			ListAccountExtendeds.Clear(); // Чистка коллекции перед заполнением
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

		/// <summary>
		/// Смена пароля
		/// </summary>
		/// <param name="account"> Учетная запись у которой меняется пароль. </param>
		/// <param name="newPassword"> Новый пароль. </param>
		/// <returns></returns>
		public async Task<bool> ChangingPassword(Account account, string newPassword)
		{
			return await _authorizationService.ChangingPassword(account, newPassword);
		}



		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Кнопка "добавить" аккаунт в UI
		/// </summary>
		private RelayCommand _addAccount { get; set; }
		public RelayCommand AddAccount
		{
			get
			{
				return _addAccount ??
					(_addAccount = new RelayCommand(async (obj) =>
					{
						isAddData = true;

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "изменить" аккаунт в UI
		/// </summary>
		private RelayCommand _editAccount { get; set; }
		public RelayCommand EditAccount
		{
			get
			{
				return _editAccount ??
					(_editAccount = new RelayCommand(async (obj) =>
					{
						isAddData = false;


					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "удалить" аккаунт в UI
		/// </summary>
		private RelayCommand _deleteAccount { get; set; }
		public RelayCommand DeleteAccount
		{
			get
			{
				return _deleteAccount ??
					(_deleteAccount = new RelayCommand(async (obj) =>
					{

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка смены пароля аккаунта в UI
		/// </summary>
		private RelayCommand _saveNewPassword { get; set; }
		public RelayCommand SaveNewPassword
		{
			get
			{
				return _saveNewPassword ??
					(_saveNewPassword = new RelayCommand(async (obj) =>
					{

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка сохранения новых или изменения старых данных аккаунта в UI
		/// </summary>

		private RelayCommand _saveData { get; set; }
		public RelayCommand SaveData
		{
			get
			{
				return _saveData ??
					(_saveData = new RelayCommand(async (obj) =>
					{

						if (isAddData) // Логика при добавлении данных
						{

						}
						else // Логика при редактировании данных
						{

						}

					}, (obj) => true));
			}
		}

		#region UsersSearch

		// список для фильтров таблицы
		public ObservableCollection<AccountExtended> ListSearch { get; set; } = new ObservableCollection<AccountExtended>();

		public async Task UserSearch(string searchByValue)
		{
			if (!string.IsNullOrWhiteSpace(searchByValue))
			{
				await LoadAccount(); // обновляем список
				ListSearch.Clear(); // очищаем список поиска данных

				// Объединяем атрибуты сущности для поиска
				foreach (AccountExtended item in ListAccountExtendeds)
				{
					string unification = item.login.ToLower() + " " + item.accountRole + " " +
						item.workDepartmentName + " " + item.phoneNumber + " " + item.name + " " +
						item.surname + " " + item.patronymic;

					bool dataExists = unification.Contains(searchByValue.ToLowerInvariant());

					if (dataExists)
					{
						ListSearch.Add(item);
					}
				}

				ListAccountExtendeds.Clear(); // Очистка список перед заполнением
				ListAccountExtendeds = new ObservableCollection<AccountExtended>(ListSearch); // Обновление списка

				if (ListSearch.Count == 0)
				{
					// Оповещениие об отсутствии данных
					systemMessage.Text = "Пользователь не найдена!"; // Собщение об ошибке
					BeginFadeAnimation(systemMessageBorder); // Анимация затухания ошибки
				}
			}
			else
			{
				ListAccountExtendeds.Clear(); // Очистка список перед заполнением
				await LoadAccount(); // обновляем список
			}
		}

		#endregion

		#endregion

		#region Popup

		/// <summary>
		/// Скрыть popup (все открытые)
		/// </summary>
		private RelayCommand _closePopup { get; set; }
		public RelayCommand ClosePopup
		{
			get
			{
				return _closePopup ??
					(_closePopup = new RelayCommand(async (obj) =>
					{
						await ClosePopupWorkingWithData();
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Скрытие popup
		/// </summary>
		private async Task ClosePopupWorkingWithData()
		{

		}

		#endregion

		#region Features

		/// <summary>
		/// Асинхронно получаем информацию из привязанного View
		/// </summary>
		public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters)
		{
			darkBackground = adminViewModelParameters.darkBackground;
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			errorInputPopup = adminViewModelParameters.errorInputPopup;
			systemMessage = adminViewModelParameters.errorInputText;
			systemMessageBorder = adminViewModelParameters.errorInputBorder;
			deleteDataPopup = adminViewModelParameters.deleteDataPopup;
		}

		/// <summary>
		/// Выбранный аккаунт в UI
		/// </summary>
		private AccountExtended _selectedAccount { get; set; }
		public AccountExtended SelectedAccount
		{
			get { return _selectedAccount; }
			set
			{
				_selectedAccount = value; OnPropertyChanged(nameof(SelectedAccount));
				OnPropertyChanged(nameof(IsWorkButtonEnable));
			}
		}

		/// <summary>
		/// Отображение кнопки «удалить» и «редактировать» в UI.
		/// </summary>
		private bool _isWorkButtonEnable { get; set; }
		public bool IsWorkButtonEnable
		{
			get { return SelectedAccount != null; } // Если в таблице выбранн объект, то кнопки доступны
			set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
		}

		/// <summary>
		/// Добавление или редактирование данных
		/// </summary>
		/// <remarks>true - добавление данных; false - редактирование данных</remarks>
		private bool isAddData { get; set; }

		#region View

		/// <summary>
		/// Затемненный фон позади Popup
		/// </summary>
		public Border? darkBackground { get; set; }

		/// <summary>
		/// Анимация полей
		/// </summary>
		public Storyboard? fieldIllumination { get; set; }

		/// <summary>
		/// Вывод сообщения системы и анимация текста на странице
		/// </summary>
		public TextBlock? systemMessage { get; set; }

		/// <summary>
		/// Вывод контейнера для сообщения системы
		/// </summary>
		public Border? systemMessageBorder { get; set; }

		/// <summary>
		/// Вывод ошибки и анимация текста в Popup
		/// </summary>
		public TextBlock? errorInputPopup { get; set; }

		/// <summary>
		/// Popup удаления данных
		/// </summary>
		public Popup? deleteDataPopup { get; set; }

		#endregion

		#endregion

		#region Animation

		// выводим сообщения об ошибке с анимацией затухания
		public async void BeginFadeAnimation(TextBlock textBlock)
		{
			textBlock.IsEnabled = true;
			textBlock.Opacity = 1.0;

			Storyboard storyboard = new Storyboard();
			DoubleAnimation fadeAnimation = new DoubleAnimation
			{
				From = 2.0,
				To = 0.0,
				Duration = TimeSpan.FromSeconds(2),
			};
			Storyboard.SetTargetProperty(fadeAnimation, new System.Windows.PropertyPath(System.Windows.UIElement.OpacityProperty));
			storyboard.Children.Add(fadeAnimation);
			storyboard.Completed += (s, e) => textBlock.IsEnabled = false;
			storyboard.Begin(textBlock);
		}

		public async void BeginFadeAnimation(Border border)
		{
			border.IsEnabled = true;
			border.Opacity = 1.0;

			Storyboard storyboard = new Storyboard();
			DoubleAnimation fadeAnimation = new DoubleAnimation
			{
				From = 2.0,
				To = 0.0,
				Duration = TimeSpan.FromSeconds(2),
			};
			Storyboard.SetTargetProperty(fadeAnimation, new System.Windows.PropertyPath(System.Windows.UIElement.OpacityProperty));
			storyboard.Children.Add(fadeAnimation);
			storyboard.Completed += (s, e) => border.IsEnabled = false;
			storyboard.Begin(border);
		}

		// запускаем анимации для TextBox (подсвечивание объекта)
		private void StartFieldIllumination(TextBox textBox)
		{
			fieldIllumination.Begin(textBox);
		}
		private void StartFieldIllumination(PasswordBox passwordBox)
		{
			fieldIllumination.Begin(passwordBox);
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
