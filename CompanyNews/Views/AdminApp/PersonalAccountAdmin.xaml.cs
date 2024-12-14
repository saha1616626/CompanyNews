using CompanyNews.Helpers.Event;
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

namespace CompanyNews.Views.AdminApp
{
    /// <summary>
    /// Interaction logic for PersonalAccountAdmin.xaml
    /// </summary>
    public partial class PersonalAccountAdmin : Page
    {
		private readonly PersonalAccountAdminViewModel _personalAccountAdminViewModel; // Связь с ViewModel

		public PersonalAccountAdmin()
        {
            InitializeComponent();

			_personalAccountAdminViewModel = (PersonalAccountAdminViewModel)this.Resources["PersonalAccountAdminViewModel"];

			// Передаем параметры в ViewModel
			var parameters = new AdminViewModelParameters
			{
				errorInputText = this.SystemMessage,
				errorInputBorder = this.SystemMessageBorder,
				fieldIllumination = (Storyboard)FindResource("fieldIllumination")
			};

			_personalAccountAdminViewModel.InitializeAsync(parameters);
		}


		// Закрываем "гамбургер" меню, если открыто, при нажатии на окно, но не на меню
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			HamburgerMenuEvent.CloseHamburgerMenu();
		}

		#region Popup

		/// <summary>
		/// Скрыть фон при закрытие popup
		/// </summary>
		private void PopupClosed(object sender, EventArgs e)
		{
			DarkBackground.Visibility = Visibility.Collapsed;

			// Прежде чем выводим сообщение, проверяем успех операции
			if(!_personalAccountAdminViewModel.IsOperationSuccessful || _personalAccountAdminViewModel.IsOperationSuccessful == null)
			{
				// Сообщение об завершении операции
				SystemMessage.Text = "Операция отменена.";
				SystemMessageBorder.Visibility = System.Windows.Visibility.Visible;
				// Исчезание сообщения
				_personalAccountAdminViewModel.BeginFadeAnimation(SystemMessage);
				_personalAccountAdminViewModel.BeginFadeAnimation(SystemMessageBorder);
			}
			else
			{
				_personalAccountAdminViewModel.IsOperationSuccessful = false;
			}
			
		}

		#endregion
	}
}
