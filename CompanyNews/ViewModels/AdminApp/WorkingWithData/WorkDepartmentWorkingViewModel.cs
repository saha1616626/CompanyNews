using CompanyNews.Helpers;
using CompanyNews.Helpers.Event;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace CompanyNews.ViewModels.AdminApp.WorkingWithData
{
	public class WorkDepartmentWorkingViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервисы для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly NewsCategoryService _newsCategoryService; // Категория
		private readonly WorkDepartmentService _workDepartmentService; // Рабочий отдел
		private readonly NewsCategoriesWorkDepartmentService _newsCategoriesWorkDepartmentService; // Категории рабочего отдела

		/// <summary>
		/// Отображаемый список рабочих отделов в UI для текущего отдела
		/// </summary>
		private ObservableCollection<NewsCategoryExtended> _listNewsCategoryExtended { get; set; }
		public ObservableCollection<NewsCategoryExtended> ListNewsCategoryExtended
		{
			get { return _listNewsCategoryExtended; }
			set { _listNewsCategoryExtended = value; OnPropertyChanged(nameof(ListNewsCategoryExtended)); }
		}

		/// <summary>
		/// Отображаемый список всех доступных категорий, которых нет в текущем списке
		/// </summary>
		private ObservableCollection<NewsCategoryExtended> _listAvailableCategories { get; set; }
		public ObservableCollection<NewsCategoryExtended> ListAvailableCategories
		{
			get { return _listAvailableCategories; }
			set { _listAvailableCategories = value; OnPropertyChanged(nameof(ListAvailableCategories)); }
		}

		public WorkDepartmentWorkingViewModel()
		{
			_newsCategoryService = ServiceLocator.GetService<NewsCategoryService>();
			_workDepartmentService = ServiceLocator.GetService<WorkDepartmentService>();
			_newsCategoriesWorkDepartmentService = ServiceLocator.GetService<NewsCategoriesWorkDepartmentService>();
			ListNewsCategoryExtended = new ObservableCollection<NewsCategoryExtended>();
			ListAvailableCategories = new ObservableCollection<NewsCategoryExtended>();
			SettingUpPage(); // Настройка страницы
		}

		#region WorkingWithPages

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async Task InitialPageSetup(bool IsAddData, WorkDepartment? workDepartment)
		{
			this.isAddData = IsAddData;

			if (IsAddData) // Режим добавления данных
			{
				HeadingPage = "Создание рабочего отдела";
			}
			else
			{
				HeadingPage = "Изменение рабочего отдела";

				if (workDepartment != null)
				{
					SelectWorkDepartment = workDepartment;
					await LoadCategoriesWorkDepartment(); // Выводим категории для данного рабочего отдела

					if (animationName != null && animationDescription != null)
					{
						// Заполняем все поля
						animationName.Text = workDepartment.name;
						animationDescription.Text = workDepartment.description;
					}
				}
				else
				{
					// Возврат на предыдущую страницу
					HamburgerMenuEvent.OpenPageWorkDepartment();
				}
			}
		}

		/// <summary>
		///  Настройка страницы
		/// </summary>
		public async void SettingUpPage()
		{
			DarkBackground = Visibility.Collapsed; // Скрываем фон для Popup
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
						// Переход на страницу "Рабочие отделы"
						HamburgerMenuEvent.OpenPageWorkDepartment();
					}, (obj) => true));
			}
		}

		#endregion

		#region CRUD Operations

		/// <summary>
		/// Удалить категорию из рабочего отдела
		/// </summary>
		public async Task DeleteNewsCategoriesWorkDepartmentAsync(NewsCategoryExtended newsCategoryExtended)
		{
			//await _newsCategoriesWorkDepartmentService.DeleteNewsCategoriesWorkDepartmentAsync(newsCategoryExtended.NewsCategoriesWorkDepartmentExtendedId); // Удаление из БД

			// Находим категорию в списке для отображения в UI и удаляем объект
			NewsCategoryExtended? newsCategoryExtendedSearch = ListNewsCategoryExtended.FirstOrDefault(a => a.id == newsCategoryExtended.id);
			if (newsCategoryExtendedSearch != null) { ListNewsCategoryExtended.Remove(newsCategoryExtendedSearch); }
		}

		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Вывод списка всех категорий рабочего отдела в UI.
		/// </summary>
		private async Task LoadCategoriesWorkDepartment()
		{
			ListNewsCategoryExtended.Clear(); // Чистка коллекции перед заполнением
			var workDepartments = await _newsCategoriesWorkDepartmentService.GetNewsCategoriesWorkDepartmentExtendedAsync();
			foreach (var item in workDepartments)
			{
				if (item.workDepartment.id == SelectWorkDepartment.id)
				{
					ListNewsCategoryExtended = new ObservableCollection<NewsCategoryExtended>(item.categories);
				}
			}
		}

		/// <summary>
		/// Вывод списка всех категорий за исключением выбранных в таблице в UI .
		/// </summary>
		public async Task AvailableCategoriesAdd()
		{
			ListAvailableCategories.Clear(); // Чистка коллекции перед заполнением

			var workDepartments = await _newsCategoryService.GetAllNewsCategoriesAsync();
			if (workDepartments != null)
			{
				foreach (var item in workDepartments)
				{
					// Проверка, что категория не в архиве
					if (!item.isArchived)
					{
						// Если текущей категории нет в списке данного отдела, то мы её добавляем
						if (!ListNewsCategoryExtended.Any(a => a.id == item.id))
						{
							NewsCategoryExtended newsCategoryExtended = new NewsCategoryExtended();
							newsCategoryExtended.id = item.id;
							newsCategoryExtended.name = item.name;
							newsCategoryExtended.description = item.description;
							newsCategoryExtended.IsAddCategory = true;
							newsCategoryExtended.IsDeleteCategory = true;
							newsCategoryExtended.isArchived = item.isArchived;
							ListAvailableCategories.Add(newsCategoryExtended);
						}
					}
				}
			}
		}

		/// <summary>
		/// Добавить категорию в список из popup
		/// </summary>
		public void AddCategory(NewsCategoryExtended newsCategoryExtended)
		{
			// Добавляем категорию в список, где указаны все выбранные категории для данного рабочего отдела
			ListNewsCategoryExtended.Add(newsCategoryExtended);
			// Помечаем в списке Popup, что мы добавили данные
			NewsCategoryExtended categoryExtended = ListAvailableCategories.FirstOrDefault(c => c.id == newsCategoryExtended.id);
			if (categoryExtended != null)
			{
				categoryExtended.IsAddCategory = false; // Помечаем, что категория добавлена
				categoryExtended.IsDeleteCategory = false; // Можно убрать из списка
			}
		}

		/// <summary>
		/// Убрать категорию из списка в popup
		/// </summary>
		public void DeleteCategory(NewsCategoryExtended newsCategoryExtended)
		{
			// Убираем категорию из списка, где указаны все выбранные категории для данного рабочего отдела
			NewsCategoryExtended categoryExtended = ListNewsCategoryExtended.FirstOrDefault(c => c.id == newsCategoryExtended.id);
			if (categoryExtended != null)
			{
				ListNewsCategoryExtended.Remove(categoryExtended);

				// Ищем данную категорию в списке Popup
				NewsCategoryExtended newsCategory = ListAvailableCategories.FirstOrDefault(c => c.id == newsCategoryExtended.id);
				if (newsCategory != null)
				{
					categoryExtended.IsAddCategory = true; // Помечаем, что категория не добавлена
					categoryExtended.IsDeleteCategory = true; // Нельзя убрать из списка
				}
			}
		}

		/// <summary>
		/// Кнопка "удалить" в UI
		/// </summary>
		private RelayCommand _deleteItem { get; set; }
		public RelayCommand DeleteItem
		{
			get
			{
				return _deleteItem ??
					(_deleteItem = new RelayCommand(async (obj) =>
					{
						if (SelectedNewsCategoryExtended != null)
						{
							await DeleteNewsCategoriesWorkDepartmentAsync(SelectedNewsCategoryExtended);

							systemMessage.Text = $"Категория из текущего рабочего отдела удалена. Для подтверждения изменений нажмите кнопку «Сохранить».";
							systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
							// Исчезание сообщения
							BeginFadeAnimation(systemMessage);
							BeginFadeAnimation(systemMessageBorder);
						}
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "Добавить" в UI
		/// </summary>
		private RelayCommand _addItem { get; set; }
		public RelayCommand AddItem
		{
			get
			{
				return _addItem ??
					(_addItem = new RelayCommand(async (obj) =>
					{

						StartPopup = true; // отображаем Popup
						DarkBackground = Visibility.Visible; // показать фон

						// Выводим список категорий
						await AvailableCategoriesAdd();

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
						if (animationName != null && animationDescription != null)
						{
							// Проверка обязательных полей
							if (!string.IsNullOrWhiteSpace(animationName.Text))
							{
								if (isAddData) // Добавление данных
								{
									if (await CheckingWorkDepartamentUniqueness())
									{
										WorkDepartment workDepartment = new WorkDepartment();
										workDepartment.name = animationName.Text.Trim();
										if (animationDescription != null) { workDepartment.description = animationDescription.Text.Trim(); }
										workDepartment = await _workDepartmentService.AddWorkDepartmentAsync(workDepartment);
										if (workDepartment != null)
										{
											foreach (var item in ListNewsCategoryExtended)
											{
												NewsCategoriesWorkDepartment newsCategoriesWorkDepartment = new NewsCategoriesWorkDepartment();
												newsCategoriesWorkDepartment.newsCategoryId = item.id;
												newsCategoriesWorkDepartment.workDepartmentId = workDepartment.id;
												_newsCategoriesWorkDepartmentService.AddNewsCategoriesWorkDepartmentAsync(newsCategoriesWorkDepartment);
											}
										}

										// Возврат на страницу "Рабочие отделы"
										HamburgerMenuEvent.OpenPageWorkDepartment();
										WorkingWithDataEvent.DataWasAddedSuccessfullyWorkDepartment(); // Уведомление об успешно добавлении данных
									}
									else
									{
										StartFieldIllumination(animationName); // Подсветка поля
										systemMessage.Text = "Рабочий отдел уже существует.";
										systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
										// Исчезание сообщения
										BeginFadeAnimation(systemMessage);
										BeginFadeAnimation(systemMessageBorder);
									}
								}
								else // Редактирование данных
								{
									if (await CheckingWorkDepartamentUniqueness())
									{
										WorkDepartment workDepartment = new WorkDepartment();
										workDepartment.id = SelectWorkDepartment.id;
										workDepartment.name = animationName.Text.Trim();
										if (animationDescription != null) { workDepartment.description = animationDescription.Text.Trim(); }
										_workDepartmentService.UpdateWorkDepartmentAsync(workDepartment);

										// Удаляем все категории текущего отдела
										_newsCategoriesWorkDepartmentService.DeleteNewsCategoriesWorkDepartmentAsync(SelectWorkDepartment.id);

										// Перезаписываем категории
										foreach (var item in ListNewsCategoryExtended)
										{
											NewsCategoriesWorkDepartment newsCategoriesWorkDepartment = new NewsCategoriesWorkDepartment();
											newsCategoriesWorkDepartment.newsCategoryId = item.id;
											newsCategoriesWorkDepartment.workDepartmentId = SelectWorkDepartment.id;
											_newsCategoriesWorkDepartmentService.AddNewsCategoriesWorkDepartmentAsync(newsCategoriesWorkDepartment);
										}

										// Возврат на страницу "Рабочие отделы"
										HamburgerMenuEvent.OpenPageWorkDepartment();
										WorkingWithDataEvent.DataWasChangedSuccessfullyWorkDepartment(); // Уведомление об успешном изменении данных
									}
									else
									{
										StartFieldIllumination(animationName); // Подсветка поля
										systemMessage.Text = "Рабочий отдел уже существует.";
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
		public async Task<bool> CheckingWorkDepartamentUniqueness()
		{
			IEnumerable<WorkDepartment> WorkDepartments = await _workDepartmentService.GetAllWorkDepartmentsAsync();
			if (WorkDepartments != null)
			{
				if (isAddData) // При добавлении данных
				{
					// Проверка, что в названии нет 
					return !WorkDepartments.Any(a => a.name.Equals(animationName.Text, StringComparison.OrdinalIgnoreCase)
													  || a.name.Equals(animationName.Text, StringComparison.OrdinalIgnoreCase));
				}
				else // При редактировании данных
				{
					// Получение списка с исключенным текущим значением
					return !WorkDepartments.Where(a => !a.name.Equals(SelectWorkDepartment.name, StringComparison.OrdinalIgnoreCase)) // Исключаем текущее значение
						.Any(a => a.name.Equals(animationName.Text, StringComparison.OrdinalIgnoreCase)
								   || a.name.Equals(animationName.Text, StringComparison.OrdinalIgnoreCase)); // Проверяем на наличие и совпадения
				}

			}
			return true;
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

						systemMessage.Text = $"Для подтверждения внесенных изменений нажмите кнопку «Сохранить».";
						systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
						// Исчезание сообщения
						BeginFadeAnimation(systemMessage);
						BeginFadeAnimation(systemMessageBorder);
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Скрытие popup
		/// </summary>
		private async Task ClosePopupWorkingWithData()
		{
			// Закрываем Popup
			StartPopup = false;
			DarkBackground = Visibility.Collapsed; // Скрываем фон
		}

		#region FeaturesPopup

		/// <summary>
		/// Popup удаления данных
		/// </summary>
		private bool _startPopup { get; set; }
		public bool StartPopup
		{
			get { return _startPopup; }
			set { _startPopup = value; OnPropertyChanged(nameof(StartPopup)); }
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
		public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters, TextBox name,
			TextBox Description, Border PopupMessageBorder, TextBlock PopupMessage)
		{
			systemMessage = adminViewModelParameters.errorInputText;
			systemMessageBorder = adminViewModelParameters.errorInputBorder;
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			animationName = name;
			animationDescription = Description;
			popupMessageBorder = PopupMessageBorder;
			popupMessage = PopupMessage;
		}

		/// <summary>
		/// Вывод сообщения popup и анимация текста на странице
		/// </summary>
		public TextBlock? popupMessage { get; set; }

		/// <summary>
		/// Вывод контейнера для сообщения popup
		/// </summary>
		public Border? popupMessageBorder { get; set; }

		/// <summary>
		/// Затемненный фон позади Popup
		/// </summary>
		public Border? darkBackground { get; set; }

		/// <summary>
		/// Выбранный рабочий отдел в UI
		/// </summary>
		private NewsCategoryExtended _selectedNewsCategoryExtended { get; set; }
		public NewsCategoryExtended SelectedNewsCategoryExtended
		{
			get { return _selectedNewsCategoryExtended; }
			set
			{
				_selectedNewsCategoryExtended = value; OnPropertyChanged(nameof(SelectedNewsCategoryExtended));
				OnPropertyChanged(nameof(IsWorkButtonEnable));
			}
		}

		/// <summary>
		/// Отображение кнопки «удалить» в UI.
		/// </summary>
		private bool _isWorkButtonEnable { get; set; }
		public bool IsWorkButtonEnable
		{
			get { return SelectedNewsCategoryExtended != null; } // Если в таблице выбранн объект, то кнопки доступны
			set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
		}

		/// <summary>
		/// Название
		/// </summary>
		public TextBox? animationName { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public TextBox? animationDescription { get; set; }

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
		///  Переданный рабочий отдел для изменения
		/// </summary>
		private WorkDepartment SelectWorkDepartment { get; set; }

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
				AvailableCategoriesAdd(); // обновляем список
				ListSearch.Clear(); // очищаем список поиска данных

				// Объединяем атрибуты сущности для поиска
				foreach (NewsCategoryExtended item in ListAvailableCategories)
				{
					string description = "";
					if (item.description == null) { description = ""; } else { description = item.description; }

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
						// Оповещениие об отсутствии данных
						popupMessage.Text = $"Категория не найдена.";
						popupMessageBorder.Visibility = System.Windows.Visibility.Visible;
						// Исчезание сообщения
						BeginFadeAnimation(popupMessage);
						BeginFadeAnimation(popupMessageBorder);
					}
				}
			}
			else
			{
				ListAvailableCategories.Clear(); // Очистка список перед заполнением
				AvailableCategoriesAdd(); // обновляем список
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
