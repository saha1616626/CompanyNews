using CompanyNews.Helpers.Event;
using CompanyNews.Models;
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
using static MaterialDesignThemes.Wpf.Theme;

namespace CompanyNews.Views.AdminApp.WorkingWithData
{
	/// <summary>
	/// Interaction logic for NewsPostWorkingPage.xaml
	/// </summary>
	public partial class NewsPostWorkingPage : Page
	{
		/// <summary>
		/// Объект класса
		/// </summary>
		private readonly NewsPostWorkingViewModel _newsPostWorkingViewModel;

		public NewsPostWorkingPage(bool IsAddData, NewsPostExtended newsPostExtended)
		{
			InitializeComponent();

			var parameters = new AdminViewModelParameters
			{
				errorInputText = this.SystemMessage,
				errorInputBorder = this.SystemMessageBorder,
				fieldIllumination = (Storyboard)FindResource("fieldIllumination")
			};

			_newsPostWorkingViewModel = (NewsPostWorkingViewModel)this.Resources["NewsPostWorkingViewModel"];
			_newsPostWorkingViewModel.InitializeAsync(parameters, message, Category);  // Передаем параметры в ViewModel
			_newsPostWorkingViewModel.InitialPageSetup(IsAddData, newsPostExtended); // Первоначальная настройка страницы
		}

		/// <summary>
		/// Закрываем "гамбургер" меню, если открыто, при нажатии на окно, но не на меню
		/// </summary>
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			HamburgerMenuEvent.CloseHamburgerMenu();
		}

		#region Popup

		

		#endregion
	}
}
