using CompanyNews.Helpers;
using CompanyNews.Helpers.Event;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using CompanyNews.Services;
using CompanyNews.Views.AdminApp.WorkingWithData;
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
			SettingUpPage(); // Первоначальная настройка страницы
			LoadAccount(); // Выводим список на экран

			// Подписываемся на событие — успшное добавление данных.
			WorkingWithDataEvent.dataWasAddedSuccessfullyAccount += DataWasAddedSuccessfullyAccount;
			// Подписываемся на событие — успшное изменение данных.
			WorkingWithDataEvent.dataWasChangedSuccessfullyAccount += DataWasChangedSuccessfullyAccount;
		}

		#region CRUD Operations

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async void SettingUpPage()
		{
			DefaultListSelected = true; // Список аккаунтов по умолчанию
			ListBlockedAccountsSelected = false; // Список заблокирванных аккаунтов не отображается
			DarkBackground = Visibility.Collapsed; // Скрываем фон для Popup
		}

		/// <summary>
		/// Успшное добавление данных
		/// </summary>
		public async void DataWasAddedSuccessfullyAccount(object sender, EventAggregator e)
		{
			await Task.Delay(500);
			systemMessage.Text = $"Учетная запись успешно создана.";
			systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
			// Исчезание сообщения
			BeginFadeAnimation(systemMessage);
			BeginFadeAnimation(systemMessageBorder);
		}

		/// <summary>
		/// Успшное изменение данных
		/// </summary>
		public async void DataWasChangedSuccessfullyAccount(object sender, EventAggregator e)
		{
			await Task.Delay(500);
			systemMessage.Text = $"Учетная запись успешно изменена.";
			systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
			// Исчезание сообщения
			BeginFadeAnimation(systemMessage);
			BeginFadeAnimation(systemMessageBorder);
		}

		/// <summary>
		/// Вывод списка всех учетных записей в UI.
		/// </summary>
		private async Task LoadAccount()
		{
			if (DefaultListSelected)
			{
				ListAccountExtendeds.Clear(); // Чистка коллекции перед заполнением
				var accounts = await _accountService.GetAllAccountsAsync();
				foreach (var account in accounts)
				{
					ListAccountExtendeds.Add(account);
				}
			}

			if (ListBlockedAccountsSelected)
			{
				ListAccountExtendeds.Clear(); // Чистка коллекции перед заполнением
				var accounts = await _accountService.GetAllAccountsAsync();
				foreach (var account in accounts)
				{
					if(account.isProfileBlocked) 
						ListAccountExtendeds.Add(account);
				}
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
						HamburgerMenuEvent.CloseHamburgerMenu(); // Закрываем, если открыто "гамбургер меню"
						AccountPageFrame = new AccountWorkingPage(isAddData, SelectedAccount);
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
						HamburgerMenuEvent.CloseHamburgerMenu(); // Закрываем, если открыто "гамбургер меню"
						AccountPageFrame = new AccountWorkingPage(isAddData, SelectedAccount);

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
						StartPoupDeleteData = true; // отображаем Popup
						DarkBackground = Visibility.Visible; // показать фон
						
						if(SelectedAccount != null)
						{
							DataDeleted = $"Имя и фамилия: {SelectedAccount.name} {SelectedAccount.surname}\nРоль: \"{SelectedAccount.accountRole}\"\nЛогин: \"{SelectedAccount.login}\"";
						}

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
		/// Кнопка сохранения новых или изменения старых данных аккаунта в UI Popup
		/// </summary>

		private RelayCommand _saveData { get; set; }
		public RelayCommand SaveData
		{
			get
			{
				return _saveData ??
					(_saveData = new RelayCommand(async (obj) =>
					{
						Account account = await _accountService.AccountExtendedConvert(SelectedAccount);
						if (account != null)
						{
							await DeleteAccountAsync(account);

							if(systemMessage != null && systemMessageBorder != null)
							{
								await ClosePopupWorkingWithData(); // Скрываем Popup
								// Выводим сообщение об успешном удалении данных
								systemMessage.Text = $"Пользователь успешно удален.";
								systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
								// Исчезание сообщения
								BeginFadeAnimation(systemMessage);
								BeginFadeAnimation(systemMessageBorder);
							}

						}
						

					}, (obj) => true));
			}
		}

		#region UsersSearch

		// Cписок для фильтров таблицы
		public ObservableCollection<AccountExtended> ListSearch { get; set; } = new ObservableCollection<AccountExtended>();

		/// <summary>
		/// Поиск данных в таблицы через строку запроса
		/// </summary>
		public async Task UserSearch(string searchByValue)
		{
			if (!string.IsNullOrWhiteSpace(searchByValue))
			{
				await LoadAccount(); // обновляем список
				ListSearch.Clear(); // очищаем список поиска данных

				// Объединяем атрибуты сущности для поиска
				foreach (AccountExtended item in ListAccountExtendeds)
				{
					string workDepartmentName = "";
					if (item.workDepartmentName == null) { workDepartmentName = ""; } else { workDepartmentName = item.workDepartmentName; }
					string patronymic = "";
					if (item.patronymic == null) { patronymic = ""; } else { patronymic = item.patronymic; }

					string unification = item.login.ToLower() + " " + item.accountRole.ToLower() + " " +
						workDepartmentName.ToLower() + " " + item.phoneNumber.ToLower() + " " + item.name.ToLower() + " " +
						item.surname.ToLower() + " " + patronymic.ToLower();

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
					if (systemMessage != null && systemMessageBorder != null)
					{
						// Оповещениие об отсутствии данных
						systemMessage.Text = $"Пользователь не найден.";
						systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
						// Исчезание сообщения
						BeginFadeAnimation(systemMessage);
						BeginFadeAnimation(systemMessageBorder);
					}
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
			// Закрываем Popup
			StartPoupDeleteData = false;
			DarkBackground = Visibility.Collapsed; // Скрываем фон
		}

		#region FeaturesPopup

		/// <summary>
		/// Popup удаления данных
		/// </summary>
		private bool _startPoupDeleteData { get; set; }
		public bool StartPoupDeleteData
		{
			get { return _startPoupDeleteData; }
			set
			{
				_startPoupDeleteData = value;
				OnPropertyChanged(nameof(StartPoupDeleteData));
			}
		}

		/// <summary>
		/// Данные передаются в Popup, как предпросмотр перед удалением
		/// </summary>
		private string _dataDeleted { get; set; }
		public string DataDeleted
		{
			get { return _dataDeleted; }
			set { _dataDeleted = value; OnPropertyChanged(nameof(DataDeleted)); }
		}

		/// <summary>
		/// Затемненный фон позади Popup
		/// </summary>
		private Visibility _darkBackground { get; set; }
		public Visibility DarkBackground
		{
			get { return _darkBackground; }
			set
			{
				_darkBackground = value;
				OnPropertyChanged(nameof(DarkBackground));
			}
		}

		#endregion

		#endregion

		#region Features

		/// <summary>
		/// Асинхронно получаем информацию из привязанного View
		/// </summary>
		public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters)
		{
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			errorInputPopup = adminViewModelParameters.errorInputPopup;
			systemMessage = adminViewModelParameters.errorInputText;
			systemMessageBorder = adminViewModelParameters.errorInputBorder;
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
				OnPropertyChanged(nameof(IsWorkButtonEnableEdit));
				OnPropertyChanged(nameof(IsWorkButtonEnableDelete)); 
			}
		}

		/// <summary>
		/// Отображение или скрытие кнопки «редактировать» в UI. 
		/// </summary>
		private bool _isWorkButtonEnableEdit { get; set; }
		public bool IsWorkButtonEnableEdit
		{
			get { return SelectedAccount != null; } // Если в таблице выбранн объект, то кнопки доступны
			set { _isWorkButtonEnableEdit = value; OnPropertyChanged(nameof(IsWorkButtonEnableEdit)); }
		}

		/// <summary>
		/// Отображение или скрытие кнопки «удалить» в UI.
		/// </summary>
		private bool _isWorkButtonEnableDelete { get; set; }
		public bool IsWorkButtonEnableDelete
		{
			get { return SelectedAccount != null && SelectedAccount.accountRole != "Администратор"; } // Если выбрана роль Admin, то удалить ее нельзя
			set { _isWorkButtonEnableDelete = value; OnPropertyChanged(nameof(IsWorkButtonEnableDelete)); }
		}

		/// <summary>
		/// Добавление или редактирование данных
		/// </summary>
		/// <remarks>true - добавление данных; false - редактирование данных</remarks>
		private bool isAddData { get; set; }

		#region View

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
		/// Выбран список сортировки по умолчанию в UI
		/// </summary>
		private bool _defaultListSelected { get; set; }
		public bool DefaultListSelected
		{
			get { return _defaultListSelected; }
			set
			{
				_defaultListSelected = value; OnPropertyChanged(nameof(DefaultListSelected));
				LoadAccount();
			}
		}

		/// <summary>
		/// Выбран список заблокированных аккаунтов в UI
		/// </summary>
		private bool _listBlockedAccountsSelected { get; set; }
		public bool ListBlockedAccountsSelected
		{
			get { return _listBlockedAccountsSelected; }
			set
			{
				_listBlockedAccountsSelected = value; OnPropertyChanged(nameof(ListBlockedAccountsSelected));
				LoadAccount();
			}
		}

		// Page для запуска страницы
		private Page _accountPageFrame { get; set; }
		public Page AccountPageFrame
		{
			get { return _accountPageFrame; }
			set { _accountPageFrame = value; OnPropertyChanged(nameof(AccountPageFrame)); }
		}

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
