using CompanyNews.Models.Authorization;
using CompanyNews.Services;
using CompanyNews.Views.Authorization;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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

		public MainWindow()
		{
			InitializeComponent();

			_authorizationService = ServiceLocator.GetService<AuthorizationService>();
			// Запускаем асинхронный метод управления сессией
			SessionManagementAsync(); // используем "fire-and-forget"
		}
		
		/// <summary>
		///  Автоматизированный вход в систему.
		/// </summary>
		public async Task SessionManagementAsync()
		{
			await Task.Delay(500);

			// Проверка состояния пользователя в системе
			UserLoginStatus userLoginStatus = await _authorizationService.GetUserStatusInSystem();
			if(userLoginStatus == null || !userLoginStatus.isUserLoggedIn)
			{
				AuthorizationPage authorizationPage = new AuthorizationPage();
				// Если пользователь не авторизован, то направляем его на страницу авторизации
				mainFrame.Navigate(authorizationPage);
			}
		}
	}
}