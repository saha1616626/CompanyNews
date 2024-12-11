using CompanyNews.ViewModels.AdminApp;
using CompanyNews.ViewModels.Authorization;
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

namespace CompanyNews.Views.Authorization
{
	/// <summary>
	/// Interaction logic for AuthorizationPage.xaml
	/// </summary>
	public partial class AuthorizationPage : Page
	{
		private readonly AuthorizationViewModel _authorizationViewModel; // Связь с ViewModel
		public AuthorizationPage()
		{
			InitializeComponent();

			_authorizationViewModel = (AuthorizationViewModel)this.Resources["AuthorizationViewModel"];

			// Передаем параметры в ViewModel
			var parameters = new AdminViewModelParameters
			{
				errorInputText = this.errorInputText,
				errorInputBorder = this.errorInputBorder,
				fieldIllumination = (Storyboard)FindResource("fieldIllumination")
			};

			_authorizationViewModel.InitializeAsync(parameters, Login, Password);
		}
	}
}
