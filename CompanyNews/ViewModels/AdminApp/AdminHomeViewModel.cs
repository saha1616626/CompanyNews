using CompanyNews.Helpers;
using CompanyNews.Helpers.Event;
using CompanyNews.Services;
using CompanyNews.Views.AdminApp;
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
		}

		#region WorkingWithPages

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public void SettingUpPage()
		{
			DarkBackground = Visibility.Collapsed; // Фон для Popup скрыт
			IsAccountIcon = true; // Кнопка перехода в ЛК отображается
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
						LaunchFrame.Navigate(personalAccountAdmin = new PersonalAccountAdmin());
						
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
						ClearMemoryAfterFrame(personalAccountAdmin); // Очищаем память от страницы авторизации
						HamburgerMenuEvent.CloseHamburgerMenu(); // Закрываем "гамбургер" меню

						// Открываем последнюю страницу перед переходом в ЛК
						LaunchFrame.Navigate(lastCopy);

					}, (obj) => true));
			}
		}

		// при переходе по меню нужно очищать фреймы старые для этого есть lastCopy. Поэтому, нужно предварительно туда записывать данные

		#endregion

		#region WorkingWithMemory

		/// <summary>
		/// Последний экземпляр класса
		/// </summary>
		public object lastCopy;

		/// <summary>
		/// Личный кабинет администратора или редактр
		/// </summary>
		PersonalAccountAdmin personalAccountAdmin { get; set; }

		/// <summary>
		/// Работа с учетными записями
		/// </summary>
		AccountPage accountPage { get; set; } 

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
					(_logOutYourAccount = new RelayCommand(async(obj) =>
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
				LaunchFrame.NavigationService.Navigate(accountPage = new AccountPage());
				lastCopy = accountPage;
			}
		}

		/// <summary>
		/// Видимость кнопки для перехода в личный кабинет
		/// </summary>
		public bool _isAccountIcon {  get; set; }
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
		private bool _isGoBack {  get; set; }
		public bool IsGoBack
		{
			get { return _isGoBack; }
			set
			{
				_isGoBack = value;
				OnPropertyChanged(nameof(IsGoBack));	
			}
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
