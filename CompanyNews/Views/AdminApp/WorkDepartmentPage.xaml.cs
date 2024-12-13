using CompanyNews.Helpers.Event;
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
    /// Interaction logic for WorkDepartmentPage.xaml
    /// </summary>
    public partial class WorkDepartmentPage : Page
    {
        public WorkDepartmentPage()
        {
            InitializeComponent();
        }

		// Закрываем "гамбургер" меню, если открыто, при нажатии на окно, но не на меню
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			HamburgerMenuEvent.CloseHamburgerMenu();
		}
	}
}
