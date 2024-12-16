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

namespace CompanyNews.Views.AdminApp.WorkingWithData
{
	/// <summary>
	/// Interaction logic for NewsCategoryWorkingPage.xaml
	/// </summary>
	public partial class NewsCategoryWorkingPage : Page
	{
		/// <summary>
		/// Объект класса
		/// </summary>
		private readonly NewsCategoryWorkingViewModel _newsCategoryWorkingViewModel;

		public NewsCategoryWorkingPage(bool IsAddData, NewsCategory newsCategory)
		{
			InitializeComponent();

			var parameters = new AdminViewModelParameters
			{
				errorInputText = this.SystemMessage,
				errorInputBorder = this.SystemMessageBorder,
				fieldIllumination = (Storyboard)FindResource("fieldIllumination")
			};

			_newsCategoryWorkingViewModel = (NewsCategoryWorkingViewModel)this.Resources["NewsCategoryWorkingViewModel"];

			_newsCategoryWorkingViewModel.InitializeAsync(parameters, Name, Description);  // Передаем параметры в ViewModel
			_newsCategoryWorkingViewModel.InitialPageSetup(IsAddData, newsCategory); // Первоначальная настройка страницы
																				  
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
