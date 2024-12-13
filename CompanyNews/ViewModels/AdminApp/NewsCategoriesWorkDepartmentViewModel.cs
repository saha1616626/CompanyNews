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

namespace CompanyNews.ViewModels.AdminApp
{
	public class NewsCategoriesWorkDepartmentViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly NewsCategoriesWorkDepartmentService _newsCategoriesWorkDepartmentService;

		/// <summary>
		/// Отображаемый список доступных категорий рабочего отдела в UI
		/// </summary>
		public ObservableCollection<NewsCategoriesWorkDepartmentExtended> ListNewsCategoriesWorkDepartmentExtendeds;

		public NewsCategoriesWorkDepartmentViewModel()
		{
			_newsCategoriesWorkDepartmentService = ServiceLocator.GetService<NewsCategoriesWorkDepartmentService>();
			ListNewsCategoriesWorkDepartmentExtendeds = new ObservableCollection<NewsCategoriesWorkDepartmentExtended>();
			LoadAvailableCategoriesUser(); // Выводим список на экран
		}

		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех доступных категорий у каждого рабочего отдела в UI.
		/// </summary>
		private async Task LoadAvailableCategoriesUser()
		{
			var newsCategoriesWorkDepartment = await _newsCategoriesWorkDepartmentService.GetNewsCategoriesWorkDepartmentExtendedAsync();
			if(newsCategoriesWorkDepartment != null) // Если категории есть у рабочего отдела, то мы выводим список
			{
				foreach (var availableCategoriesUser in newsCategoriesWorkDepartment)
				{
					ListNewsCategoriesWorkDepartmentExtendeds.Add(availableCategoriesUser);
				}
			}
		}

		/// <summary>
		/// Добавить категорию в список доступных
		/// </summary>
		public async Task AddNewsCategoriesWorkDepartmentAsync(NewsCategoriesWorkDepartment newsCategoriesWorkDepartment)
		{
			var addedCategoty = await _newsCategoriesWorkDepartmentService.AddNewsCategoriesWorkDepartmentAsync(newsCategoriesWorkDepartment); // Добавление в БД + возврат обновленного объекта

			// Получение доступных категорий постов рабочего отдела по идентификатору отдела
			NewsCategoriesWorkDepartmentExtended? newsCategoriesWorkDepartmentExtended = 
				await _newsCategoriesWorkDepartmentService.GetNewsCategoriesWorkDepartmentExtendedByIdAsync(addedCategoty.workDepartmentId);
			if(newsCategoriesWorkDepartmentExtended != null)
			{
				// Находим в списке UI нужный рабочий отдел и обновляем список доступных категорий
				NewsCategoriesWorkDepartmentExtended? newsCategoriesWorkDepartmentExtendedSearch 
					= ListNewsCategoriesWorkDepartmentExtendeds.FirstOrDefault(ncwde => ncwde.workDepartment.id == newsCategoriesWorkDepartmentExtended.workDepartment.id);
				if (newsCategoriesWorkDepartmentExtendedSearch != null)
				{
					newsCategoriesWorkDepartmentExtendedSearch.categories = newsCategoriesWorkDepartmentExtended.categories; // Обновление коллекции
				}
			}
		}

