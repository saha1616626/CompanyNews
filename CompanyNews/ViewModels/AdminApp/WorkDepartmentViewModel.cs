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
	public class WorkDepartmentViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly WorkDepartmentService _workDepartmentService;

		/// <summary>
		/// Отображаемый список рабочих отделов в UI
		/// </summary>
		public ObservableCollection<WorkDepartment> ListWorkDepartments;

		public WorkDepartmentViewModel()
		{
			_workDepartmentService = ServiceLocator.GetService<WorkDepartmentService>();
			ListWorkDepartments = new ObservableCollection<WorkDepartment>();
			LoadWorkDepartment(); // Выводим список на экран
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
		/// Кнопка "добавить" рабочий отдел в UI
		/// </summary>
		private RelayCommand _addWorkDepartment { get; set; }
		public RelayCommand AddWorkDepartment
		{
			get
			{
				return _addWorkDepartment ??
					(_addWorkDepartment = new RelayCommand(async (obj) =>
					{
						isAddData = true;

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "изменить" рабочий отдел в UI
		/// </summary>
		private RelayCommand _editWorkDepartment { get; set; }
		public RelayCommand EditWorkDepartment
		{
			get
			{
				return _editWorkDepartment ??
					(_editWorkDepartment = new RelayCommand(async (obj) =>
					{
						isAddData = false;


					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "удалить" рабочий отдел в UI
		/// </summary>
		private RelayCommand _deleteWorkDepartment { get; set; }
		public RelayCommand DeleteWorkDepartment
		{
			get
			{
				return _deleteWorkDepartment ??
					(_deleteWorkDepartment = new RelayCommand(async (obj) =>
					{

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка сохранения новых или изменения старых данных рабочего отдела в UI
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
			errorInput = adminViewModelParameters.errorInput;
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
