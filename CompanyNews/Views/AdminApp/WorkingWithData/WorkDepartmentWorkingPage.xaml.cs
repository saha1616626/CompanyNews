using CompanyNews.Helpers.Event;
using CompanyNews.Models;
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

			_workDepartmentWorkingViewModel = (WorkDepartmentWorkingViewModel)this.Resources["WorkDepartmentWorkingViewModel"];
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
