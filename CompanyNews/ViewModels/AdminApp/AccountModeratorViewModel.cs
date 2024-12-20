using CompanyNews.Helpers.Event;
using CompanyNews.Helpers;
using CompanyNews.Models.Extended;
using CompanyNews.Models;
using CompanyNews.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace CompanyNews.ViewModels.AdminApp
{
	public class AccountModeratorViewModel : INotifyPropertyChanged
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

		public AccountModeratorViewModel()
		{
			_accountService = ServiceLocator.GetService<AccountService>();
			ListAccountExtendeds = new ObservableCollection<AccountExtended>();
			SettingUpPage(); // Первоначальная настройка страницы
			LoadAccount(); // Выводим список на экран

			// Кнопки блокировки сообщений
			IsAccountBlockMessage = false;
			IsAccountResoreMessage = false;
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
		/// Вывод списка всех учетных записей в UI.
		/// </summary>
		private async Task LoadAccount()
		{
			if (DefaultListSelected)
			{
				ListAccountExtendeds.Clear(); // Чистка коллекции перед заполнением
				var accounts = await _accountService.GetAllAccountsAsync();
				foreach (var account in accounts.Reverse())
				{
					if (account.accountRole != "Администратор" && account.accountRole != "Модератор")
						ListAccountExtendeds.Add(account);
				}
			}

			if (ListUnBlockedAccountsSelected)
			{
				ListAccountExtendeds.Clear(); // Чистка коллекции перед заполнением
				var accounts = await _accountService.GetAllAccountsAsync();
				foreach (var account in accounts.Reverse())
				{
					if (!account.isCanLeaveComments)
						if (account.accountRole != "Администратор" && account.accountRole != "Модератор")
							ListAccountExtendeds.Add(account);
				}
			}

			if (ListBlockedAccountsSelected)
			{
				ListAccountExtendeds.Clear(); // Чистка коллекции перед заполнением
				var accounts = await _accountService.GetAllAccountsAsync();
				foreach (var account in accounts.Reverse())
				{
					if (account.isCanLeaveComments)
						if (account.accountRole != "Администратор" && account.accountRole != "Модератор")
							ListAccountExtendeds.Add(account);
				}
			}

		}



		#endregion

		#region UI RelayCommand Operations		

		/// <summary>
		/// Кнопка для блокировки или разблокировки пользователя пользователя
		/// </summary>

		private RelayCommand _blockAccount { get; set; }
		public RelayCommand BlockAccount
		{
			get
			{
				return _blockAccount ??
					(_blockAccount = new RelayCommand(async (obj) =>
					{

						if (IsBlockAccount) // Заблокировать пользователя 
						{
							// Проверяем поле на заполнение
							if (descriptionBlocking != null)
							{
								if (!string.IsNullOrWhiteSpace(descriptionBlocking.Text))
								{
									// Скрываем Popup
									await ClosePopupWorkingWithData();

									SelectedAccount.isCanLeaveComments = true;
									SelectedAccount.reasonBlockingMessages = descriptionBlocking.Text.Trim();


									Account account = new Account();
									account.id = SelectedAccount.id;
									account.password = SelectedAccount.password;
									account.login = SelectedAccount.login;
									account.accountRole = SelectedAccount.accountRole;
									account.workDepartmentId = SelectedAccount.workDepartmentId;
									account.phoneNumber = SelectedAccount.phoneNumber;
									account.name = SelectedAccount.name;
									account.surname = SelectedAccount.surname;
									account.patronymic = SelectedAccount.patronymic;
									account.isProfileBlocked = SelectedAccount.isProfileBlocked;
									account.reasonBlockingAccount = SelectedAccount.reasonBlockingAccount;
									account.isCanLeaveComments = true;
									account.reasonBlockingMessages = descriptionBlocking.Text.Trim();

									await _accountService.UpdateAccountAsync(account);
									IsAccountBlockMessage = false;
									IsAccountResoreMessage = true;

									if (ListUnBlockedAccountsSelected)
									{
										ListAccountExtendeds.Remove(SelectedAccount);
									}

									AccountExtended accountExtended = ListAccountExtendeds.FirstOrDefault(a => a.id == SelectedAccount.id);
									if (accountExtended != null)
									{
										accountExtended.isCanLeaveComments = true;
										accountExtended.reasonBlockingMessages = descriptionBlocking.Text.Trim();
									}

									systemMessage.Text = $"Пользователь успешно заблокирован.";
									systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
									// Исчезание сообщения
									BeginFadeAnimation(systemMessage);
									BeginFadeAnimation(systemMessageBorder);
								}
								else
								{
									StartFieldIllumination(descriptionBlocking); // Подсветка поля
									popupMessage.Text = "Заполните обязательное поле.";
									popupMessageBorder.Visibility = System.Windows.Visibility.Visible;
									// Исчезание сообщения
									BeginFadeAnimation(popupMessage);
									BeginFadeAnimation(popupMessageBorder);
								}
							}
						}

						if (!IsBlockAccount) // Разблокировать пользоавателя
						{
							// Скрываем Popup
							await ClosePopupWorkingWithData();

							Account account = new Account();
							account.id = SelectedAccount.id;
							account.login = SelectedAccount.login;
							account.password = SelectedAccount.password;
							account.accountRole = SelectedAccount.accountRole;
							account.workDepartmentId = SelectedAccount.workDepartmentId;
							account.phoneNumber = SelectedAccount.phoneNumber;
							account.name = SelectedAccount.name;
							account.surname = SelectedAccount.surname;
							account.patronymic = SelectedAccount.patronymic;
							account.isProfileBlocked = SelectedAccount.isProfileBlocked;
							account.reasonBlockingAccount = SelectedAccount.reasonBlockingAccount;
							account.isCanLeaveComments = false;
							account.reasonBlockingMessages = "";

							_accountService.UpdateAccountAsync(account);

							IsAccountBlockMessage = true;
							IsAccountResoreMessage = false;

							if (ListBlockedAccountsSelected)
							{
								ListAccountExtendeds.Remove(SelectedAccount);
							}

							AccountExtended accountExtended = ListAccountExtendeds.FirstOrDefault(a => a.id == SelectedAccount.id);
							if (accountExtended != null)
							{
								accountExtended.isCanLeaveComments = false;
								accountExtended.reasonBlockingMessages = "";
							}

							systemMessage.Text = $"Пользователь успешно разблокирован.";
							systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
							// Исчезание сообщения
							BeginFadeAnimation(systemMessage);
							BeginFadeAnimation(systemMessageBorder);
						}


					}, (obj) => true));
			}
		}

		/// <summary>
		/// Определение состояние аккаунта оставлять комментарии
		/// </summary>
		public void StatusBlockMessage()
		{
			if (SelectedAccount != null)
			{
				if (SelectedAccount.isCanLeaveComments != null)
				{
					if (SelectedAccount.isCanLeaveComments)
					{
						IsAccountBlockMessage = false; // Кнопки блокировки сообщений
						IsAccountResoreMessage = true;
					}
					else
					{
						IsAccountBlockMessage = true; // Кнопки блокировки сообщений
						IsAccountResoreMessage = false;
					}
				}
				else
				{
					IsAccountBlockMessage = false; // Кнопки блокировки сообщений
					IsAccountResoreMessage = false;
				}
			}
			else
			{
				IsAccountBlockMessage = false; // Кнопки блокировки сообщений
				IsAccountResoreMessage = false;
			}
		}

		/// <summary>
		/// Заблокировать пользователя
		/// </summary>
		private RelayCommand _accountBlockMessage { get; set; }
		public RelayCommand AccountBlockMessage
		{
			get
			{
				return _accountBlockMessage ??
					(_accountBlockMessage = new RelayCommand(async (obj) =>
					{
						// Запускаем Popup
						StartBlockingPoup = true;// отображаем Popup
						DarkBackground = Visibility.Visible; // показать фон
						descriptionBlocking.Text = ""; // Очищаем поле
						ActionsWithData = "Вы действительно хотите ограничить возможность пользователю комментировать публикации?";
						IsBlockAccount = true;
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Разблокировать пользователя
		/// </summary>
		private RelayCommand _accountResoreMessage { get; set; }
		public RelayCommand AccountResoreMessage
		{
			get
			{
				return _accountResoreMessage ??
					(_accountResoreMessage = new RelayCommand(async (obj) =>
					{
						// Запускаем Popup
						StartPoup = true;// отображаем Popup
						DarkBackground = Visibility.Visible; // показать фон
						ActionsWithData = "Вы действительно хотите разблокировать возможность пользователю комментировать публикации?";
						IsBlockAccount = false;
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
		/// Popup для работы с данными
		/// </summary>
		private bool _startPoup { get; set; }
		public bool StartPoup
		{
			get { return _startPoup; }
			set
			{
				_startPoup = value;
				OnPropertyChanged(nameof(StartPoup));
			}
		}

		private bool _startBlockingPoup { get; set; }
		public bool StartBlockingPoup
		{
			get { return _startBlockingPoup; }
			set
			{
				_startBlockingPoup = value;
				OnPropertyChanged(nameof(StartBlockingPoup));
			}
		}

		/// <summary>
		/// Данные передаются в Popup, как предпросмотр перед удалением
		/// </summary>
		private string _actionsWithData { get; set; }
		public string ActionsWithData
		{
			get { return _actionsWithData; }
			set { _actionsWithData = value; OnPropertyChanged(nameof(ActionsWithData)); }
		}


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
			StartBlockingPoup = false;
			StartPoup = false;
			DarkBackground = Visibility.Collapsed; // Скрываем фон
		}

		#region FeaturesPopup


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
		public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters, TextBlock PopupMessage, Border PopupMessageBorder,
			TextBox DescriptionBlocking)
		{
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			errorInputPopup = adminViewModelParameters.errorInputPopup;
			systemMessage = adminViewModelParameters.errorInputText;
			systemMessageBorder = adminViewModelParameters.errorInputBorder;
			popupMessage = PopupMessage;
			popupMessageBorder = PopupMessageBorder;
			descriptionBlocking = DescriptionBlocking;

		}

		/// <summary>
		/// Блокировка пользователя
		/// </summary>
		public bool IsBlockAccount { get; set; }

		/// <summary>
		/// Описание блокировки
		/// </summary>
		public TextBox descriptionBlocking { get; set; }

		/// <summary>
		/// Видимость кнопки заблокировать пользователя оставлять сообщения
		/// </summary>
		private bool _isAccountBlockMessage { get; set; }
		public bool IsAccountBlockMessage
		{
			get { return _isAccountBlockMessage; }
			set
			{
				_isAccountBlockMessage = value;
				OnPropertyChanged(nameof(IsAccountBlockMessage));
			}
		}

		/// <summary>
		/// Видимость кнопки разблокировать пользователя оставлять сообщения
		/// </summary>
		private bool _isAccountResoreMessage { get; set; }
		public bool IsAccountResoreMessage
		{
			get { return _isAccountResoreMessage; }
			set
			{
				_isAccountResoreMessage = value;
				OnPropertyChanged(nameof(IsAccountResoreMessage));
			}
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
				StatusBlockMessage(); // Отображение кнопки
			}
		}

		#region View

		/// <summary>
		/// Анимация полей
		/// </summary>
		public Storyboard? fieldIllumination { get; set; }


		/// <summary>
		/// Вывод ошибки и анимация текста в Popup
		/// </summary>
		public Border? systemMessageBorder { get; set; }

		public Border? popupMessageBorder { get; set; }

		/// <summary>
		/// Вывод ошибки и анимация текста на странице
		/// </summary>
		public TextBlock? systemMessage { get; set; }
		public TextBlock? popupMessage { get; set; }

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
		/// Выбран список разблокированных аккаунтов в UI
		/// </summary>
		private bool _listUnBlockedAccountsSelected { get; set; }
		public bool ListUnBlockedAccountsSelected
		{
			get { return _listUnBlockedAccountsSelected; }
			set
			{
				_listUnBlockedAccountsSelected = value; OnPropertyChanged(nameof(ListUnBlockedAccountsSelected));
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
