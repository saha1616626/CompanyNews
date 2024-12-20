using CompanyNews.Helpers;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using CompanyNews.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CompanyNews.ViewModels.AdminApp
{
    public class PersonalAccountAdminViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly AuthorizationService _authorizationService;
		private readonly AccountService _accountService;

		public PersonalAccountAdminViewModel()
		{
			_authorizationService = ServiceLocator.GetService<AuthorizationService>();
			_accountService = ServiceLocator.GetService<AccountService>();	
			SettingUpPage(); // Первоначальная настройка страницы
		}

		#region WorkingWithPages

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async void SettingUpPage()
		{
			DarkBackground = Visibility.Collapsed; // Фон для Popup скрыт

			// Получаем аккаунт
			Account account = await _authorizationService.GetUserAccount();

			try
			{
				// Установка изображения профиля
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

					// Смена парооля в зависимости от роли. 
					if(account.accountRole == "Модератор" || account.accountRole == "Пользователь")
					{
						IsEditPassword = true;
					}
					else
					{
						IsEditPassword = false;
					}
				}
			}
			catch(Exception ex) { } // Если не удалось установить изображение

			// Установка информации о пользователе
			if(account != null)
			{
				AccountExtended? accountExtended = await _accountService.AcountConvert(account);
				if(accountExtended != null)
				{
					UserName = accountExtended.name + " " + accountExtended.surname;
					if (accountExtended.patronymic != null) { UserName += " " + accountExtended.patronymic; }
					UserPhoneNumber = await PhoneNumberMask(accountExtended.phoneNumber);
					UserWorkDepartment = accountExtended.workDepartmentName;
					if(accountExtended.profileDescription != null) { UserProfileDescription = accountExtended.profileDescription; }
				}

				// Проверка ограничений
				if (account.isCanLeaveComments)
				{
					IsUserLimitations = true;
					UserLimitations = account.reasonBlockingMessages;
				}
			}
		}

		/// <summary>
		/// Маска номера телефона
		/// </summary>
		public async Task<string> PhoneNumberMask(string phoneNumber)
		{
			if (string.IsNullOrEmpty(phoneNumber))
				return string.Empty;

			// Маска для формата +7 (XXX) XXX-XX-XX
			string formattedNumber = "+7 (";

			if (phoneNumber.Length > 1)
			{
				formattedNumber += phoneNumber.Substring(1, 3) + ") ";
			}
			else
			{
				return formattedNumber; // Возвращаем только +7 (
			}

			if (phoneNumber.Length > 4)
			{
				formattedNumber += phoneNumber.Substring(4, 3) + "-";
			}
			else
			{
				return formattedNumber; // Возвращаем +7 (XXX)
			}

			if (phoneNumber.Length > 6)
			{
				formattedNumber += phoneNumber.Substring(7, 2) + "-";
			}
			else
			{
				return formattedNumber; // Возвращаем +7 (XXX) XXX
			}

			if (phoneNumber.Length > 8)
			{
				formattedNumber += phoneNumber.Substring(9, 2);
			}

			return formattedNumber;
		}

		/// <summary>
		///	Смена пароля
		/// </summary>
		private RelayCommand _editPassword { get; set; }
		public RelayCommand EditPassword
		{
			get
			{
				return _editPassword ??
					(_editPassword = new RelayCommand(async (obj) =>
					{
						// Проверка полей
						if(oldPassword != null && newPassword != null && repeatNewPassword != null)
						{
							if(!string.IsNullOrWhiteSpace(oldPassword.Password) && !string.IsNullOrWhiteSpace(newPassword.Password)
							&& !string.IsNullOrWhiteSpace(repeatNewPassword.Password))
							{
								// Проверка старого пароля
								Account account = await _authorizationService.GetUserAccount();
								if(account != null)
								{
									if(PasswordHasher.VerifyPassword(oldPassword.Password.Trim(), account.password))
									{
										// Проверка нового пароля
										if(newPassword.Password.Trim() == repeatNewPassword.Password.Trim())
										{
											// Проверка сложности пароля
											// Валидация пароля
											// Проверка на минимальную длину
											if (newPassword.Password.Length < 8)
											{
												StartFieldIllumination(newPassword); // Подсветка полей
												StartFieldIllumination(repeatNewPassword);
												systemMessage.Text = "Пароль должен содержать не менее 8 символов.";
												systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
												// Исчезание сообщения
												BeginFadeAnimation(systemMessage);
												BeginFadeAnimation(systemMessageBorder);
											}
											else
											{
												if (!Regex.IsMatch(newPassword.Password, @"^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[!@#$%^&*()_\-+=\[\]{};':""\\|,.<>\/?]).+$"))
												{
													StartFieldIllumination(newPassword); // Подсветка полей
													StartFieldIllumination(repeatNewPassword);
													systemMessage.Text = "Пароль должен содержать цифры, латинские буквы и специальные символы.";
													systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
													// Исчезание сообщения
													BeginFadeAnimation(systemMessage);
													BeginFadeAnimation(systemMessageBorder);

												}
												else
												{
													// Обновляем данные
													account.password = PasswordHasher.HashPassword(newPassword.Password.Trim());
													await _accountService.UpdateAccountAsync(account);

													systemMessage.Text = "Пароли успешнно сменен.";
													systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
													BeginFadeAnimation(systemMessage); // Исчезание сообщения
													BeginFadeAnimation(systemMessageBorder);
												}
											}

										}
										else
										{
											StartFieldIllumination(newPassword); // Подсветка поля
											StartFieldIllumination(repeatNewPassword); // Подсветка поля

											systemMessage.Text = "Пароли не совпадают.";
											systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
											BeginFadeAnimation(systemMessage); // Исчезание сообщения
											BeginFadeAnimation(systemMessageBorder);
										}
									}
									else
									{
										StartFieldIllumination(oldPassword); // Подсветка поля

										systemMessage.Text = "Старый пароль не верный.";
										systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
										BeginFadeAnimation(systemMessage); // Исчезание сообщения
										BeginFadeAnimation(systemMessageBorder);
									}
								}
							}
							else
							{
								// Сообщение об завершении операции
								systemMessage.Text = "Заполните обязательные поля.";
								systemMessageBorder.Visibility = System.Windows.Visibility.Visible;

								if (string.IsNullOrWhiteSpace(oldPassword.Password))
								{
									StartFieldIllumination(oldPassword); // Подсветка поля
								}

								if (string.IsNullOrWhiteSpace(newPassword.Password))
								{
									StartFieldIllumination(newPassword); // Подсветка поля
								}

								if (string.IsNullOrWhiteSpace(repeatNewPassword.Password))
								{
									StartFieldIllumination(repeatNewPassword); // Подсветка поля
								}

								BeginFadeAnimation(systemMessage); // Исчезание сообщения
								BeginFadeAnimation(systemMessageBorder);
							}
						}

					}, (obj) => true));
			}
		}

		#endregion

		#region WorkPopup

		/// <summary>
		///  Запуск popup для изменения изображения изображения
		/// </summary>
		private RelayCommand _openPopupChangeImage { get; set; }
		public RelayCommand OpenPopupChangeImage
		{
			get
			{
				return _openPopupChangeImage ??
					(_openPopupChangeImage = new RelayCommand(async (obj) =>
					{
						// Выбираем изображение
						CroppedBitmap? croppedBitmap = WorkingWithImage.AddImageFromDialogBox(200);
						if(croppedBitmap != null)
						{
							IsProfileDescriptionEditing = false;
							IsImageEditing = true;

							// Запуск popup
							StartPoupOfOutAccount = true;
							DarkBackground = Visibility.Visible; // показать фон

							ProfilePicturePopup = croppedBitmap; // Выводим изображение
						}

						if (croppedBitmap == null)
						{
							// Сообщение об завершении операции
							systemMessage.Text = "Операция отменена.";
							systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
							IsOperationSuccessful = false; // Сообщаем, что операция не прошла
							// Исчезание сообщения
							BeginFadeAnimation(systemMessage);
							BeginFadeAnimation(systemMessageBorder);
						}

					}, (obj) => true));
			}
		}



		/// <summary>
		///  Запуск popup для изменения информации о себе
		/// </summary>
		private RelayCommand _openPopupChangeInfo { get; set; }
		public RelayCommand OpenPopupChangeInfo
		{
			get
			{
				return _openPopupChangeInfo ??
					(_openPopupChangeInfo = new RelayCommand(async (obj) =>
					{
						IsProfileDescriptionEditing = true;
						IsImageEditing = false;

						// Запуск popup
						StartPoupOfOutAccount = true;
						DarkBackground = Visibility.Visible; // показать фон

						// Получаем аккаунт
						Account account = await _authorizationService.GetUserAccount();
						if (account != null)
						{
							if (account.profileDescription != null) { UserProfileDescriptionPopup = account.profileDescription; }
						}

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

						// Сообщение об завершении операции
						systemMessage.Text = "Операция отменена.";
						systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
						IsOperationSuccessful = false; // Сообщаем, что операция не прошла
						// Исчезание сообщения
						BeginFadeAnimation(systemMessage);
						BeginFadeAnimation(systemMessageBorder);

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Сохранение данных
		/// </summary>
		private RelayCommand _saveData { get; set; }
		public RelayCommand SaveData
		{
			get
			{
				return _saveData ??
					(_saveData = new RelayCommand(async (obj) =>
					{
						// Закрываем popup
						StartPoupOfOutAccount = false;
						DarkBackground = Visibility.Collapsed; // Скрыть фон

						// Получаем аккаунт
						Account account = await _authorizationService.GetUserAccount();
						if (account != null)
						{
							// Если редактирование изображения
							if (IsImageEditing)
							{
								if(ProfilePicturePopup != null)
								{
									// Сохраняем изображение в БД
									account.image = WorkingWithImage.ConvertingImageForWritingDatabase(ProfilePicturePopup);
									await _accountService.UpdateAccountAsync(account);

									// Сообщение об завершении операции
									systemMessage.Text = "Фото профиля успешно обновлено.";
									systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
									IsOperationSuccessful = true; // Сообщаем, что операция успешна
								}
							}
							else // Если изменение описания
							{
								// Сохраняем информацию в БД
								if (UserProfileDescriptionPopup == null || UserProfileDescriptionPopup.Trim() == "")
								{
									account.profileDescription = null;
								}
								account.profileDescription = UserProfileDescriptionPopup;
								await _accountService.UpdateAccountAsync(account);

								// Сообщение об завершении операции
								systemMessage.Text = "Описание профиля успешно обновлено.";
								systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
								IsOperationSuccessful = true; // Сообщаем, что операция успешна
							}

							// Исчезание сообщения
							BeginFadeAnimation(systemMessage); 
							BeginFadeAnimation(systemMessageBorder);

							// Обновление данных
							SettingUpPage();
						}

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

		/// <summary>
		/// Информация о себе пользователя в Popup
		/// </summary>
		private string _userProfileDescriptionPopup { get; set; }
		public string UserProfileDescriptionPopup
		{
			get { return _userProfileDescriptionPopup; }
			set
			{
				_userProfileDescriptionPopup = value;
				OnPropertyChanged(nameof(UserProfileDescriptionPopup));
			}
		}

		/// <summary>
		/// Изображение профиля в Popup
		/// </summary>
		private CroppedBitmap _profilePicturePopup { get; set; }
		public CroppedBitmap ProfilePicturePopup
		{
			get { return _profilePicturePopup; }
			set
			{
				_profilePicturePopup = value;
				OnPropertyChanged(nameof(ProfilePicturePopup));
			}
		}

		/// <summary>
		/// Редактирование изображения
		/// </summary>
		private bool _isImageEditing { get; set; }
		public bool IsImageEditing
		{
			get { return _isImageEditing; }
			set
			{
				_isImageEditing = value;
				OnPropertyChanged(nameof(IsImageEditing));
			}
		}

		/// <summary>
		/// Редактирование информации о себе
		/// </summary>
		private bool _isProfileDescriptionEditing { get; set; }
		public bool IsProfileDescriptionEditing
		{
			get { return _isProfileDescriptionEditing; }
			set
			{
				_isProfileDescriptionEditing = value;
				OnPropertyChanged(nameof(IsProfileDescriptionEditing));
			}
		}

		public bool IsOperationSuccessful { get; set; }

		#endregion

		#endregion

		#region Features

		/// <summary>
		/// Асинхронно получаем информацию из привязанного View
		/// </summary>
		public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters, PasswordBox OldPassword, PasswordBox NewPassword,
			PasswordBox RepeatNewPassword)
		{
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			systemMessage = adminViewModelParameters.errorInputText;
			systemMessageBorder = adminViewModelParameters.errorInputBorder;
			oldPassword = OldPassword;
			newPassword = NewPassword;
			repeatNewPassword = RepeatNewPassword;
		}

		/// <summary>
		/// Видимость функции смены пароля
		/// </summary>
		private bool _isEditPassword {  get; set; }
		public bool IsEditPassword
		{
			get { return _isEditPassword; }
			set { _isEditPassword = value; OnPropertyChanged(nameof(IsEditPassword)); }
		}

		/// <summary>
		/// Повтор нового пароля
		/// </summary>
		PasswordBox? repeatNewPassword {  get; set; }

		/// <summary>
		/// Поле нового пароля
		/// </summary>
		PasswordBox? newPassword {  get; set; }

		/// <summary>
		/// Поле старого пароля
		/// </summary>
		PasswordBox? oldPassword {  get; set; }

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
		/// ИФО пользователя
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
		/// Информация о себе пользователя
		/// </summary>
		private string _userProfileDescription { get; set; }
		public string UserProfileDescription
		{
			get { return _userProfileDescription; }
			set
			{
				_userProfileDescription = value;
				OnPropertyChanged(nameof(UserProfileDescription));
			}
		}

		/// <summary>
		/// Должность пользователя
		/// </summary>
		private string _userWorkDepartment { get; set; }
		public string UserWorkDepartment
		{
			get { return _userWorkDepartment; }
			set
			{
				_userWorkDepartment = value;
				OnPropertyChanged(nameof(UserWorkDepartment));
			}
		}

		/// <summary>
		/// Пользователь ограничен в каких либо действиях на платформе?
		/// </summary>
		private bool _IsUserLimitations { get; set; }
		public bool IsUserLimitations
		{
			get { return _IsUserLimitations; }
			set
			{
				_IsUserLimitations = value;
				OnPropertyChanged(nameof(IsUserLimitations));
			}
		}

		/// <summary>
		/// Описание причины ограничения пользователя
		/// </summary>
		private string? _userLimitations { get; set; }
		public string? UserLimitations
		{
			get { return _userLimitations; }
			set
			{
				_userLimitations = value;
				OnPropertyChanged(nameof(UserLimitations));
			}
		}

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
