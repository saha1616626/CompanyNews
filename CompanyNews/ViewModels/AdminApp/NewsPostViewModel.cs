using CompanyNews.Helpers;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using CompanyNews.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;
using CompanyNews.Helpers.Event;
using CompanyNews.Views.AdminApp.WorkingWithData;

namespace CompanyNews.ViewModels.AdminApp
{
	public class NewsPostViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly NewsPostService _newsPostService;
		private readonly NewsCategoryService _newsCategoryService;

		/// <summary>
		/// Отображаемый список постов в UI
		/// </summary>
		private ObservableCollection<NewsPostExtended> _listNewsPosts {  get; set; }
		public ObservableCollection<NewsPostExtended> ListNewsPosts
		{
			get { return _listNewsPosts; }
			set { _listNewsPosts = value; OnPropertyChanged(nameof(ListNewsPosts)); }
		}


		public NewsPostViewModel()
		{
			_newsPostService = ServiceLocator.GetService<NewsPostService>();
			_newsCategoryService = ServiceLocator.GetService<NewsCategoryService>();
			ListNewsPosts = new ObservableCollection<NewsPostExtended>();
			SettingUpPage(); // Первоначальная настройка страницы
			LoadNewsPost(); // Выводим список на экран
		}

		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех постов в UI.
		/// </summary>
		private async Task LoadNewsPost()
		{
			// Поиск вне архива
			if (DefaultListSelected)
			{
				if (SelectedCategory == null)
				{
					ListNewsPosts.Clear(); // Чистка коллекции перед заполнением
					var newsPosts = await _newsPostService.GetAllNewsPostsAsync();
					foreach (var newsPost in newsPosts.Reverse())
					{
						if (!newsPost.isArchived)
						{
							ListNewsPosts.Add(newsPost);
						}
					}
				}
				else
				{
					ListNewsPosts.Clear(); // Чистка коллекции перед заполнением
										   // Поиск по выбранной категории
					var newsPosts = await _newsPostService.GetAllNewsPostsAsync();
					foreach (var newsPost in newsPosts.Reverse())
					{
						if (newsPost.newsCategoryId == SelectedCategory.id)
						{
							if (!newsPost.isArchived)
							{
								ListNewsPosts.Add(newsPost);
							}
						}
					}
				}
			}

			// Поиск по архиву
			if (ListArchive)
			{
				if (SelectedCategory == null)
				{
					ListNewsPosts.Clear(); // Чистка коллекции перед заполнением
					var newsPosts = await _newsPostService.GetAllNewsPostsAsync();
					foreach (var newsPost in newsPosts.Reverse())
					{
						if (newsPost.isArchived)
						{
							ListNewsPosts.Add(newsPost);
						}
					}
				}
				else
				{
					ListNewsPosts.Clear(); // Чистка коллекции перед заполнением
										   // Поиск по выбранной категории
					var newsPosts = await _newsPostService.GetAllNewsPostsAsync();
					foreach (var newsPost in newsPosts.Reverse())
					{
						if (newsPost.newsCategoryId == SelectedCategory.id)
						{
							if (newsPost.isArchived)
							{
								ListNewsPosts.Add(newsPost);
							}
						}
					}
				}
			}

		}

		/// <summary>
		/// Добавить пост
		/// </summary>
		public async Task AddAccountAsync(NewsPost newsPost)
		{
			var addedNewsPost = await _newsPostService.AddNewsPostAsync(newsPost); // Добавление в БД + возврат обновленного объекта
			ListNewsPosts.Add(await _newsPostService.NewsPostConvert(addedNewsPost)); // Обновление коллекции
		}

		/// <summary>
		/// Изменить пост
		/// </summary>
		public async Task UpdateAccountAsync(NewsPost newsPost)
		{
			await _newsPostService.UpdateNewsPostAsync(newsPost); // Обновление данных в БД

			// Находим пост в списке для отображения в UI и заменяем объект
			NewsPostExtended? newsPostExtended = ListNewsPosts.FirstOrDefault(a => a.id == newsPost.id);
			if (newsPostExtended != null) { newsPostExtended = await _newsPostService.NewsPostConvert(newsPost); }
		}

