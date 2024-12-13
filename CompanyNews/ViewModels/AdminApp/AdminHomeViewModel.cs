using CompanyNews.Helpers;
using CompanyNews.Helpers.Event;
using CompanyNews.Models;
using CompanyNews.Models.Authorization;
using CompanyNews.Services;
using CompanyNews.Views.AdminApp;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CompanyNews.ViewModels.AdminApp
{
	public class AdminHomeViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly AuthorizationService _authorizationService;

		public AdminHomeViewModel()
		{
			_authorizationService = ServiceLocator.GetService<AuthorizationService>();
			SettingUpPage(); // Первоначальная настройка страницы

			// Подписываемся на событие — переход на страницу для работы с учетными записями.
			HamburgerMenuEvent.openPageAccount += OpenPageAccount;
			// Подписываемся на событие — переход на страницу для работы с рабочими отделами.
			HamburgerMenuEvent.openPageWorkDepartment += OpenPageWorkDepartment;
			// Подписываемся на событие — переход на страницу для работы с категориями постов.
			HamburgerMenuEvent.openPageNewsCategory += OpenPageNewsCategory;
			// Подписываемся на событие — переход на страницу для работы с новостными постами.
			HamburgerMenuEvent.openPageNewsPost += OpenPageNewsPost;
			// Подписываемся на событие — переход на страницу для работы с сообщениями пользователей.
			HamburgerMenuEvent.openPageMessageUser += OpenPageMessageUser;
		}

		#region WorkingWithPages

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async void SettingUpPage()
		{
			DarkBackground = Visibility.Collapsed; // Фон для Popup скрыт
			IsAccountIcon = true; // Кнопка перехода в ЛК отображается

			// Выводим имя и фамилию пользователя 
			Account account = await _authorizationService.GetUserAccount();
			if (account != null)
			{
				UserName = $"{account.name} {account.surname}";
			}
		}

		/// <summary>
		/// Запуск начальной страницы после запуска меню
		/// </summary>
		public async Task StartingHomePage()
		{
			// Запуск начальной страницы в зависимости от роли пользователя
			// Получаем роль
			UserLoginStatus userLoginStatus = await _authorizationService.GetUserStatusInSystem();
			if (userLoginStatus != null)
			{
				if (userLoginStatus.accountRole == "Администратор")
				{
					LaunchFrame.NavigationService.Navigate(new AccountPage());
					lastCopy = new AccountPage();
				}
				else if (userLoginStatus.accountRole == "Модератор")
				{
					LaunchFrame.NavigationService.Navigate(new NewsPostPage());
					lastCopy = new NewsPostPage();
				}
			}
		}

		/// <summary>
		/// Запуск страницы с личным кабинетом
		/// </summary>
		private RelayCommand _openPersonalAccount { get; set; }
		public RelayCommand OpenPersonalAccount
		{
			get
			{
				return _openPersonalAccount ??
					(_openPersonalAccount = new RelayCommand((obj) =>
					{
						IsAccountIcon = false; // Скрываем кнопку с аккаунтом
						IsGoBack = true; // Отображаем кнопку возврата назад
						HamburgerMenuEvent.CloseHamburgerMenu(); // Закрываем "гамбургер" меню
						LaunchFrame.Navigate(new PersonalAccountAdmin());

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Возврат на предидущую страницу
		/// </summary>
		private RelayCommand _openPreviousPage { get; set; }
		public RelayCommand OpenPreviousPage
		{
			get
			{
				return _openPreviousPage ??
					(_openPreviousPage = new RelayCommand((obj) =>
					{
						IsAccountIcon = true; // Отображаем кнопку с аккаунтом
						IsGoBack = false; // Скрываем кнопку возврата назад
						HamburgerMenuEvent.CloseHamburgerMenu(); // Закрываем "гамбургер" меню

						// Открываем последнюю страницу перед переходом в ЛК
						LaunchFrame.Navigate(lastCopy);

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Переход на страницу для работы с учетными записями.
		/// </summary>
		public async void OpenPageAccount(object sender, EventAggregator e)
		{
			IsAccountIcon = true; // Отображаем кнопку с аккаунтом
			IsGoBack = false; // Скрываем кнопку возврата назад
			LaunchFrame.NavigationService.Navigate(new AccountPage());
			lastCopy = new AccountPage();
		}

		/// <summary>
		/// Переход на страницу для работы с рабочими отделами.
		/// </summary>
		public async void OpenPageWorkDepartment(object sender, EventAggregator e)
		{
			IsAccountIcon = true; // Отображаем кнопку с аккаунтом
			IsGoBack = false; // Скрываем кнопку возврата назад
			LaunchFrame.NavigationService.Navigate(new WorkDepartmentPage());
			lastCopy = new WorkDepartmentPage();
		}

		/// <summary>
		/// Переход на страницу для работы с категориями постов.
		/// </summary>
		public async void OpenPageNewsCategory(object sender, EventAggregator e)
		{
			IsAccountIcon = true; // Отображаем кнопку с аккаунтом
			IsGoBack = false; // Скрываем кнопку возврата назад
			LaunchFrame.NavigationService.Navigate(new NewsCategoryPage());
			lastCopy = new NewsCategoryPage();
		}

		/// <summary>
		/// Переход на страницу для работы с новостными постами.
		/// </summary>
		public async void OpenPageNewsPost(object sender, EventAggregator e)
		{
			LaunchFrame.NavigationService.Navigate(new NewsPostPage());
			lastCopy = new NewsPostPage();
		}

		/// <summary>
		/// Переход на страницу для работы с сообщениями пользователей.
		/// </summary>
		public async void OpenPageMessageUser(object sender, EventAggregator e)
		{
			LaunchFrame.NavigationService.Navigate(new MessageUserPage());
			lastCopy = new MessageUserPage();
		}

		#endregion

		#region WorkingWithMemory

		/// <summary>
		/// Последний экземпляр класса
		/// </summary>
		public object lastCopy;

		/// <summary>
		/// Очистка памяти
		/// </summary>
		/// <param name="element"> Экземпляр страницы. </param>
		public void ClearMemoryAfterFrame(FrameworkElement element)
		{
			if (element != null)
			{
				// очистка всех привязанных элементов
				BindingOperations.ClearAllBindings(element);
				// очистка визуальных элементов
				element.Resources.Clear();
				// Очистка ссылки на предыдущий экземпляр
				element = null;
			}

			if (LaunchFrame != null)
			{
				// очистка фрейма
				LaunchFrame.Content = null;
			}
		}

		#endregion

		#region WorkPopup

		/// <summary>
		///  Запуск popup
		/// </summary>
		private RelayCommand _openPopup { get; set; }
		public RelayCommand OpenPopup
		{
			get
			{
				return _openPopup ??
					(_openPopup = new RelayCommand((obj) =>
					{
						// Запуск popup
						StartPoupOfOutAccount = true;
						DarkBackground = Visibility.Visible; // показать фон

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Скрытие popup
		/// </summary>
		private RelayCommand _closePopup { get; set; }
		public RelayCommand ClosePopup
		{
			get
			{
				return _closePopup ??
					(_closePopup = new RelayCommand((obj) =>
					{
						// закрываем popup
						StartPoupOfOutAccount = false;
						DarkBackground = Visibility.Collapsed; // скрыть фон

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Выход из аккаунта
		/// </summary>
		private RelayCommand _logOutYourAccount { get; set; }
		public RelayCommand LogOutYourAccount
		{
			get
			{
				return _logOutYourAccount ??
					(_logOutYourAccount = new RelayCommand(async (obj) =>
					{
						// закрываем popup
						StartPoupOfOutAccount = false;
						DarkBackground = Visibility.Collapsed; // Скрыть фон
						await _authorizationService.LogOutYourAccount(); // Переход на страницу авторизации

					}, (obj) => true));
			}
		}

		#region Features

		/// <summary>
		/// Отображение Popup
		/// </summary>
		private bool _startPoupOfOutAccount { get; set; }
		public bool StartPoupOfOutAccount
		{
			get { return _startPoupOfOutAccount; }
			set
			{
				_startPoupOfOutAccount = value;
				OnPropertyChanged(nameof(StartPoupOfOutAccount));
			}
		}

		/// <summary>
		/// Фон для Popup
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
		/// Ассинхронно передаём фрейм из AdminHomePage
		/// </summary>
		/// <param name="openPage"></param>
		/// <returns></returns>
		public async Task InitializeAsync(Frame openPage)
		{
			if (openPage != null)
			{
				LaunchFrame = openPage;
				await StartingHomePage(); // Запуск начальной страницы
			}
		}

		/// <summary>
		/// Свойство для запуска страниц
		/// </summary>
		private Frame _launchFrame;
		public Frame LaunchFrame
		{
			get { return _launchFrame; }
			set
			{
				_launchFrame = value;
				OnPropertyChanged(nameof(LaunchFrame));
			}
		}

		/// <summary>
		/// Видимость кнопки для перехода в личный кабинет
		/// </summary>
		public bool _isAccountIcon { get; set; }
		public bool IsAccountIcon
		{
			get { return _isAccountIcon; }
			set
			{
				_isAccountIcon = value;
				OnPropertyChanged(nameof(IsAccountIcon));
			}
		}

		/// <summary>
		/// Видимость кнопки вернуться назад
		/// </summary>
		private bool _isGoBack { get; set; }
		public bool IsGoBack
		{
			get { return _isGoBack; }
			set
			{
				_isGoBack = value;
				OnPropertyChanged(nameof(IsGoBack));
			}
		}

		private string _userName { get; set; }
		public string UserName
		{
			get { return _userName; }
			set { _userName = value; OnPropertyChanged(nameof(UserName)); }
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
