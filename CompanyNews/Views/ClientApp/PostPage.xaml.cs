using CompanyNews.Models.Extended;
using CompanyNews.ViewModels.AdminApp;
using CompanyNews.ViewModels.ClientApp;
using System;
using System.Collections.Generic;
using System.Globalization;
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
	/// Interaction logic for PostPage.xaml
	/// </summary>
	public partial class PostPage : Page
	{
		/// <summary>
		/// Объект класса
		/// </summary>
		private readonly PostViewModel _postViewModel;

		public PostPage(NewsPostExtended newsPostExtended)
		{
			InitializeComponent();

			_postViewModel = (PostViewModel)this.Resources["PostViewModel"];
			_postViewModel.newsPostExtendedSelected = newsPostExtended;
			_postViewModel.InitialPageSetup(newsPostExtended); // Первоначальная настройка страницы
		}

		/// <summary>
		/// Ввод сообщения
		/// </summary>
		private void TextMessage(object sender, TextChangedEventArgs e)
		{
			// Получаем текст из поля при вводе
			var textInfo = sender as System.Windows.Controls.TextBox;
			if (textInfo != null)
			{
				_postViewModel.TextMessage(textInfo.Text);
			}
		}
	}
}
