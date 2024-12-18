using CompanyNews.Models.Extended;
using CompanyNews.ViewModels.AdminApp;
using CompanyNews.ViewModels.ClientApp;
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

namespace CompanyNews.Views.ClientApp
{
    /// <summary>
    /// Interaction logic for ClientHomePage.xaml
    /// </summary>
    public partial class ClientHomePage : Page
    {
		/// <summary>
		/// Объект класса
		/// </summary>
		private readonly ClientHomeViewModel _clientHomeViewModel;

		public ClientHomePage()
        {
            InitializeComponent();

			_clientHomeViewModel = (ClientHomeViewModel)this.Resources["ClientHomeViewModel"];

			// Передаем параметры в ViewModel
			var parameters = new AdminViewModelParameters
			{
				errorInputText = this.SystemMessage,
				errorInputBorder = this.SystemMessageBorder,
				fieldIllumination = (Storyboard)FindResource("fieldIllumination")
			};

			_clientHomeViewModel.InitializeAsync(parameters, ScrollViewerPost);
		}

		/// <summary>
		/// Запуск категории
		/// </summary>
		public void LaunchingCategory(object sender, RoutedEventArgs e)
		{
			var currentItem = (e.Source as FrameworkElement)?.DataContext as NewsCategoryExtended;
			if (currentItem != null)
			{
				_clientHomeViewModel.SelectedCategory = currentItem;

				_clientHomeViewModel.LaunchingCategory(currentItem);

				// Устанавливаем IsSelected в false для всех категорий
				foreach (var category in _clientHomeViewModel.ListAvailableCategories)
				{
					category.IsSelected = false;
				}

				// Устанавливаем IsSelected для выбранной категории
				currentItem.IsSelected = true;

				ScrollViewerPost.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			}
		}

		/// <summary>
		/// Открыть пост
		/// </summary>
		public void OpenPost(object sender, RoutedEventArgs e)
		{
			var currentItem = (e.Source as FrameworkElement)?.DataContext as NewsPostExtended;
			if (currentItem != null)
			{
				_clientHomeViewModel.SelectedNewsPostExtended = currentItem;

				_clientHomeViewModel.OpenPost(currentItem);
			}
		}

		/// <summary>
		/// Поиск
		/// </summary>
		private void CategorySearch(object sender, TextChangedEventArgs e)
		{
			// Получаем текст из поля при вводе
			var textInfo = sender as System.Windows.Controls.TextBox;
			if (textInfo != null)
			{
				_clientHomeViewModel.CategorySearch(textInfo.Text);
			}
		}
	}
}
