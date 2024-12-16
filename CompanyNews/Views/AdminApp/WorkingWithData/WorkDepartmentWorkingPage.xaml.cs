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
	/// Interaction logic for WorkDepartmentWorkingPage.xaml
	/// </summary>
	public partial class WorkDepartmentWorkingPage : Page
	{
		/// <summary>
		/// Объект класса
		/// </summary>
		private readonly WorkDepartmentWorkingViewModel _workDepartmentWorkingViewModel;

		public WorkDepartmentWorkingPage(bool IsAddData, WorkDepartment workDepartment)
		{
			InitializeComponent();

			var parameters = new AdminViewModelParameters
			{
				errorInputText = this.SystemMessage,
				errorInputBorder = this.SystemMessageBorder,
				fieldIllumination = (Storyboard)FindResource("fieldIllumination")
			};

			_workDepartmentWorkingViewModel = (WorkDepartmentWorkingViewModel)this.Resources["WorkDepartmentWorkingViewModel"];

			_workDepartmentWorkingViewModel.InitializeAsync(parameters, Name, Description, PopupMessageBorder, PopupMessage);  // Передаем параметры в ViewModel
			_workDepartmentWorkingViewModel.InitialPageSetup(IsAddData, workDepartment); // Первоначальная настройка страницы
		}

		/// <summary>
		/// Закрываем "гамбургер" меню, если открыто, при нажатии на окно, но не на меню
		/// </summary>
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			HamburgerMenuEvent.CloseHamburgerMenu();
		}

		/// <summary>
		/// Изменение размера таблицы в зависимости от размера окна
		/// </summary>
		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// Получаем новые размеры окна
			double newHeight = e.NewSize.Height;
			double minSizeTable = 350;
			double minSizeWindow = 700;

			// Устанавливаем размеры DataGrid в зависимости от размера окна

			MyDataGrid.Height = newHeight - minSizeWindow + minSizeTable;
		}

		#region Popup

		/// <summary>
		/// скрыть фон при скрытие popup
		/// </summary>
		private void MyPopup_Closed(object sender, EventArgs e)
		{
			DarkBackground.Visibility = Visibility.Collapsed;

			_workDepartmentWorkingViewModel.systemMessage.Text = $"Для подтверждения внесенных изменений нажмите кнопку «Сохранить».";
			_workDepartmentWorkingViewModel.systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
			// Исчезание сообщения
			_workDepartmentWorkingViewModel.BeginFadeAnimation(SystemMessage);
			_workDepartmentWorkingViewModel.BeginFadeAnimation(SystemMessageBorder);
		}

		/// <summary>
		/// Кнопка добавить категорию
		/// </summary>
		public void AddCategory(object sender, RoutedEventArgs e)
		{
			var currentItem = (e.Source as FrameworkElement)?.DataContext as NewsCategoryExtended;
			if(currentItem != null)
			{
				_workDepartmentWorkingViewModel.AddCategory(currentItem);
			}
		}

		/// <summary>
		/// Кнопка убрать категорию
		/// </summary>
		public void DeleteCategory(object sender, RoutedEventArgs e)
		{
			var currentItem = (e.Source as FrameworkElement)?.DataContext as NewsCategoryExtended;
			if (currentItem != null)
			{
				_workDepartmentWorkingViewModel.DeleteCategory(currentItem);
			}
		}

		#endregion

		/// <summary>
		/// Поиск категории
		/// </summary>
		private void CategorySearch(object sender, TextChangedEventArgs e)
		{
			// Получаем текст из поля при вводе
			var textInfo = sender as System.Windows.Controls.TextBox;
			if (textInfo != null)
			{
				_workDepartmentWorkingViewModel.CategorySearch(textInfo.Text);
			}
		}
	}
}
