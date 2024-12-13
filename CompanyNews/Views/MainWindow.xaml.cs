using CompanyNews.Helpers.Event;
using CompanyNews.Models.Authorization;
using CompanyNews.Services;
using CompanyNews.Views.AdminApp;
using CompanyNews.Views.Authorization;
using CompanyNews.Views.ClientApp;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CompanyNews.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой авторизации
		/// </summary>
		private readonly AuthorizationService _authorizationService;

		AuthorizationPage authorizationPage {  get; set; } // Страница для авторизации
		AdminHomePage adminHomePage { get; set; } // Административная часть
		ClientHomePage clientHomePage { get; set; } // Пользовательская часть

		public MainWindow()
		{
			InitializeComponent();
			
			_authorizationService = ServiceLocator.GetService<AuthorizationService>();
			// Запускаем асинхронный метод управления сессией
			SessionManagementAsync(); // Вход в систему

			// Подписываемся на событие перехода на нужную страница после успешной авторизации
			AuthorizationEvent.logInYourAccount += LogInYourAccount;
			// Подписываемся на событие перехода на страницу авторизации после успешного выхода
			AuthorizationEvent.logOutYourAccount += LogOutYourAccount;
		}

		/// <summary>
		///  Автоматизированный вход в систему.
		/// </summary>
		public async Task SessionManagementAsync()
		{
			// очистка памяти
			ClearMemoryAfterFrame(authorizationPage);
			ClearMemoryAfterFrame(adminHomePage);
			ClearMemoryAfterFrame(clientHomePage);
			await Task.Delay(500);

			// Проверка состояния пользователя в системе
			UserLoginStatus userLoginStatus = await _authorizationService.GetUserStatusInSystem();
			if(userLoginStatus == null || !userLoginStatus.isUserLoggedIn)
			{
				// Если пользователь не авторизован, то направляем его на страницу авторизации
				mainFrame.Navigate(authorizationPage = new AuthorizationPage());
			}

			if (userLoginStatus.accountRole != null)
			{
				// Вход в учетную запись
				if (userLoginStatus.accountRole == "Администратор" || userLoginStatus.accountRole == "Модератор")
				{
					mainFrame.Navigate(adminHomePage = new AdminHomePage());
				}
				else
				{
					mainFrame.Navigate(clientHomePage = new ClientHomePage());
				}
			}

		}

		/// <summary>
		/// Вход в аккаунт. Подписка на событие.
		/// </summary>
		private async void LogOutYourAccount(object sender, EventAggregator e)
		{
			await SessionManagementAsync();
		}

		/// <summary>
		/// Выход из аккаунта. Подписка на событие.
		/// </summary>
		private async void LogInYourAccount(object sender, EventAggregator e)
		{
			await SessionManagementAsync();
		}

		/// <summary>
		/// Очистка памяти
		/// </summary>
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

			if (mainFrame != null)
			{
				// очистка фрейма
				mainFrame.Content = null;
			}
		}

	}
}