using CompanyNews.Helpers;
using CompanyNews.Models;
using CompanyNews.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CompanyNews.ViewModels.AdminApp
{
    public class PersonalAccountAdminViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly AuthorizationService _authorizationService;

		public PersonalAccountAdminViewModel()
		{
			_authorizationService = ServiceLocator.GetService<AuthorizationService>();
			SettingUpPage(); // Первоначальная настройка страницы
		}

		#region WorkingWithPages

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async void SettingUpPage()
		{
			try
			{
				// Установка изображения профиля
				// Получаем аккаунт
				Account account = await _authorizationService.GetUserAccount();
				if(account != null)
				{
					if(account.image != null)
					{
						ProfilePicture = WorkingWithImage.ResizingPhotos(account.image, 200);
					}
					else
					{
						ProfilePicture = WorkingWithImage.ConvertImageCroppedBitmap(
						   new BitmapImage(new Uri("../../../Resources/Pictures/user.png", UriKind.Relative)), 200);
					}
				}
			}
			catch(Exception ex) { } // Если не удалось установить изображение

		}

		#endregion

		#region Features

		/// <summary>
		/// Изображение профиля
		/// </summary>
		private CroppedBitmap _profilePicture { get; set; }
		public CroppedBitmap ProfilePicture
		{
			get { return _profilePicture; }
			set
			{
				_profilePicture = value;
				OnPropertyChanged(nameof(ProfilePicture));
			}
		}

		/// <summary>
		/// Имя пользователя
		/// </summary>
		private string _userName {  get; set; }
		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				OnPropertyChanged(nameof(UserName));
			}
		}

		/// <summary>
		/// Фамилия пользователя
		/// </summary>
		private string _userSurname { get; set; }
		public string UserSurname
		{
			get { return _userSurname; }
			set
			{
				_userSurname = value;
				OnPropertyChanged(nameof(UserSurname));
			}
		}

		/// <summary>
		/// Отчество пользователя
		/// </summary>
		private string _userPatronymic { get; set; }
		public string UserPatronymic
		{
			get { return _userPatronymic; }
			set
			{
				_userPatronymic = value;
				OnPropertyChanged(nameof(UserPatronymic));
			}
		}

		/// <summary>
		/// Номер телефона пользователя
		/// </summary>
		private string _userPhoneNumber { get; set; }
		public string UserPhoneNumber
		{
			get { return _userPhoneNumber; }
			set
			{
				_userPhoneNumber = value;
				OnPropertyChanged(nameof(UserPhoneNumber));
			}
		}

		/// <summary>
		/// Пользователь ограничен в каких либо действиях на платформе?
		/// </summary>
		private string _IsUserLimitations { get; set; }
		public string IsUserLimitations
		{
			get { return _IsUserLimitations; }
			set
			{
				_IsUserLimitations = value;
				OnPropertyChanged(nameof(IsUserLimitations));
			}
		}

		/// <summary>
		/// Описание ограничения пользователя
		/// </summary>
		private string _userLimitations { get; set; }
		public string UserLimitations
		{
			get { return _userLimitations; }
			set
			{
				_userLimitations = value;
				OnPropertyChanged(nameof(UserLimitations));
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
