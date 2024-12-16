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
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CompanyNews.ViewModels.AdminApp.WorkingWithData
{
	public class NewsCategoryWorkingViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервисы для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly NewsCategoryService _newsCategoryService; // Категория

		public NewsCategoryWorkingViewModel()
		{
			_newsCategoryService = ServiceLocator.GetService<NewsCategoryService>();
		}

		#region WorkingWithPages

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async Task InitialPageSetup(bool IsAddData, NewsCategory? newsCategory)
		{
			this.isAddData = IsAddData;

			if (IsAddData) // Режим добавления данных
			{
				HeadingPage = "Создание категори";
			}
			else
			{
				HeadingPage = "Изменение категории";

				if(newsCategory != null)
				{
					SelectNewsCategory = newsCategory;

					if (animationName != null && animationDescription != null)
					{
						// Заполняем все поля
						animationName.Text = newsCategory.name;
						animationDescription.Text = newsCategory.description;
						IsAddArchive = newsCategory.isArchived;
					}
				}
				else
				{
					// Возврат на страницу "Категории"
					HamburgerMenuEvent.OpenPageNewsCategory();
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
						// Переход на страницу "Учетные записи"
						HamburgerMenuEvent.OpenPageNewsCategory();
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка сохранения новых или изменения старых данных в UI
		/// </summary>
		private RelayCommand? _saveData { get; set; }
		public RelayCommand? SaveData
		{
			get
			{
				return _saveData ??
					(_saveData = new RelayCommand(async (obj) =>
					{
						if (animationName != null && animationDescription != null)
						{
							// Проверка обязательных полей
							if (!string.IsNullOrWhiteSpace(animationName.Text))
							{
								if (isAddData) // Добавление данных
								{
									if (await CheckingUsernameUniqueness())
									{
										NewsCategory newsCategory = new NewsCategory();
										newsCategory.name = animationName.Text.Trim();
										newsCategory.description = animationDescription.Text.Trim();
										newsCategory.isArchived = IsAddArchive;

										await _newsCategoryService.AddNewsCategoryAsync(newsCategory);
										// Возврат на страницу "Категории"
										HamburgerMenuEvent.OpenPageNewsCategory();
										WorkingWithDataEvent.DataWasAddedSuccessfullyNewsCategory(); // Уведомление об успешно добавлении данных
									}
									else
									{
										StartFieldIllumination(animationName); // Подсветка поля
										systemMessage.Text = "Категория уже существует.";
										systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
										// Исчезание сообщения
										BeginFadeAnimation(systemMessage);
										BeginFadeAnimation(systemMessageBorder);
									}
								}
								else // Редактирование данных
								{
									if (await CheckingUsernameUniqueness())
									{
										NewsCategory newsCategory = new NewsCategory();
										newsCategory.id = SelectNewsCategory.id;	
										newsCategory.name = animationName.Text.Trim();
										newsCategory.description = animationDescription.Text.Trim();
										newsCategory.isArchived = IsAddArchive;

										await _newsCategoryService.UpdateNewsCategoryAsync(newsCategory);
										// Возврат на страницу "Категории"
										HamburgerMenuEvent.OpenPageNewsCategory();
										WorkingWithDataEvent.DataWasChangedSuccessfullyNewsCategory(); // Уведомление об успешном обновлении данных
									}
									else
									{
										StartFieldIllumination(animationName); // Подсветка поля
										systemMessage.Text = "Категория уже существует.";
										systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
										// Исчезание сообщения
										BeginFadeAnimation(systemMessage);
										BeginFadeAnimation(systemMessageBorder);
									}
								}
							}
							else
							{
								systemMessage.Text = "Заполните все обязательные поля.";
								systemMessageBorder.Visibility = System.Windows.Visibility.Visible;

								if (string.IsNullOrWhiteSpace(animationName.Text))
								{
									StartFieldIllumination(animationName); // Подсветка полей
								}

								// Исчезание сообщения
								BeginFadeAnimation(systemMessage);
								BeginFadeAnimation(systemMessageBorder);
							}
						}
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Проверка названия на уникальность
		/// </summary>
		public async Task<bool> CheckingUsernameUniqueness()
		{
			IEnumerable<NewsCategory> newsCategories = await _newsCategoryService.GetAllNewsCategoriesAsync();
			if(newsCategories != null)
			{
				if (isAddData) // При добавлении данных данных
				{
					return !newsCategories.Any(a => a.name.ToLowerInvariant().Contains(animationName.Text.ToLowerInvariant()));
				}
				else // При редактировании данных
				{
					return !newsCategories.Where(a => !a.name.Equals(SelectNewsCategory.name, StringComparison.OrdinalIgnoreCase)) // Получение списка с исключенным текущим значением
						.Any(a => a.name.ToLowerInvariant().Contains(animationName.Text.ToLowerInvariant())); // Поиск в полученном списке совпадения
				}
			}
			return true;
		}

		#endregion


		#region Features

		/// <summary>
		/// Асинхронно получаем информацию из привязанного View
		/// </summary>
		public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters, TextBox name,
			TextBox Description)
		{
			systemMessage = adminViewModelParameters.errorInputText;
			systemMessageBorder = adminViewModelParameters.errorInputBorder;
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			animationName = name;
			animationDescription = Description;
		}

		/// <summary>
		/// Название категории
		/// </summary>
		public TextBox? animationName { get; set; }

		/// <summary>
		/// Описание категории
		/// </summary>
		public TextBox? animationDescription { get; set; }

		/// <summary>
		/// Архивирование категории
		/// </summary>
		private bool _isAddArchive { get; set; }
		public bool IsAddArchive
		{
			get { return _isAddArchive; }
			set { _isAddArchive = value; OnPropertyChanged(nameof(IsAddArchive)); }
		}

		/// <summary>
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
		///  Переданная категория для изменения
		/// </summary>
		private NewsCategory SelectNewsCategory { get; set; }

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
