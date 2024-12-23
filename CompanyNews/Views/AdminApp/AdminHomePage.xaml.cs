﻿using CompanyNews.Helpers.Event;
using CompanyNews.ViewModels.AdminApp;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CompanyNews.Views.AdminApp
{
    /// <summary>
    /// Interaction logic for AdminHomePage.xaml
    /// </summary>
    public partial class AdminHomePage : Page
    {
		/// <summary>
		/// Объект класса
		/// </summary>
		private readonly AdminHomeViewModel _adminHomeViewModel;

		public AdminHomePage()
        {
			InitializeComponent();

			_adminHomeViewModel = (AdminHomeViewModel)this.Resources["AdminHomeViewModel"];
			// Ассинхронно передаём фрейм в ViewModel
			_adminHomeViewModel.InitializeAsync(adminHomeFrame);
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
		}



		#endregion
	}
}