		/// <summary>
		/// Удалить пост
		/// </summary>
		public async Task DeleteNewsPostAsync(NewsPost newsPost)
		{
			await _newsPostService.DeleteNewsPostAsync(newsPost.id); // Удаление из БД

			// Находим пост в списке для отображения в UI и удаляем объект
			NewsPostExtended? newsPostExtended = ListNewsPosts.FirstOrDefault(a => a.id == newsPost.id);
			if (newsPostExtended != null) { ListNewsPosts.Remove(newsPostExtended); }
		}

		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async void SettingUpPage()
		{
			DefaultListSelected = true; // Список по умолчанию
			ListArchive = false; // Список архивов не отображается
			DarkBackground = Visibility.Collapsed; // Скрываем фон для Popup

			// Получаем список категорий для фильтрации
			ListCategory = new List<NewsCategory>(await _newsCategoryService.GetAllNewsCategoriesAsync());

		}

		/// <summary>
		/// Кнопка "добавить" пост в UI
		/// </summary>
		private RelayCommand _add { get; set; }
		public RelayCommand Add
		{
			get
			{
				return _add ??
					(_add = new RelayCommand(async (obj) =>
					{
						isAddData = true;
						HamburgerMenuEvent.CloseHamburgerMenu(); // Закрываем, если открыто "гамбургер меню"
						PageFrame = new NewsPostWorkingPage(isAddData, SelectedNewsPost);
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "изменить" пост в UI
		/// </summary>
		private RelayCommand _edit { get; set; }
		public RelayCommand Edit
		{
			get
			{
				return _edit ??
					(_edit = new RelayCommand(async (obj) =>
					{
						isAddData = false;
						HamburgerMenuEvent.CloseHamburgerMenu(); // Закрываем, если открыто "гамбургер меню"
						PageFrame = new NewsPostWorkingPage(isAddData, SelectedNewsPost);
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "удалить" в UI
		/// </summary>
		private RelayCommand _delete { get; set; }
		public RelayCommand Delete
		{
			get
			{
				return _delete ??
					(_delete = new RelayCommand(async (obj) =>
					{
						if (SelectedNewsPost != null)
						{
							StartPoupDeleteData = true; // отображаем Popup
							DarkBackground = Visibility.Visible; // показать фон

							DataDeleted = $"Дата публикации: \"{SelectedNewsPost.datePublication}\"\nКатегория: \"{SelectedNewsPost.newsCategoryName}\"";
						}
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка для удаления в UI Popup
		/// </summary>

		private RelayCommand _saveData { get; set; }
		public RelayCommand SaveData
		{
			get
			{
				return _saveData ??
					(_saveData = new RelayCommand(async (obj) =>
					{
						if (SelectedNewsPost != null)
						{
							NewsPost newsPost = new NewsPost();
							newsPost.id = SelectedNewsPost.id;

							await DeleteNewsPostAsync(newsPost);

							if (systemMessage != null && systemMessageBorder != null)
							{
								await ClosePopupWorkingWithData(); // Скрываем Popup
																   // Выводим сообщение об успешном удалении данных
								systemMessage.Text = $"Пост успешно удален.";
								systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
								// Исчезание сообщения
								BeginFadeAnimation(systemMessage);
								BeginFadeAnimation(systemMessageBorder);
							}

						}


					}, (obj) => true));
			}
		}

		#endregion

		#region Popup

		/// <summary>
		/// Скрыть popup (все открытые)
		/// </summary>
		private RelayCommand _closePopup { get; set; }
		public RelayCommand ClosePopup
		{
			get
			{
				return _closePopup ??
					(_closePopup = new RelayCommand(async (obj) =>
					{
						await ClosePopupWorkingWithData();
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Скрытие popup
		/// </summary>
		private async Task ClosePopupWorkingWithData()
		{
			// Закрываем Popup
			StartPoupDeleteData = false;
			DarkBackground = Visibility.Collapsed; // Скрываем фон
		}

		#region FeaturesPopup

		/// <summary>
		/// Popup удаления данных
		/// </summary>
		private bool _startPoupDeleteData { get; set; }
		public bool StartPoupDeleteData
		{
			get { return _startPoupDeleteData; }
			set
			{
				_startPoupDeleteData = value;
				OnPropertyChanged(nameof(StartPoupDeleteData));
			}
		}

		/// <summary>
		/// Данные передаются в Popup, как предпросмотр перед удалением
		/// </summary>
		private string _dataDeleted { get; set; }
		public string DataDeleted
		{
			get { return _dataDeleted; }
			set { _dataDeleted = value; OnPropertyChanged(nameof(DataDeleted)); }
		}

		/// <summary>
		/// Затемненный фон позади Popup
		/// </summary>
		private Visibility _darkBackground { get; set; }
		public Visibility DarkBackground
		{
			get { return _darkBackground; }
			set
			{
				_darkBackground = value;
				OnPropertyChanged(nameof(DarkBackground));
			}
		}

		#endregion

		#endregion

		#region Features

		/// <summary>
		/// Асинхронно получаем информацию из привязанного View
		/// </summary>
		public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters)
		{
			darkBackground = adminViewModelParameters.darkBackground;
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			systemMessageBorder = adminViewModelParameters.errorInputBorder;
			systemMessage = adminViewModelParameters.errorInputText;
			deleteDataPopup = adminViewModelParameters.deleteDataPopup;
		}

		/// <summary>
		/// Выбран список сортировки по умолчанию в UI
		/// </summary>
		private bool _defaultListSelected { get; set; }
		public bool DefaultListSelected
		{
			get { return _defaultListSelected; }
			set
			{
				_defaultListSelected = value; OnPropertyChanged(nameof(DefaultListSelected));
				LoadNewsPost();
			}
		}

		/// <summary>
		/// Выбран список архивных категорий в UI
		/// </summary>
		private bool _listArchive { get; set; }
		public bool ListArchive
		{
			get { return _listArchive; }
			set
			{
				_listArchive = value; OnPropertyChanged(nameof(ListArchive));
				LoadNewsPost();
			}
		}

		/// <summary>
		/// Выбранная категория
		/// </summary>
		private NewsCategory _selectedCategory { get; set; }
		public NewsCategory SelectedCategory
		{
			get { return _selectedCategory; }
			set { _selectedCategory = value; OnPropertyChanged(nameof(SelectedCategory)); LoadNewsPost(); }
		}

		/// <summary>
		/// Список категорий
		/// </summary>
		private List<NewsCategory> _listCategory {  get; set; }
		public List<NewsCategory> ListCategory
		{
			get { return _listCategory; }
			set { _listCategory = value; OnPropertyChanged(nameof(ListCategory)); }
		}

		/// <summary>
		/// Выбранный пост в UI
		/// </summary>
		private NewsPostExtended _selectedNewsPost { get; set; }
		public NewsPostExtended SelectedNewsPost
		{
			get { return _selectedNewsPost; }
			set
			{
				_selectedNewsPost = value; OnPropertyChanged(nameof(SelectedNewsPost));
				OnPropertyChanged(nameof(IsWorkButtonEnable));
			}
		}

		/// <summary>
		/// Отображение кнопки «удалить» и «редактировать» в UI.
		/// </summary>
		private bool _isWorkButtonEnable { get; set; }
		public bool IsWorkButtonEnable
		{
			get { return SelectedNewsPost != null; } // Если в таблице выбранн объект, то кнопки доступны
			set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
		}

		/// <summary>
		/// Добавление или редактирование данных
		/// </summary>
		/// <remarks>true - добавление данных; false - редактирование данных</remarks>
		private bool isAddData { get; set; }

		#region View

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

		/// <summary>
		/// Cписок для фильтров таблицы
		/// </summary>
		public ObservableCollection<NewsPostExtended> ListSearch { get; set; } = new ObservableCollection<NewsPostExtended>();

		/// <summary>
		/// Поиск данных в таблицы через строку запроса
		/// </summary>
		public void SearchNewsPost(string searchByValue)
		{
			if (!string.IsNullOrWhiteSpace(searchByValue))
			{
				LoadNewsPost(); // обновляем список
				ListSearch.Clear(); // очищаем список поиска данных

				// Объединяем атрибуты сущности для поиска
				foreach (NewsPostExtended item in ListNewsPosts)
				{
					string message = "";
					if (item.message == null) { message = ""; } else { message = item.message; }

					string unification = item.newsCategoryName.ToLower() + " " + message.ToLower() + " " + item.datePublication.ToString().ToLower();

					bool dataExists = unification.Contains(searchByValue.ToLowerInvariant());

					if (dataExists)
					{
						ListSearch.Add(item);
					}
				}

				ListNewsPosts.Clear(); // Очистка список перед заполнением
				ListNewsPosts = new ObservableCollection<NewsPostExtended>(ListSearch); // Обновление списка

				if (ListSearch.Count == 0)
				{
					if (systemMessage != null && systemMessageBorder != null)
					{
						// Оповещениие об отсутствии данных
						systemMessage.Text = $"Пост не найден.";
						systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
						// Исчезание сообщения
						BeginFadeAnimation(systemMessage);
						BeginFadeAnimation(systemMessageBorder);
					}
				}
			}
			else
			{
				ListNewsPosts.Clear(); // Очистка список перед заполнением
				LoadNewsPost(); // обновляем список
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
