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
	public class WorkDepartmentViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly WorkDepartmentService _workDepartmentService;

		/// <summary>
		/// Отображаемый список рабочих отделов в UI
		/// </summary>
		private ObservableCollection<WorkDepartment> _listWorkDepartments { get; set; }
		public ObservableCollection<WorkDepartment> ListWorkDepartments
		{
			get { return _listWorkDepartments; }
			set { _listWorkDepartments = value; OnPropertyChanged(nameof(ListWorkDepartments)); }
		}

		public WorkDepartmentViewModel()
		{
			_workDepartmentService = ServiceLocator.GetService<WorkDepartmentService>();
			ListWorkDepartments = new ObservableCollection<WorkDepartment>();
			SettingUpPage(); // Первоначальная настройка страницы
			LoadWorkDepartment(); // Выводим список на экран

			// Подписываемся на событие — успшное добавление данных.
			WorkingWithDataEvent.dataWasAddedSuccessfullyWorkDepartment += DataWasAddedSuccessfullyWorkDepartment;
			// Подписываемся на событие — успшное изменение данных.
			WorkingWithDataEvent.dataWasChangedSuccessfullyWorkDepartment += DataWasChangedSuccessfullyWorkDepartment;
		}

		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех рабочих отделов в UI.
		/// </summary>
		private async Task LoadWorkDepartment()
		{
			var workDepartments = await _workDepartmentService.GetAllWorkDepartmentsAsync();
			foreach (var account in workDepartments)
			{
				ListWorkDepartments.Add(account);
			}
		}

		/// <summary>
		/// Добавить рабочий отдел
		/// </summary>
		public async Task AddWorkDepartmentAsync(WorkDepartment workDepartment)
		{
			var addedWorkDepartment = await _workDepartmentService.AddWorkDepartmentAsync(workDepartment); // Добавление в БД + возврат обновленного объекта
			ListWorkDepartments.Add(await _workDepartmentService.AddWorkDepartmentAsync(addedWorkDepartment)); // Обновление коллекции
		}

		/// <summary>
		/// Изменить рабочий отдел
		/// </summary>
		public async Task UpdateWorkDepartmentAsync(WorkDepartment workDepartment)
		{
			await _workDepartmentService.UpdateWorkDepartmentAsync(workDepartment); // Обновление данных в БД

			// Находим рабочий отдел в списке для отображения в UI и заменяем объект
			WorkDepartment? workDepartmentSearch = ListWorkDepartments.FirstOrDefault(a => a.id == workDepartment.id);
			if (workDepartmentSearch != null) { workDepartmentSearch = await _workDepartmentService.AddWorkDepartmentAsync(workDepartment); }
		}

		/// <summary>
		/// Удалить рабочий отдел
		/// </summary>
		public async Task DeleteWorkDepartmentAsync(WorkDepartment workDepartment)
		{
			await _workDepartmentService.DeleteWorkDepartmentAsync(workDepartment.id); // Удаление из БД

			// Находим рабочий отдел в списке для отображения в UI и удаляем объект
			WorkDepartment? workDepartmentSearch = ListWorkDepartments.FirstOrDefault(a => a.id == workDepartment.id);
			if (workDepartmentSearch != null) { ListWorkDepartments.Remove(workDepartmentSearch); }
		}

		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async void SettingUpPage()
		{
			DarkBackground = Visibility.Collapsed; // Скрываем фон для Popup
		}

		/// <summary>
		/// Кнопка "добавить" рабочий отдел в UI
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
						PageFrame = new WorkDepartmentWorkingPage(isAddData, SelectedWorkDepartment);
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "изменить" рабочий отдел в UI
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
						PageFrame = new WorkDepartmentWorkingPage(isAddData, SelectedWorkDepartment);
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
						if (SelectedWorkDepartment != null)
						{
							StartPoupDeleteData = true; // отображаем Popup
							DarkBackground = Visibility.Visible; // показать фон

							DataDeleted = $"Название: \"{SelectedWorkDepartment.name}\"";
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
						if (SelectedWorkDepartment != null)
						{
							await DeleteWorkDepartmentAsync(SelectedWorkDepartment);

							if (systemMessage != null && systemMessageBorder != null)
							{
								await ClosePopupWorkingWithData(); // Скрываем Popup
																   // Выводим сообщение об успешном удалении данных
								systemMessage.Text = $"Рабочий отдел успешно удален.";
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
		public async void DataWasAddedSuccessfullyWorkDepartment(object sender, EventAggregator e)
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
		public async void DataWasChangedSuccessfullyWorkDepartment(object sender, EventAggregator e)
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
		/// Выбранный рабочий отдел в UI
		/// </summary>
		private WorkDepartment _selectedWorkDepartment { get; set; }
		public WorkDepartment SelectedWorkDepartment
		{
			get { return _selectedWorkDepartment; }
			set
			{
				_selectedWorkDepartment = value; OnPropertyChanged(nameof(SelectedWorkDepartment));
				OnPropertyChanged(nameof(IsWorkButtonEnable));
			}
		}

		/// <summary>
		/// Отображение кнопки «удалить» и «редактировать» в UI.
		/// </summary>
		private bool _isWorkButtonEnable { get; set; }
		public bool IsWorkButtonEnable
		{
			get { return SelectedWorkDepartment != null; } // Если в таблице выбранн объект, то кнопки доступны
			set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
		}

		/// <summary>
		/// Добавление или редактирование данных
		/// </summary>
		/// <remarks>true - добавление данных; false - редактирование данных</remarks>
		private bool isAddData { get; set; }

		#region View

		// Page для запуска страницы
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
		public ObservableCollection<WorkDepartment> ListSearch { get; set; } = new ObservableCollection<WorkDepartment>();

		/// <summary>
		/// Поиск данных в таблицы через строку запроса
		/// </summary>
		public void SearchWorkDepartment(string searchByValue)
		{
			if (!string.IsNullOrWhiteSpace(searchByValue))
			{
				LoadWorkDepartment(); // обновляем список
				ListSearch.Clear(); // очищаем список поиска данных

				// Объединяем атрибуты сущности для поиска
				foreach (WorkDepartment item in ListWorkDepartments)
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

				ListWorkDepartments.Clear(); // Очистка список перед заполнением
				ListWorkDepartments = new ObservableCollection<WorkDepartment>(ListSearch); // Обновление списка

				if (ListSearch.Count == 0)
				{
					if (systemMessage != null && systemMessageBorder != null)
					{
						// Оповещениие об отсутствии данных
						systemMessage.Text = $"Рабочий отдел не найден.";
						systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
						// Исчезание сообщения
						BeginFadeAnimation(systemMessage);
						BeginFadeAnimation(systemMessageBorder);
					}
				}
			}
			else
			{
				ListWorkDepartments.Clear(); // Очистка список перед заполнением
				LoadWorkDepartment(); // обновляем список
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
