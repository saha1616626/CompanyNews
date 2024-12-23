﻿using CompanyNews.Helpers.Event;
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

namespace CompanyNews.Views.AdminApp
{
    /// <summary>
    /// Interaction logic for MessageUserPage.xaml
    /// </summary>
    public partial class MessageUserPage : Page
    {
		/// <summary>
		/// Объект класса
		/// </summary>
		private readonly MessageUserViewModel _messageUserViewModel;
		public MessageUserPage()
        {
            InitializeComponent();

			var parameters = new AdminViewModelParameters
			{
				errorInputText = this.SystemMessage,
				errorInputBorder = this.SystemMessageBorder,
				fieldIllumination = (Storyboard)FindResource("fieldIllumination")
			};

			_messageUserViewModel = (MessageUserViewModel)this.Resources["MessageUserViewModel"];
			_messageUserViewModel.InitializeAsync(parameters, PopupMessage, PopupMessageBorder, DescriptionBlocking);
		}

		/// <summary>
		/// Поиск
		/// </summary>
		private void SearchNewsPost(object sender, TextChangedEventArgs e)
		{
			// Получаем текст из поля при вводе
			var textInfo = sender as System.Windows.Controls.TextBox;
			if (textInfo != null)
			{
				_messageUserViewModel.SearchNewsPost(textInfo.Text);
			}
		}

		/// <summary>
		/// Закрываем "гамбургер" меню, если открыто, при нажатии на окно, но не на меню
		/// </summary>
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			HamburgerMenuEvent.CloseHamburgerMenu();
		}

		#region ButtonList

		/// <summary>
		/// Блокировка пользователя (писать сообщения)
		/// </summary>
		public void BlockAccount(object sender, RoutedEventArgs e)
		{
			var currentItem = (e.Source as FrameworkElement)?.DataContext as MessageUserExtended;
			if (currentItem != null)
			{
				_messageUserViewModel.CheckBlockAccount(currentItem);
			}
		}

		/// <summary>
		/// Разблокировать пользователя (писать сообщения)
		/// </summary>
		public void UnlockAccount(object sender, RoutedEventArgs e)
		{
			var currentItem = (e.Source as FrameworkElement)?.DataContext as MessageUserExtended;
			if (currentItem != null)
			{
				_messageUserViewModel.CheckUnlockAccount(currentItem);
			}
		}

		/// <summary>
		/// Одобрить сообщение пользователя
		/// </summary>
		public void ApproveMessage(object sender, RoutedEventArgs e)
        {
			var currentItem = (e.Source as FrameworkElement)?.DataContext as MessageUserExtended;
			if (currentItem != null)
			{
				_messageUserViewModel.CheckApproveMessage(currentItem);
			}
		}

		/// <summary>
		/// Отклонить сообщение пользователя
		/// </summary>
		public void RejectMessage(object sender, RoutedEventArgs e)
		{
			var currentItem = (e.Source as FrameworkElement)?.DataContext as MessageUserExtended;
			if (currentItem != null)
			{
				_messageUserViewModel.CheckRejectMessage(currentItem);
			}
		}

		/// <summary>
		/// Восстановить сообщение пользователя
		/// </summary>
		public void RestoreMessage(object sender, RoutedEventArgs e)
		{
			var currentItem = (e.Source as FrameworkElement)?.DataContext as MessageUserExtended;
			if (currentItem != null)
			{
				_messageUserViewModel.CheckRestoreMessage(currentItem);
			}
		}

		/// <summary>
		/// Удалить сообщение пользователя
		/// </summary>
		public void DeleteMessage(object sender, RoutedEventArgs e)
		{
			var currentItem = (e.Source as FrameworkElement)?.DataContext as MessageUserExtended;
			if (currentItem != null)
			{
				_messageUserViewModel.CheckDeleteMessage(currentItem);
			}
		}

		#endregion


		#region Popup

		// скрыть фон при скрытие popup
		private void MyPopup_Closed(object sender, EventArgs e)
		{
			DarkBackground.Visibility = Visibility.Collapsed;
		}

		#endregion

	}
}
