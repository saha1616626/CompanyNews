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

		#region workHamburgerMenu

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

		#endregion

		#region Features

		/// <summary>
		/// Свойство видимости кнопки "пользователи"
		/// </summary>
		private bool _isUserSettings { get; set; }
		public bool IsUserSettings
		{
			get { return _isUserSettings; }
			set { _isUserSettings = value; OnPropertyChanged(nameof(IsUserSettings)); }
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
					// Отображаем пункт меню с пользователями
					IsUserSettings = true;
				}
				else if (userLoginStatus.accountRole == "Редактор")
				{
					// Скрываем пункт меню с пользователями
					IsUserSettings = false;
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
