using CompanyNews.Helpers;
using CompanyNews.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
			DarkBackground = Visibility.Collapsed; // Фон для Popup скрыт
		}

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



		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
