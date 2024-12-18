using CompanyNews.Helpers;
using CompanyNews.Helpers.Event;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using CompanyNews.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace CompanyNews.ViewModels.AdminApp.WorkingWithData
{
	public class NewsPostWorkingViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly NewsPostService _newsPostService;
		private readonly NewsCategoryService _newsCategoryService;

		public NewsPostWorkingViewModel()
		{
			_newsPostService = ServiceLocator.GetService<NewsPostService>();
			_newsCategoryService = ServiceLocator.GetService<NewsCategoryService>();
		}

		#region WorkingWithPages

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async Task InitialPageSetup(bool IsAddData, NewsPostExtended? newsPostExtended)
		{
			this.isAddData = IsAddData;

			if (IsAddData) // Режим добавления данных
			{
				HeadingPage = "Создание поста";

				await GetListCategories(); // Вывод списка категорий
			}
			else // Режим редактирования данных
			{
				HeadingPage = "Изменение поста";

				if (newsPostExtended != null)
				{
					SelectNewsPostExtended = newsPostExtended;
					await GetListCategories(); // Вывод списка категорий

					if(animationCategory != null && animationMessage != null)
					{
						// Заполняем все поля
						animationMessage.Text = newsPostExtended.message;
						animationCategory.SelectedItem = await _newsCategoryService.GetNewsCategoryByIdAsync(newsPostExtended.newsCategoryId);
						IsAddArchive = newsPostExtended.isArchived;
						if(newsPostExtended.image != null) { ImagePost = newsPostExtended.image; IsImageVisible = true; }
					}
				}
				else
				{
					// Возврат на предыдущую страницу
					HamburgerMenuEvent.OpenPageNewsPost();
				}
			}
		}

		/// <summary>
		/// Получение списка категорий
		/// </summary>
		public async Task GetListCategories()
		{
			ListCategory = new List<NewsCategory>();

			if (isAddData) // Режим добавления данных
			{
				// Категории не в архиве
				List<NewsCategory> newsCategories = new List<NewsCategory>(await _newsCategoryService.GetAllNewsCategoriesAsync());
				if(newsCategories != null && newsCategories.Count > 0)
				{
					foreach (NewsCategory category in newsCategories)
					{
						if (!category.isArchived)
						{
							ListCategory.Add(category);
						}
					}
				}
			}
			else // Режим редактирования данных
			{
				// Категории не в архиве за исключение выбранной, если она в архиве
				List<NewsCategory> newsCategories = new List<NewsCategory>(await _newsCategoryService.GetAllNewsCategoriesAsync());
				if (newsCategories != null && newsCategories.Count > 0)
				{
					foreach (NewsCategory category in newsCategories)
					{
						if (!category.isArchived || category.id == SelectNewsPostExtended.newsCategoryId)
						{
							ListCategory.Add(category);
						}
					}
				}
			}
		}

		/// <summary>
		/// Возврат на предыдущую страницу.
		/// </summary>
		private RelayCommand? _refund { get; set; }
		public RelayCommand? Refund
		{
			get
			{
				return _refund ??
					(_refund = new RelayCommand(async (obj) =>
					{
						// Переход на страницу
						HamburgerMenuEvent.OpenPageNewsPost();
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Добавить изображение.
		/// </summary>
		private RelayCommand? _addImage { get; set; }
		public RelayCommand? AddImage
		{
			get
			{
				return _addImage ??
					(_addImage = new RelayCommand(async (obj) =>
					{

						// Выбираем изображение
						CroppedBitmap? croppedBitmap = WorkingWithImage.AddImageFromDialogBox(500);
						if (croppedBitmap != null)
						{
							ImagePost = croppedBitmap; // Выводим изображение
							IsImageVisible = true;
						}
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Удалить изображение.
		/// </summary>
		private RelayCommand? _deleteImage { get; set; }
		public RelayCommand? DeleteImage
		{
			get
			{
				return _deleteImage ??
					(_deleteImage = new RelayCommand(async (obj) =>
					{
						ImagePost = null; // Выводим изображение
						IsImageVisible = false;
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка сохранения новых или изменения старых данных в UI
		/// </summary>

		private RelayCommand _saveData { get; set; }
		public RelayCommand SaveData
		{
			get
			{
				return _saveData ??
					(_saveData = new RelayCommand(async (obj) =>
					{
						if(animationMessage != null && animationCategory != null)
						{
							// Проверка обязательных полей
							if ((!string.IsNullOrWhiteSpace(animationMessage.Text) || ImagePost != null) && animationCategory.SelectedItem != null)
							{
								if (isAddData) // Добавление данных
								{
									NewsPost newsPost = new NewsPost();
									newsPost.newsCategoryId = SelectedCategory.id;
									newsPost.datePublication = DateTime.Now;
									newsPost.message = animationMessage.Text.Trim();
									if (ImagePost == null) { newsPost.image = null; }
									else { newsPost.image = WorkingWithImage.ConvertingImageForWritingDatabase(ImagePost); }
									newsPost.isArchived = IsAddArchive;

									_newsPostService.AddNewsPostAsync(newsPost);
									// Возврат на страницу "Посты"
									HamburgerMenuEvent.OpenPageNewsPost();
									WorkingWithDataEvent.DataWasAddedSuccessfullyNewsPost(); // Уведомление об успешном добавлении данных
								}
								else
								{
									NewsPost newsPost = new NewsPost();
									newsPost.id = SelectNewsPostExtended.id;
									newsPost.newsCategoryId = SelectedCategory.id;
									newsPost.datePublication = DateTime.Now;
									newsPost.message = animationMessage.Text.Trim();
									if(ImagePost == null) { newsPost.image = null; }
									else { newsPost.image = WorkingWithImage.ConvertingImageForWritingDatabase(ImagePost); }
									newsPost.isArchived = IsAddArchive;

									 _newsPostService.UpdateNewsPostAsync(newsPost);
									// Возврат на страницу "Посты"
									HamburgerMenuEvent.OpenPageNewsPost();
									WorkingWithDataEvent.DataWasChangedSuccessfullyNewsPost(); // Уведомление об успешном изменении данных
								}
									
							}
							else
							{
								systemMessage.Text = "";
								systemMessageBorder.Visibility = System.Windows.Visibility.Visible;

								if (animationCategory.SelectedItem == null)
								{
									systemMessage.Text = "Заполните все обязательные поля. ";
									StartFieldIllumination(animationCategory);
								}

								if (string.IsNullOrWhiteSpace(animationMessage.Text) && ImagePost == null)
								{
									systemMessage.Text += "Чтобы сохранить пост, нужно добавить хотя бы одно изображение или написать текст.";
									StartFieldIllumination(animationMessage);
								}

								// Исчезание сообщения
								BeginFadeAnimation(systemMessage);
								BeginFadeAnimation(systemMessageBorder);
							}
						}
					}, (obj) => true));
			}
		}

		#endregion

		#region Features

		/// <summary>
		/// Асинхронно получаем информацию из привязанного View
		/// </summary>
		public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters, TextBox message,
			ComboBox category)
		{
			systemMessage = adminViewModelParameters.errorInputText;
			systemMessageBorder = adminViewModelParameters.errorInputBorder;
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			animationMessage = message;
			animationCategory = category;

		}

		/// <summary>
		/// Добавить в архив
		/// </summary>
		private bool _isAddArchive { get; set; }
		public bool IsAddArchive
		{
			get { return _isAddArchive; }
			set
			{
				_isAddArchive = value;
				OnPropertyChanged(nameof(IsAddArchive));
			}
		}

		/// <summary>
		/// Видимость рамки для изображения поста
		/// </summary>
		private bool _IsImageVisible { get; set; }
		public bool IsImageVisible
		{
			get { return _IsImageVisible; }
			set
			{
				_IsImageVisible = value;
				OnPropertyChanged(nameof(IsImageVisible));
			}
		}

		/// <summary>
		/// Изображение для поста
		/// </summary>
		private CroppedBitmap _imagePost { get; set; }
		public CroppedBitmap ImagePost
		{
			get { return _imagePost; }
			set
			{
				_imagePost = value;
				OnPropertyChanged(nameof(ImagePost));
			}
		}

		/// <summary>
		/// Выбранная категория
		/// </summary>
		private NewsCategory _selectedCategory { get; set; }
		public NewsCategory SelectedCategory
		{
			get { return _selectedCategory; }
			set { _selectedCategory = value; OnPropertyChanged(nameof(SelectedCategory)); }
		}

		/// <summary>
		/// Список категорий
		/// </summary>
		private List<NewsCategory> _listCategory { get; set; }
		public List<NewsCategory> ListCategory
		{
			get { return _listCategory; }
			set { _listCategory = value; OnPropertyChanged(nameof(ListCategory)); }
		}

		/// <summary>
		/// Тело поста
		/// </summary>
		public TextBox? animationMessage { get; set; }

		/// <summary>
		/// Выбор категории
		/// </summary>
		public ComboBox? animationCategory { get; set; }

		// <summary>
		/// Анимация полей
		/// </summary>
		public Storyboard? fieldIllumination { get; set; }

		/// <summary>
		/// Вывод сообщения системы и анимация текста на странице
		/// </summary>
		public TextBlock? systemMessage { get; set; }

		/// <summary>
		/// Вывод контейнера для сообщения системы
		/// </summary>
		public Border? systemMessageBorder { get; set; }

		/// <summary>
		/// Добавление или редактирование данных
		/// </summary>
		/// <remarks>true - добавление данных; false - редактирование данных</remarks>
		private bool isAddData { get; set; }

		/// <summary>
		///  Переданный пост для изменения
		/// </summary>
		private NewsPostExtended SelectNewsPostExtended { get; set; }

		/// <summary>
		/// Заголовок страницы
		/// </summary>
		private string? _headingPage { get; set; }
		public string? HeadingPage
		{
			get { return _headingPage; }
			set { _headingPage = value; OnPropertyChanged(nameof(HeadingPage)); }
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

		private void StartFieldIllumination(ComboBox comboBox)
		{
			fieldIllumination.Begin(comboBox);
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
