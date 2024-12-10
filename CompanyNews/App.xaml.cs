using CompanyNews.Services;
using CompanyNews.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CompanyNews
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			// Конфигурируем сервисы перед использованием
			ServiceLocator.ConfigureServices();

		}
	}

}
