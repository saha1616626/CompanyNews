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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CompanyNews.Views.AdminApp
{
	/// <summary>
	/// Interaction logic for AccountModeratorPage.xaml
	/// </summary>
	public partial class AccountModeratorPage : Page
	{
		/// <summary>
		/// Объект класса
		/// </summary>
		private readonly AccountModeratorViewModel _accountModeratorViewModel;
		public AccountModeratorPage()
		{
			InitializeComponent();

			_accountModeratorViewModel = (AccountModeratorViewModel)this.Resources["AccountModeratorViewModel"];
			// Передаем параметры в ViewModel
			var parameters = new AdminViewModelParameters
			{
				errorInputText = this.SystemMessage,
				errorInputBorder = this.SystemMessageBorder,
				fieldIllumination = (Storyboard)FindResource("fieldIllumination")
			};

			_accountModeratorViewModel.InitializeAsync(parameters, PopupMessage, PopupMessageBorder, DescriptionBlocking);
		}

		/// <summary>
		/// Закрываем "гамбургер" меню, если открыто, при нажатии на окно, но не на меню
		/// </summary>
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			HamburgerMenuEvent.CloseHamburgerMenu();
		}

		/// <summary>
		/// Поиск пользователя
		/// </summary>
		private void UserSearch(object sender, TextChangedEventArgs e)
		{
			// Получаем текст из поля при вводе
			var textInfo = sender as System.Windows.Controls.TextBox;
			if (textInfo != null)
			{
				_accountModeratorViewModel.UserSearch(textInfo.Text);
			}
		}

		/// <summary>
		/// Изменение размера таблицы в зависимости от размера окна
		/// </summary>
		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// Получаем новые размеры окна
			double newHeight = e.NewSize.Height;
			double minSizeTable = 410;
			double minSizeWindow = 700;

			// Устанавливаем размеры DataGrid в зависимости от размера окна

			MyDataGrid.Height = newHeight - minSizeWindow + minSizeTable;
		}

		#region Popup

		// скрыть фон при скрытие popup
		private void MyPopup_Closed(object sender, EventArgs e)
		{
			DarkBackground.Visibility = Visibility.Collapsed;
		}

		#endregion
	}
}