		/// <summary>
		/// Удалить категорию из списка доступных
		/// </summary>
		public async Task DeleteNewsCategoriesWorkDepartmentAsync(NewsCategoriesWorkDepartment newsCategoriesWorkDepartment)
		{
			await _newsCategoriesWorkDepartmentService.DeleteNewsCategoriesWorkDepartmentAsync(newsCategoriesWorkDepartment.id); // Удаление из БД

			// Находим в списке UI нужный рабочий отдел и обновляем список доступных категорий
			NewsCategoriesWorkDepartmentExtended? newsCategoriesWorkDepartmentExtendedSearch
				= ListNewsCategoriesWorkDepartmentExtendeds.FirstOrDefault(ncwde => ncwde.workDepartment.id == newsCategoriesWorkDepartment.workDepartmentId);
			if (newsCategoriesWorkDepartmentExtendedSearch != null)
			{
				NewsCategoriesWorkDepartmentExtended? newsCategoriesWorkDepartmentExtended = await _newsCategoriesWorkDepartmentService.GetNewsCategoriesWorkDepartmentExtendedByIdAsync(newsCategoriesWorkDepartment.workDepartmentId);
				if(newsCategoriesWorkDepartmentExtended != null) { newsCategoriesWorkDepartmentExtendedSearch = newsCategoriesWorkDepartmentExtended; } // Обновление коллекции 
			}
		}

		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Кнопка "добавить" категорию в UI
		/// </summary>
		private RelayCommand _addNewsCategoriesWorkDepartment { get; set; }
		public RelayCommand AddNewsCategoriesWorkDepartment
		{
			get
			{
				return _addNewsCategoriesWorkDepartment ??
					(_addNewsCategoriesWorkDepartment = new RelayCommand(async (obj) =>
					{
						isAddData = true;

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "изменить" категорию в UI. (Изменить категории у конкретного рабочего отдела. Массово)
		/// </summary>
		private RelayCommand _editNewsCategoriesWorkDepartment { get; set; }
		public RelayCommand EditNewsCategoriesWorkDepartment
		{
			get
			{
				return _editNewsCategoriesWorkDepartment ??
					(_editNewsCategoriesWorkDepartment = new RelayCommand(async (obj) =>
					{
						isAddData = false;


					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "удалить" категорию в UI
		/// </summary>
		private RelayCommand _deleteNewsCategoriesWorkDepartment { get; set; }
		public RelayCommand DeleteNewsCategoriesWorkDepartment
		{
			get
			{
				return _deleteNewsCategoriesWorkDepartment ??
					(_deleteNewsCategoriesWorkDepartment = new RelayCommand(async (obj) =>
					{

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка сохранения новых или изменения старых данных категорий в UI
		/// </summary>

		private RelayCommand _saveData { get; set; }
		public RelayCommand SaveData
		{
			get
			{
				return _saveData ??
					(_saveData = new RelayCommand(async (obj) =>
					{

						if (isAddData) // Логика при добавлении данных
						{

						}
						else // Логика при редактировании данных
						{

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

		}

		#endregion

		#region Features

		/// <summary>
		/// Асинхронно получаем информацию из привязанного View
		/// </summary>
		public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters)
		{
			darkBackground = adminViewModelParameters.darkBackground;
			fieldIllumination = adminViewModelParameters.fieldIllumination;
			errorInputPopup = adminViewModelParameters.errorInputPopup;
			errorInput = adminViewModelParameters.errorInputText;
			deleteDataPopup = adminViewModelParameters.deleteDataPopup;
		}

		/// <summary>
		/// Выбранная доступная категория рабочего отдела в UI
		/// </summary>
		private NewsCategoriesWorkDepartmentExtended _selectedNewsCategoriesWorkDepartmentExtended { get; set; }
		public NewsCategoriesWorkDepartmentExtended SelectedNewsCategoriesWorkDepartmentExtended
		{
			get { return _selectedNewsCategoriesWorkDepartmentExtended; }
			set
			{
				_selectedNewsCategoriesWorkDepartmentExtended = value; OnPropertyChanged(nameof(SelectedNewsCategoriesWorkDepartmentExtended));
				OnPropertyChanged(nameof(IsWorkButtonEnable));
			}
		}

		/// <summary>
		/// Отображение кнопки «удалить» и «редактировать» в UI.
		/// </summary>
		private bool _isWorkButtonEnable { get; set; }
		public bool IsWorkButtonEnable
		{
			get { return SelectedNewsCategoriesWorkDepartmentExtended != null; } // Если в таблице выбранн объект, то кнопки доступны
			set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
		}

		/// <summary>
		/// Добавление или редактирование данных
		/// </summary>
		/// <remarks>true - добавление данных; false - редактирование данных</remarks>
		private bool isAddData { get; set; }

		#region View

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
		public TextBlock? errorInputPopup { get; set; }

		/// <summary>
		/// Вывод ошибки и анимация текста на странице
		/// </summary>
		public TextBlock? errorInput { get; set; }

		/// <summary>
		/// Popup удаления данных
		/// </summary>
		public Popup? deleteDataPopup { get; set; }

		#endregion

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
