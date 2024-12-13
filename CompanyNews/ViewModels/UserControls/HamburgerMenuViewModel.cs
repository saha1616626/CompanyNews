using CompanyNews.Helpers;
using CompanyNews.Helpers.Event;
using CompanyNews.Models.Authorization;
using CompanyNews.Services;
using CompanyNews.ViewModels.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CompanyNews.ViewModels.UserControls
{
    public class HamburgerMenuViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой работы с учетной записью
		/// </summary>
		private readonly AuthorizationService _authorizationService;

		public HamburgerMenuViewModel()
		{
			_authorizationService = ServiceLocator.GetService<AuthorizationService>();

			// Подписываемся на событие закрытия "гамбургер" меню
			HamburgerMenuEvent.closeHamburgerMenu += CloseHamburgerMenu;

			IsMenuButtonVisibility = true; // Видимость кнопки работы с меню
										   
			LimitingMenuDependingRole(); // Ограничения меню в зависимости от роли
		}

		#region WorkHamburgerMenu

		/// <summary>
		/// Запуск меню или закрытие
		/// </summary>
		private RelayCommand _hamburgerButton { get; set; }
		public RelayCommand HamburgerButton
		{
			get
			{
				return _hamburgerButton ??
					(_hamburgerButton = new RelayCommand((obj) =>
					{
						OpenHamburgerMenu();
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Работа меню (запуск и остановка)
		/// </summary>
		private void OpenHamburgerMenu()
		{
			IsSideMenuVisible = !IsSideMenuVisible; // при каждом вызове меняем видимость
			SideMenuWidth = IsSideMenuVisible ? 200 : 0; // изменяем ширину
			IsMenuButtonVisibility = IsSideMenuVisible ? false : true; // скрываем кнопку или показываем
		}

		/// <summary>
		/// Закрываем меню
		/// </summary>
		private void CloseHamburgerMenu(object sender, EventAggregator e)
		{
			IsSideMenuVisible = false; // невидимое меню
			SideMenuWidth = 0; // изменяем ширину
			IsMenuButtonVisibility = IsSideMenuVisible ? false : true; // скрываем кнопку или показываем
		}

		/// <summary>
		/// Переход на страницу "Учетные записи"
		/// </summary>
		private RelayCommand _openPageAccount { get; set; }
		public RelayCommand OpenPageAccount
		{
			get
			{
				return _openPageAccount ??
					(_openPageAccount = new RelayCommand((obj) =>
					{
						HamburgerMenuEvent.OpenPageAccount();
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Переход на страницу "Рабочие отделы"
		/// </summary>
		private RelayCommand _openPageWorkDepartment { get; set; }
		public RelayCommand OpenPageWorkDepartment
		{
			get
			{
				return _openPageWorkDepartment ??
					(_openPageWorkDepartment = new RelayCommand((obj) =>
					{
						HamburgerMenuEvent.OpenPageWorkDepartment();
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Переход на страницу "Категории постов"
		/// </summary>
		private RelayCommand _openPageNewsCategory { get; set; }
		public RelayCommand OpenPageNewsCategory
		{
			get
			{
				return _openPageNewsCategory ??
					(_openPageNewsCategory = new RelayCommand((obj) =>
					{
						HamburgerMenuEvent.OpenPageNewsCategory();
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Переход на страницу "Новостные посты"
		/// </summary>
		private RelayCommand _openPageNewsPost { get; set; }
		public RelayCommand OpenPageNewsPost
		{
			get
			{
				return _openPageNewsPost ??
					(_openPageNewsPost = new RelayCommand((obj) =>
					{
						HamburgerMenuEvent.OpenPageNewsPost();
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Переход на страницу "Сообщения пользователей"
		/// </summary>
		private RelayCommand _openPageMessageUsers { get; set; }
		public RelayCommand OpenPageMessageUsers
		{
			get
			{
				return _openPageMessageUsers ??
					(_openPageMessageUsers = new RelayCommand((obj) =>
					{
						HamburgerMenuEvent.OpenPageMessageUser();
					}, (obj) => true));
			}
		}

		#endregion

		#region Features

		/// <summary>
		/// Свойство видимости кнопок для Администратора
		/// </summary>
		private bool _isAdministrativeMenuEnabled { get; set; }
		public bool IsAdministrativeMenuEnabled
		{
			get { return _isAdministrativeMenuEnabled; }
			set { _isAdministrativeMenuEnabled = value; OnPropertyChanged(nameof(IsAdministrativeMenuEnabled)); }
		}

		/// <summary>
		/// Свойство видимости кнопок для Модератора
		/// </summary>
		private bool _isModerationMenuEnabled { get; set; }
		public bool IsModerationMenuEnabled
		{
			get { return _isModerationMenuEnabled; }
			set { _isModerationMenuEnabled = value; OnPropertyChanged(nameof(IsModerationMenuEnabled)); }
		}

		/// <summary>
		/// Свойство отвечающие за ширину "гамбургер меню"
		/// </summary>
		private double _sideMenuWidth { get; set; } // ширина меню
		public double SideMenuWidth
		{
			get { return _sideMenuWidth; }
			set { _sideMenuWidth = value; OnPropertyChanged(nameof(SideMenuWidth)); }
		}

		/// <summary>
		/// Свойство отвечающие за видимость "гамбургер меню"
		/// </summary>
		private bool _isSideMenuVisible { get; set; } // видимость меню
		public bool IsSideMenuVisible
		{
			get { return _isSideMenuVisible; }
			set { _isSideMenuVisible = value; OnPropertyChanged(nameof(IsSideMenuVisible)); }
		}

		/// <summary>
		/// Свойство отвечающие за видимость кнопки запуска "гамбургер меню"
		/// </summary>
		private bool _isMenuButtonVisibility { get; set; } // видимость кнопки запуска меню
		public bool IsMenuButtonVisibility
		{
			get { return _isMenuButtonVisibility; }
			set { _isMenuButtonVisibility = value; OnPropertyChanged(nameof(IsMenuButtonVisibility)); }
		}

		#endregion

		#region SettingMenu

		/// <summary>
		/// Ограничение меню в зависимости от роли
		/// </summary>
		/// <returns></returns>
		private async Task LimitingMenuDependingRole()
		{
			// Получаем роль
			UserLoginStatus userLoginStatus = await _authorizationService.GetUserStatusInSystem();
			if(userLoginStatus != null)
			{
				if (userLoginStatus.accountRole == "Администратор")
				{
					// Отображаем пункты для Администратора и скрываем для Модератора
					IsAdministrativeMenuEnabled = true;
					IsModerationMenuEnabled = false;
				}
				else if (userLoginStatus.accountRole == "Модератор")
				{
					// Отображаем пункты для Модератора и скрываем для Администратора
					IsModerationMenuEnabled = true;
					IsAdministrativeMenuEnabled = false;
				}
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
