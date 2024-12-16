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
using CompanyNews.Views.AdminApp;

namespace CompanyNews.ViewModels.AdminApp
{
	public class NewsCategoryViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly NewsCategoryService _newsCategoryService;

		/// <summary>
		/// Отображаемый список категорий в UI
		/// </summary>
		private ObservableCollection<NewsCategory> _listNewsCategories {  get; set; }
		public ObservableCollection<NewsCategory> ListNewsCategories
		{
			get { return _listNewsCategories; }
			set { _listNewsCategories = value; OnPropertyChanged(nameof(ListNewsCategories)); }
		}

		public NewsCategoryViewModel()
		{
			_newsCategoryService = ServiceLocator.GetService<NewsCategoryService>();
			ListNewsCategories = new ObservableCollection<NewsCategory>();
			SettingUpPage(); // Первоначальная настройка страницы
			LoadNewsCategory(); // Выводим список на экран

			// Подписываемся на событие — успшное добавление данных.
			WorkingWithDataEvent.dataWasAddedSuccessfullyNewsCategory += DataWasAddedSuccessfullyNewsCategory;
			// Подписываемся на событие — успшное изменение данных.
			WorkingWithDataEvent.dataWasChangedSuccessfullyNewsCategory += DataWasChangedSuccessfullyNewsCategory;
		}

		#region CRUD Operations

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async void SettingUpPage()
		{
			DefaultListSelected = true; // Список категорий по умолчанию
			ListArchiveCategories = false; // Список архивов не отображается
			DarkBackground = Visibility.Collapsed; // Скрываем фон для Popup
		}

		/// <summary>
		/// Вывод списка всех категорий в UI.
		/// </summary>
		private async Task LoadNewsCategory()
		{
			
			if (DefaultListSelected)
			{
				ListNewsCategories.Clear(); // Чистка коллекции перед заполнением
				var newsCategories = await _newsCategoryService.GetAllNewsCategoriesAsync();
				foreach (var newsCategory in newsCategories.Reverse())
				{
					if(!newsCategory.isArchived)
					{
						ListNewsCategories.Add(newsCategory);
					}
				}
			}

			if(ListArchiveCategories)
			{
				ListNewsCategories.Clear(); // Чистка коллекции перед заполнением
				var newsCategories = await _newsCategoryService.GetAllNewsCategoriesAsync();
				foreach (var newsCategory in newsCategories.Reverse())
				{
					if (newsCategory.isArchived)
					{
						ListNewsCategories.Add(newsCategory);
					}
				}
			}
			
		}

		/// <summary>
		/// Добавить категорию
		/// </summary>
		public async Task AddNewsCategoryAsync(NewsCategory newsCategory)
		{
			var addedNewsCategory = await _newsCategoryService.AddNewsCategoryAsync(newsCategory); // Добавление в БД + возврат обновленного объекта
			ListNewsCategories.Add(addedNewsCategory); // Обновление коллекции
		}

		/// <summary>
		/// Изменить категорию
		/// </summary>
		public async Task UpdateNewsCategoryAsync(NewsCategory newsCategory)
		{
			await _newsCategoryService.UpdateNewsCategoryAsync(newsCategory); // Обновление данных в БД

			// Находим учетную запись в списке для отображения в UI и заменяем объект
			NewsCategory? NewsCategorySearch = ListNewsCategories.FirstOrDefault(a => a.id == newsCategory.id);
			if (NewsCategorySearch != null) { NewsCategorySearch = newsCategory; }
		}

