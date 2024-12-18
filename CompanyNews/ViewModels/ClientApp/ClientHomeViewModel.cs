using CompanyNews.Helpers.Event;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using CompanyNews.Services;
using CompanyNews.ViewModels.AdminApp;
using CompanyNews.Views.ClientApp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace CompanyNews.ViewModels.ClientApp
{
	public class ClientHomeViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly AuthorizationService _authorizationService;
		private readonly NewsCategoriesWorkDepartmentService _newsCategoriesWorkDepartmentService;
		private readonly NewsPostService _newsPostService;

		/// <summary>
		/// Отображаемый список в UI
		/// </summary>
		private ObservableCollection<NewsCategoryExtended> _listAvailableCategories { get; set; }
		public ObservableCollection<NewsCategoryExtended> ListAvailableCategories
		{
			get { return _listAvailableCategories; }
			set { _listAvailableCategories = value; OnPropertyChanged(nameof(ListAvailableCategories)); }
		}

		private ObservableCollection<NewsPostExtended> _listNewsPostExtendeds { get; set; }
		public ObservableCollection<NewsPostExtended> ListNewsPostExtendeds
		{
			get { return _listNewsPostExtendeds; }
			set { _listNewsPostExtendeds = value; OnPropertyChanged(nameof(ListNewsPostExtendeds)); }
		}

		public ClientHomeViewModel()
		{
			_authorizationService = ServiceLocator.GetService<AuthorizationService>();
			_newsCategoriesWorkDepartmentService = ServiceLocator.GetService<NewsCategoriesWorkDepartmentService>();
			_newsPostService = ServiceLocator.GetService<NewsPostService>();
			ListAvailableCategories = new ObservableCollection<NewsCategoryExtended>();
			ListNewsPostExtendeds = new ObservableCollection<NewsPostExtended>();
			LoadCategory(); // Выводим список на экран
			SettingUpPage(); // Первоначальная настройка страницы

			// Подписываемся на событие — выход из поста.
			ClientAppEvent.exitPost += ExitPost;
		}

		#region WorkingWithPages

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async void SettingUpPage()
		{
			IsNoСhannel = false;
			IsNoPost = false;

			// Запуск первой категории
			NewsCategoryExtended newsCategoryExtended = ListAvailableCategories[0];
			if (newsCategoryExtended != null)
			{
				HeadingPage = newsCategoryExtended.name;
				newsCategoryExtended.IsSelected = true;
				await LaunchingCategory(newsCategoryExtended);
			}
		}

		// Выход из поста
		public async void ExitPost(object sender, EventAggregator e)
		{
			ScrollViewerPost.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			PageFrame = null; // Чистика фрейма
		}

		#endregion


		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех достуаных пользователю категорий в UI.
		/// </summary>
		private async Task LoadCategory()
		{
			ListAvailableCategories.Clear(); // Чистка коллекции перед заполнением
			Account account = await _authorizationService.GetUserAccount();
			if (account != null)
			{
				NewsCategoriesWorkDepartmentExtended? newsCategoriesWorkDepartmentExtended = await _newsCategoriesWorkDepartmentService.GetNewsCategoriesWorkDepartmentExtendedByIdAsync((int)account.workDepartmentId);
				if (newsCategoriesWorkDepartmentExtended != null && newsCategoriesWorkDepartmentExtended.categories != null)
				{
					foreach(var item in newsCategoriesWorkDepartmentExtended.categories)
					{
						if (!item.isArchived)
						{
							ListAvailableCategories.Add(item);
						}
					}
				}
			}
		}

		// Запуск постов категории
		public async Task LaunchingCategory(NewsCategoryExtended newsCategoryExtended)
		{
			PageFrame = null; // Чистика фрейма

			ListNewsPostExtendeds.Clear();

			// Получаем список постов данной категории
			IEnumerable<NewsPostExtended> newsPostExtended = await _newsPostService.GetAllNewsPostsAsync();

			if(newsPostExtended != null)
			{
				foreach (var item in newsPostExtended.Reverse())
				{
					if(item.newsCategoryId == newsCategoryExtended.id) // Проверяем категорию поста
					{
						if (!item.isArchived)
						{
							ListNewsPostExtendeds.Add(item);
						}
					}
				}
			}

			HeadingPage = newsCategoryExtended.name;

			if (ListNewsPostExtendeds.Count == 0)
			{
				// Оповещениие об отсутствии данных
				//systemMessage.Text = $"В этом канале пока нет публикаций.";
				//systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
				//// Исчезание сообщения
				//BeginFadeAnimation(systemMessage);
				//BeginFadeAnimation(systemMessageBorder);
				IsNoPost = true;
			}
			else
			{
				IsNoPost = false;
			}

		}

		/// <summary>
		/// Запуск поста
		/// </summary>
		public async Task OpenPost(NewsPostExtended newsPostExtended)
		{
			if(newsPostExtended != null)
			{
				ScrollViewerPost.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
				PageFrame = new PostPage(newsPostExtended);
			}
			
		}

		#endregion

		#region Features

		/// <summary>
		/// Асинхронно получаем информацию из привязанного View
		/// </summary>
		public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters, ScrollViewer scrollViewer)
		{
			darkBackground = adminViewModelParameters.darkBackground;
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			systemMessageBorder = adminViewModelParameters.errorInputBorder;
			systemMessage = adminViewModelParameters.errorInputText;
			deleteDataPopup = adminViewModelParameters.deleteDataPopup;
			ScrollViewerPost = scrollViewer;
		}

		#region View

		ScrollViewer ScrollViewerPost {  get; set; }

		/// <summary>
		/// Если канал не найден
		/// </summary>
		private bool _isNoPost { get; set; }
		public bool IsNoPost
		{
			get { return _isNoPost; }
			set { _isNoPost = value; OnPropertyChanged(nameof(IsNoPost)); }
		}

		/// <summary>
		/// Если канал не найден
		/// </summary>
		private bool _isNoСhannel { get; set; }
		public bool IsNoСhannel
		{
			get { return _isNoСhannel; }
			set { _isNoСhannel = value; OnPropertyChanged(nameof(IsNoСhannel)); }
		}

		/// <summary>
		/// Заголовок страницы c постами
		/// </summary>
		private string? _headingPage { get; set; }
		public string? HeadingPage
		{
			get { return _headingPage; }
			set { _headingPage = value; OnPropertyChanged(nameof(HeadingPage)); }
		}

		/// <summary>
		/// Выбранный пост в списке
		/// </summary>
		private NewsPostExtended _selectedNewsPostExtended;
		public NewsPostExtended SelectedNewsPostExtended
		{
			get => _selectedNewsPostExtended;
			set
			{
				if (_selectedNewsPostExtended != value)
				{
					_selectedNewsPostExtended = value;
					OnPropertyChanged(nameof(SelectedNewsPostExtended));
				}
			}
		}

		/// <summary>
		/// Выбранная категория в списке
		/// </summary>
		private NewsCategoryExtended _selectedCategory;
		public NewsCategoryExtended SelectedCategory
		{
			get => _selectedCategory;
			set
			{
				if (_selectedCategory != value)
				{
					_selectedCategory = value;
					OnPropertyChanged(nameof(SelectedCategory));
				}
			}
		}

		/// <summary>
		/// Page для запуска страницы
		/// </summary>
		private Page _pageFrame { get; set; }
		public Page PageFrame
		{
			get { return _pageFrame; }
			set { _pageFrame = value; OnPropertyChanged(nameof(PageFrame)); }
		}

		/// <summary>
		/// Затемненный фон позади Popup
		/// </summary>
		public Border? darkBackground { get; set; }

		/// <summary>
		/// Анимация полей
		/// </summary>
		public Storyboard? fieldIllumination { get; set; }

		/// <summary>
		/// Вывод ошибки и анимация текста в Popup
		/// </summary>
		public Border? systemMessageBorder { get; set; }

		/// <summary>
		/// Вывод ошибки и анимация текста на странице
		/// </summary>
		public TextBlock? systemMessage { get; set; }

		/// <summary>
		/// Popup удаления данных
		/// </summary>
		public Popup? deleteDataPopup { get; set; }

		#endregion

		#endregion

		#region Search

		// Cписок для фильтров таблицы
		public ObservableCollection<NewsCategoryExtended> ListSearch { get; set; } = new ObservableCollection<NewsCategoryExtended>();

		/// <summary>
		/// Поиск данных в таблицы через строку запроса
		/// </summary>
		public void CategorySearch(string searchByValue)
		{
			if (!string.IsNullOrWhiteSpace(searchByValue))
			{
				LoadCategory(); // обновляем список
				ListSearch.Clear(); // очищаем список поиска данных

				// Объединяем атрибуты сущности для поиска
				foreach (NewsCategoryExtended item in ListAvailableCategories)
				{
					string description = "";
					//if (item.description == null) { description = ""; } else { description = item.description; }

					string unification = item.name.ToLower() + " " + description.ToLower();

					bool dataExists = unification.Contains(searchByValue.ToLowerInvariant());

					if (dataExists)
					{
						ListSearch.Add(item);
					}
				}

				ListAvailableCategories.Clear(); // Очистка список перед заполнением
				ListAvailableCategories = new ObservableCollection<NewsCategoryExtended>(ListSearch); // Обновление списка

				if (ListSearch.Count == 0)
				{
					if (systemMessage != null && systemMessageBorder != null)
					{
						//// Оповещениие об отсутствии данных
						//systemMessage.Text = $"Категория не найдена.";
						//systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
						//// Исчезание сообщения
						//BeginFadeAnimation(systemMessage);
						//BeginFadeAnimation(systemMessageBorder);

						IsNoСhannel = true;
					}
				}
				else
				{
					IsNoСhannel = false;
				}
			}
			else
			{
				ListAvailableCategories.Clear(); // Очистка список перед заполнением
				LoadCategory(); // обновляем список
				IsNoСhannel = false;
			}
		}

		#endregion

		#region Animation

		// выводим сообщения об ошибке с анимацией затухания
		public async void BeginFadeAnimation(TextBlock textBlock)
		{
			textBlock.IsEnabled = true;
			textBlock.Opacity = 1.0;

			Storyboard storyboard = new Storyboard();
			DoubleAnimation fadeAnimation = new DoubleAnimation
			{
				From = 2.0,
				To = 0.0,
				Duration = TimeSpan.FromSeconds(2),
			};
			Storyboard.SetTargetProperty(fadeAnimation, new System.Windows.PropertyPath(System.Windows.UIElement.OpacityProperty));
			storyboard.Children.Add(fadeAnimation);
			storyboard.Completed += (s, e) => textBlock.IsEnabled = false;
			storyboard.Begin(textBlock);
		}

		public async void BeginFadeAnimation(Border border)
		{
			border.IsEnabled = true;
			border.Opacity = 1.0;

			Storyboard storyboard = new Storyboard();
			DoubleAnimation fadeAnimation = new DoubleAnimation
			{
				From = 2.0,
				To = 0.0,
				Duration = TimeSpan.FromSeconds(2),
			};
			Storyboard.SetTargetProperty(fadeAnimation, new System.Windows.PropertyPath(System.Windows.UIElement.OpacityProperty));
			storyboard.Children.Add(fadeAnimation);
			storyboard.Completed += (s, e) => border.IsEnabled = false;
			storyboard.Begin(border);
		}

		// запускаем анимации для TextBox (подсвечивание объекта)
		private void StartFieldIllumination(TextBox textBox)
		{
			fieldIllumination.Begin(textBox);
		}
		private void StartFieldIllumination(PasswordBox passwordBox)
		{
			fieldIllumination.Begin(passwordBox);
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
