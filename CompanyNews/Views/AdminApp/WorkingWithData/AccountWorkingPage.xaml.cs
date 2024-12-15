using CompanyNews.Helpers.Event;
using CompanyNews.Models.Extended;
using CompanyNews.ViewModels.AdminApp;
using CompanyNews.ViewModels.AdminApp.WorkingWithData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace CompanyNews.Views.AdminApp.WorkingWithData
{
	/// <summary>
	/// Interaction logic for AccountWorkingPage.xaml
	/// </summary>
	public partial class AccountWorkingPage : Page
	{
		/// <summary>
		/// Объект класса
		/// </summary>
		private readonly AccountWorkingViewModel _accountWorkingViewModel;

		public AccountWorkingPage(bool IsAddData, AccountExtended? accountExtended)
		{
			InitializeComponent();

			_accountWorkingViewModel = (AccountWorkingViewModel)this.Resources["AccountWorkingViewModel"];

			var parameters = new AdminViewModelParameters
			{
				errorInputText = this.SystemMessage,
				errorInputBorder = this.SystemMessageBorder,
				fieldIllumination = (Storyboard)FindResource("fieldIllumination")
			};

			_accountWorkingViewModel.InitializeAsync(parameters, Name, Surname, Patronymic,
				NumberPhone, Role, Login, Password, RepeatPassword, WorkDepartment, ReasonBlockingMessages);

			_accountWorkingViewModel.InitialPageSetup(IsAddData, accountExtended); // Первоначальная настройка страницы
																				   // Передаем параметры в ViewModel
		}

		/// <summary>
		/// Закрываем "гамбургер" меню, если открыто, при нажатии на окно, но не на меню
		/// </summary>
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			HamburgerMenuEvent.CloseHamburgerMenu();
		}
	}
}