		/// <summary>
		/// Удалить категорию
		/// </summary>
		public async Task DeleteNewsCategoryAsync(NewsCategory newsCategory)
		{
			await _newsCategoryService.DeleteNewsCategoryAsync(newsCategory.id); // Удаление из БД

			// Находим категорию в списке для отображения в UI и удаляем объект
			NewsCategory? NewsCategorySearch = ListNewsCategories.FirstOrDefault(a => a.id == newsCategory.id);
			if (NewsCategorySearch != null) { ListNewsCategories.Remove(NewsCategorySearch); }
		}

		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Кнопка "добавить" категорию в UI
		/// </summary>
		private RelayCommand _addNewsCategory { get; set; }
		public RelayCommand AddNewsCategory
		{
			get
			{
				return _addNewsCategory ??
					(_addNewsCategory = new RelayCommand(async (obj) =>
					{
						isAddData = true;
						HamburgerMenuEvent.CloseHamburgerMenu(); // Закрываем, если открыто "гамбургер меню"
						NewsCategoryPageFrame = new NewsCategoryWorkingPage(isAddData, SelectedNewsCategory);
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "изменить" категорию в UI
		/// </summary>
		private RelayCommand _editNewsCategory { get; set; }
		public RelayCommand EditNewsCategory
		{
			get
			{
				return _editNewsCategory ??
					(_editNewsCategory = new RelayCommand(async (obj) =>
					{
						isAddData = false;
						HamburgerMenuEvent.CloseHamburgerMenu(); // Закрываем, если открыто "гамбургер меню"
						NewsCategoryPageFrame = new NewsCategoryWorkingPage(isAddData, SelectedNewsCategory);
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "удалить" категорию в UI
		/// </summary>
		private RelayCommand _deleteNewsCategory { get; set; }
		public RelayCommand DeleteNewsCategory
		{
			get
			{
				return _deleteNewsCategory ??
					(_deleteNewsCategory = new RelayCommand(async (obj) =>
					{
						StartPoupDeleteData = true; // отображаем Popup
						DarkBackground = Visibility.Visible; // показать фон

						if (SelectedNewsCategory != null)
						{
							DataDeleted = $"Название: \"{SelectedNewsCategory.name}\"";
						}
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка удаления аккаунта в UI Popup
		/// </summary>

		private RelayCommand _saveData { get; set; }
		public RelayCommand SaveData
		{
			get
			{
				return _saveData ??
					(_saveData = new RelayCommand(async (obj) =>
					{
						if (SelectedNewsCategory != null)
						{
							await DeleteNewsCategoryAsync(SelectedNewsCategory);

							if (systemMessage != null && systemMessageBorder != null)
							{
								await ClosePopupWorkingWithData(); // Скрываем Popup
																   // Выводим сообщение об успешном удалении данных
								systemMessage.Text = $"Категория успешно удалена.";
								systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
								// Исчезание сообщения
								BeginFadeAnimation(systemMessage);
								BeginFadeAnimation(systemMessageBorder);
							}

						}


					}, (obj) => true));
			}
		}

		/// <summary>
		/// Успшное добавление данных
		/// </summary>
		public async void DataWasAddedSuccessfullyNewsCategory(object sender, EventAggregator e)
		{
			await Task.Delay(500);
			systemMessage.Text = $"Категория успешно создана.";
			systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
			// Исчезание сообщения
			BeginFadeAnimation(systemMessage);
			BeginFadeAnimation(systemMessageBorder);
		}

		/// <summary>
		/// Успшное изменение данных
		/// </summary>
		public async void DataWasChangedSuccessfullyNewsCategory(object sender, EventAggregator e)
		{
			await Task.Delay(500);
			systemMessage.Text = $"Категория успешно изменена.";
			systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
			// Исчезание сообщения
			BeginFadeAnimation(systemMessage);
			BeginFadeAnimation(systemMessageBorder);
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
				LoadNewsCategory();
			}
		}

		/// <summary>
		/// Выбран список архивных категорий в UI
		/// </summary>
		private bool _listArchiveCategories { get; set; }
		public bool ListArchiveCategories
		{
			get { return _listArchiveCategories; }
			set
			{
				_listArchiveCategories = value; OnPropertyChanged(nameof(ListArchiveCategories));
				LoadNewsCategory();
			}
		}

		/// <summary>
		/// Выбранная категория в UI
		/// </summary>
		private NewsCategory _selectedNewsCategory { get; set; }
		public NewsCategory SelectedNewsCategory
		{
			get { return _selectedNewsCategory; }
			set
			{
				_selectedNewsCategory = value; OnPropertyChanged(nameof(SelectedNewsCategory));
				OnPropertyChanged(nameof(IsWorkButtonEnable));
			}
		}

		/// <summary>
		/// Отображение кнопки «удалить» и «редактировать» в UI.
		/// </summary>
		private bool _isWorkButtonEnable { get; set; }
		public bool IsWorkButtonEnable
		{
			get { return SelectedNewsCategory != null; } // Если в таблице выбранн объект, то кнопки доступны
			set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
		}

		/// <summary>
		/// Добавление или редактирование данных
		/// </summary>
		/// <remarks>true - добавление данных; false - редактирование данных</remarks>
		private bool isAddData { get; set; }

		#region View

		// Page для запуска страницы
		private Page _newsCategoryPageFrame { get; set; }
		public Page NewsCategoryPageFrame
		{
			get { return _newsCategoryPageFrame; }
			set { _newsCategoryPageFrame = value; OnPropertyChanged(nameof(NewsCategoryPageFrame)); }
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

		#region UsersSearch

		// Cписок для фильтров таблицы
		public ObservableCollection<NewsCategory> ListSearch { get; set; } = new ObservableCollection<NewsCategory>();

		/// <summary>
		/// Поиск данных в таблицы через строку запроса
		/// </summary>
		public void CategorySearch(string searchByValue)
		{
			if (!string.IsNullOrWhiteSpace(searchByValue))
			{
				 LoadNewsCategory(); // обновляем список
				ListSearch.Clear(); // очищаем список поиска данных

				// Объединяем атрибуты сущности для поиска
				foreach (NewsCategory item in ListNewsCategories)
				{
					string description = "";
					if (item.description == null) { description = ""; } else { description = item.description; }

					string unification = item.name.ToLower() + " " + description;

					bool dataExists = unification.Contains(searchByValue.ToLowerInvariant());

					if (dataExists)
					{
						ListSearch.Add(item);
					}
				}

				ListNewsCategories.Clear(); // Очистка список перед заполнением
				ListNewsCategories = new ObservableCollection<NewsCategory>(ListSearch); // Обновление списка

				if (ListSearch.Count == 0)
				{
					if (systemMessage != null && systemMessageBorder != null)
					{
						// Оповещениие об отсутствии данных
						systemMessage.Text = $"Категория не найдена.";
						systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
						// Исчезание сообщения
						BeginFadeAnimation(systemMessage);
						BeginFadeAnimation(systemMessageBorder);
					}
				}
			}
			else
			{
				ListNewsCategories.Clear(); // Очистка список перед заполнением
				 LoadNewsCategory(); // обновляем список
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
