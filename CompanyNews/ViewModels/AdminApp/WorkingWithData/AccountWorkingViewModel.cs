using CompanyNews.Helpers;
using CompanyNews.Helpers.Event;
using CompanyNews.Helpers.Validators;
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
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CompanyNews.ViewModels.AdminApp.WorkingWithData
{
	/// <summary>
	/// Класс для работы над аккаунтами. Добавление и редактирование данных.
	/// </summary>
	public class AccountWorkingViewModel : INotifyPropertyChanged
	{
		private readonly PasswordValidationRule _passwordValidationRule; // Валидация пароля

		/// <summary>
		/// Сервисы для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly AccountService _accountService; // Учетные записи
		private readonly WorkDepartmentService _workDepartmentService; // Рабочие отделы

		public AccountWorkingViewModel()
		{
			_accountService = ServiceLocator.GetService<AccountService>();
			_workDepartmentService = ServiceLocator.GetService<WorkDepartmentService>();


		}

		#region WorkingWithPages

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async Task InitialPageSetup(bool IsAddData, AccountExtended? accountExtended)
		{
			if(IsAddData) // Режим добавления данных
			{
				HeadingPage = "Создание учетной записи";


			}
            else
            {
				HeadingPage = "Изменение учетной записи";

				if(accountExtended != null)
				{
					SelectAccountExtended = accountExtended;

					// Заполняем все поля
					if (animationName != null && animationSurname != null &&
						animationPatronymic != null && animationNumberPhone != null &&
						animationRole != null && animationLogin != null && animationPassword != null &&
						animationRepeatPassword != null && animationWorkDepartment != null && animationReasonBlockingMessages != null)
					{
						animationName.Text = accountExtended.name;
						animationSurname.Text = accountExtended.surname;
						if(accountExtended.patronymic != null)
						{
							animationPatronymic.Text = accountExtended.patronymic;
						}
						animationNumberPhone.Text = accountExtended.phoneNumber;

						// Убираем доступ к полю с ролью
						animationRole.IsEnabled = false;
						animationRole.Text = accountExtended.accountRole;

						animationLogin.Text = accountExtended.login;

						// Рабочий отел
						SelectedWorkDepartment = await _workDepartmentService.GetWorkDepartmentByIdAsync(accountExtended.id);
						SelectedRole = accountExtended.accountRole; 

						if(accountExtended.reasonBlockingAccount != null) { animationReasonBlockingMessages.Text = accountExtended.reasonBlockingAccount; }
					}
				}
				else
				{
					// Возврат на страницу "Учетные записи"
					HamburgerMenuEvent.OpenPageAccount();
				}
			}

			// Роли
			ListRole = new List<string>() { "Модератор", "Пользователь" };
			// Рабочий отел
			ListWorkDepartments = new List<WorkDepartment>(await _workDepartmentService.GetAllWorkDepartmentsAsync());
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
						if (animationName != null && animationSurname != null &&
						animationPatronymic != null && animationNumberPhone != null &&
						animationRole != null && animationLogin != null && animationPassword != null &&
						animationRepeatPassword != null && animationWorkDepartment != null && animationReasonBlockingMessages != null)
						{
							// Проверка обязательных полей
							if (!string.IsNullOrWhiteSpace(animationName.Text) &&
							!string.IsNullOrWhiteSpace(animationSurname.Text) &&
							!string.IsNullOrWhiteSpace(animationNumberPhone.Text) &&
							!string.IsNullOrWhiteSpace(SelectedRole) &&
							SelectedWorkDepartment != null && !string.IsNullOrWhiteSpace(animationLogin.Text))
							{
								// Валидация телефона
								if(animationNumberPhone.Text.Length != 11)
								{
									StartFieldIllumination(animationNumberPhone); // Подсветка поля
									systemMessage.Text = "Номер телефона должен содержать 11 цифр.";
									systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
									// Исчезание сообщения
									BeginFadeAnimation(systemMessage);
									BeginFadeAnimation(systemMessageBorder);
								}
								else
								{
									if (!(double.TryParse(animationNumberPhone.Text.Trim(), out double number))) // если все числа корректны
									{
										StartFieldIllumination(animationNumberPhone); // Подсветка поля
										systemMessage.Text = "Номер телефона должен содержать только цифры.";
										systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
										// Исчезание сообщения
										BeginFadeAnimation(systemMessage);
										BeginFadeAnimation(systemMessageBorder);
									}
									else
									{
										if (!animationNumberPhone.Text.StartsWith("7"))
										{
											StartFieldIllumination(animationNumberPhone); // Подсветка поля
											systemMessage.Text = "Номер телефона должен начинаться с '7'.";
											systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
											// Исчезание сообщения
											BeginFadeAnimation(systemMessage);
											BeginFadeAnimation(systemMessageBorder);
										}
										else
										{
											// Проверка блокировки аккаунта
											if (await ReasonBlocking())
											{
												if (isAddData) // Добавление данных
												{

												}
												else // Редактирование данных
												{

												}
											}

										}
									}
								}
							}
							else
							{
								systemMessage.Text = "Заполните все обязательные поля.";
								systemMessageBorder.Visibility = System.Windows.Visibility.Visible;

								if (string.IsNullOrWhiteSpace(animationName.Text))
								{
									StartFieldIllumination(animationName); // Подсветка полей
								}

								if (string.IsNullOrWhiteSpace(animationSurname.Text))
								{
									StartFieldIllumination(animationSurname); // Подсветка полей
								}

								if (string.IsNullOrWhiteSpace(animationNumberPhone.Text))
								{
									StartFieldIllumination(animationNumberPhone); // Подсветка полей
								}

								if (string.IsNullOrWhiteSpace(SelectedRole))
								{
									StartFieldIllumination(animationRole); // Подсветка полей
								}

								if (string.IsNullOrWhiteSpace(animationLogin.Text))
								{
									StartFieldIllumination(animationLogin); // Подсветка полей
								}

								if (SelectedWorkDepartment == null)
								{
									StartFieldIllumination(animationWorkDepartment); // Подсветка полей
								}

								// Исчезание сообщения
								BeginFadeAnimation(systemMessage);
								BeginFadeAnimation(systemMessageBorder);
							}
							
						}
						
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Проверка причины блокировки
		/// </summary>
		public async Task<bool> ReasonBlocking()
		{
			if (IsBlockAccount == true)
			{
				if (animationReasonBlockingMessages.Text.Trim() == "")
				{
					StartFieldIllumination(animationReasonBlockingMessages); // Подсветка поля
					systemMessage.Text = "Укажите причину блокировки.";
					systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
					// Исчезание сообщения
					BeginFadeAnimation(systemMessage);
					BeginFadeAnimation(systemMessageBorder);
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Изменить пароль
		/// </summary>
		private RelayCommand? _changePassword { get; set; }
		public RelayCommand? ChangePassword
		{
			get
			{
				return _changePassword ??
					(_changePassword = new RelayCommand(async (obj) =>
					{
						if (await CheckPassword())
						{
							// Валидация пароля прошла успешно
							// Сохраняем пароль
							AccountExtended accountExtended = SelectAccountExtended;
							if (accountExtended != null)
							{
								accountExtended.password = PasswordHasher.HashPassword(animationPassword.Password.Trim());
								Account account = await _accountService.AccountExtendedConvert(accountExtended);
								if (account != null)
								{
									await _accountService.UpdateAccountAsync(account);
									systemMessage.Text = "Пароль успешно обновлен.";
									systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
									// Исчезание сообщения
									BeginFadeAnimation(systemMessage);
									BeginFadeAnimation(systemMessageBorder);
								}
							}
						}
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Валидация пароля
		/// </summary>
		private async Task<bool> CheckPassword()
		{
			// Проверка полей
			if (animationPassword != null && animationRepeatPassword != null)
			{
				if (animationPassword.Password.Trim() != "" && animationRepeatPassword.Password.Trim() != "")
				{
					if (animationPassword.Password == animationRepeatPassword.Password)
					{
						// Валидация пароля
						// Проверка на минимальную длину
						if (animationPassword.Password.Length < 8)
						{
							StartFieldIllumination(animationRepeatPassword); // Подсветка полей
							StartFieldIllumination(animationPassword);
							systemMessage.Text = "Пароль должен содержать не менее 8 символов.";
							systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
							// Исчезание сообщения
							BeginFadeAnimation(systemMessage);
							BeginFadeAnimation(systemMessageBorder);

							return false;
						}
						else
						{
							if (!Regex.IsMatch(animationPassword.Password, @"^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[!@#$%^&*()_\-+=\[\]{};':""\\|,.<>\/?]).+$"))
							{
								StartFieldIllumination(animationRepeatPassword); // Подсветка полей
								StartFieldIllumination(animationPassword);
								systemMessage.Text = "Пароль должен содержать цифры, латинские буквы и специальные символы.";
								systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
								// Исчезание сообщения
								BeginFadeAnimation(systemMessage);
								BeginFadeAnimation(systemMessageBorder);

								return false;
							}
							else
							{
								return true;
							}
						}
					}
					else
					{
						StartFieldIllumination(animationRepeatPassword); // Подсветка полей
						StartFieldIllumination(animationPassword);
						systemMessage.Text = $"Пароли не совпадают.";
						systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
						// Исчезание сообщения
						BeginFadeAnimation(systemMessage);
						BeginFadeAnimation(systemMessageBorder);

						return false;
					}
				}
				else
				{
					if (animationPassword.Password.Trim() == "")
					{
						StartFieldIllumination(animationPassword);
					}

					if (animationRepeatPassword.Password.Trim() == "")
					{
						StartFieldIllumination(animationRepeatPassword);
					}

					systemMessage.Text = $"Заполните все поля.";
					systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
					// Исчезание сообщения
					BeginFadeAnimation(systemMessage);
					BeginFadeAnimation(systemMessageBorder);

					return false;
				}
			}
			return false;
		}

		/// <summary>
		/// Возврат на предыдущую страницу.
		/// </summary>
		private RelayCommand? _refund { get; set; }
		public RelayCommand? Refund
		{
			get
			{
				return _refund ??
					(_refund = new RelayCommand(async (obj) =>
					{
						// Переход на страницу "Учетные записи"
						HamburgerMenuEvent.OpenPageAccount();
					}, (obj) => true));
			}
		}

		#endregion

		#region Features

		/// <summary>
		/// Асинхронно получаем информацию из привязанного View
		/// </summary>
		public async Task InitializeAsync
			(AdminViewModelParameters adminViewModelParameters, TextBox name,
			TextBox Surname, TextBox Patronymic, TextBox NumberPhone, ComboBox Role, TextBox Login,
			PasswordBox Password, PasswordBox RepeatPassword, ComboBox WorkDepartment,
			TextBox ReasonBlockingMessages)
		{
			systemMessage = adminViewModelParameters.errorInputText;
			systemMessageBorder = adminViewModelParameters.errorInputBorder;
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			animationName = name;
			animationSurname = Surname;
			animationPatronymic = Patronymic;
			animationNumberPhone = NumberPhone;
			animationRole = Role;
			animationLogin = Login;
			animationPassword = Password;
			animationRepeatPassword = RepeatPassword;
			animationWorkDepartment = WorkDepartment;
			animationReasonBlockingMessages = ReasonBlockingMessages;
		}

		/// <summary>
		/// Выбранная роль
		/// </summary>
		private string _selectedRole { get; set; }
		public string SelectedRole
		{
			get { return _selectedRole; }
			set { _selectedRole = value; OnPropertyChanged(nameof(SelectedRole)); }
		}

		/// <summary>
		/// Список ролей
		/// </summary>
		private List<string>? _listRole { get; set; }
		public List<string>? ListRole
		{
			get { return _listRole; }
			set
			{
				_listRole = value;
				OnPropertyChanged(nameof(ListRole));
			}
		}

		/// <summary>
		///  Переданный аккаунт для изменения
		/// </summary>
		private AccountExtended SelectAccountExtended {  get; set; }

		/// <summary>
		/// Выбранный рабочий отдел
		/// </summary>
		private WorkDepartment _selectedWorkDepartment { get; set; }
		public WorkDepartment SelectedWorkDepartment
		{
			get { return _selectedWorkDepartment; }
			set { _selectedWorkDepartment = value; OnPropertyChanged(nameof(SelectedWorkDepartment)); }
		}

		/// <summary>
		/// Список рабочих отделов
		/// </summary>
		private List<WorkDepartment>? _listWorkDepartments { get; set; }
		public List<WorkDepartment>? ListWorkDepartments
		{
			get { return _listWorkDepartments; }
			set
			{
				_listWorkDepartments = value;
				OnPropertyChanged(nameof(ListWorkDepartments));
			}
		}

		/// <summary>
		/// Имя
		/// </summary>
		public TextBox? animationName { get; set; }

		//public string Nam

		/// <summary>
		/// Фамилия
		/// </summary>
		public TextBox? animationSurname { get; set; } 

		/// <summary>
		/// Отчество
		/// </summary>
		public TextBox? animationPatronymic { get; set; } 

		/// <summary>
		/// Номер телефона
		/// </summary>
		public TextBox? animationNumberPhone { get; set; } 

		/// <summary>
		/// Роль
		/// </summary>
		public ComboBox? animationRole { get; set; }

		/// <summary>
		/// Логин
		/// </summary>
		public TextBox? animationLogin { get; set; }

		/// <summary>
		/// Пароль
		/// </summary>
		public PasswordBox? animationPassword { get; set; }

		/// <summary>
		/// Повтор пароля
		/// </summary>
		public PasswordBox? animationRepeatPassword { get; set; }

		/// <summary>
		/// Рабочий отдел
		/// </summary>
		public ComboBox? animationWorkDepartment { get; set; }

		/// <summary>
		/// Описание блокирвки
		/// </summary>
		public TextBox? animationReasonBlockingMessages { get; set; }

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
		/// Добавление или редактирование данных
		/// </summary>
		/// <remarks>true - добавление данных; false - редактирование данных</remarks>
		private bool isAddData { get; set; }

		/// <summary>
		/// Заголовок страницы
		/// </summary>
		private string? _headingPage { get; set; }
		public string? HeadingPage
		{
			get { return _headingPage; }
			set { _headingPage = value; OnPropertyChanged(nameof(HeadingPage)); }
		}

		/// <summary>
		/// CheckBox для блокироваки аккаунта в UI. 
		/// </summary>
		private bool _isBlockAccount { get; set; }
		public bool IsBlockAccount
		{
			get { return _isBlockAccount; }
			set
			{
				_isBlockAccount = value; 
				OnPropertyChanged(nameof(IsBlockAccount));
				if (IsBlockAccount)
				{
					IsReasonBlockingMessages = true;
				}
				else
				{
					IsReasonBlockingMessages = false;
				}
			}
		}

		/// <summary>
		/// Отображение или скрытие поля для ввода причины блокироваки аккаунта в UI. 
		/// </summary>
		private bool _isReasonBlockingMessages { get; set; }
		public bool IsReasonBlockingMessages
		{
			get { return _isReasonBlockingMessages; }
			set { _isReasonBlockingMessages = value; OnPropertyChanged(nameof(IsReasonBlockingMessages)); }
		}

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

		private void StartFieldIllumination(ComboBox comboBox)
		{
			fieldIllumination.Begin(comboBox);
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
