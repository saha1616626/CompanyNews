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
	public class AvailableCategoriesUserViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly AvailableCategoriesUserService _availableCategoriesUserService;

		/// <summary>
		/// Отображаемый список доступных категорий пользователей в UI
		/// </summary>
		public ObservableCollection<AvailableCategoriesUserExtended> ListAvailableCategoriesUserExtendeds;

		public AvailableCategoriesUserViewModel()
		{
			_availableCategoriesUserService = ServiceLocator.GetService<AvailableCategoriesUserService>();
			ListAvailableCategoriesUserExtendeds = new ObservableCollection<AvailableCategoriesUserExtended>();
			LoadAvailableCategoriesUser(); // Выводим список на экран
		}

		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех доступных категорий у каждого пользователя в UI.
		/// </summary>
		private async Task LoadAvailableCategoriesUser()
		{
			var availableCategoriesUsers = await _availableCategoriesUserService.GetAvailableCategoriesUserExtendedAsync();
			foreach (var availableCategoriesUser in availableCategoriesUsers)
			{
				ListAvailableCategoriesUserExtendeds.Add(availableCategoriesUser);
			}
		}

		/// <summary>
		/// Добавить категорию из списка доступных
		/// </summary>
		public async Task AddAvailableCategoriesUserAsync(AvailableCategoriesUser availableCategoriesUser)
		{
			var addedAccount = await _availableCategoriesUserService.AddAvailableCategoriesUserAsync(availableCategoriesUser); // Добавление в БД + возврат обновленного объекта
			ListAvailableCategoriesUserExtendeds.Add(await _availableCategoriesUserService.AvailableCategoriesUserConvert(addedAccount)); // Обновление коллекции
		}

		/// <summary>
		/// Удалить категорию из списка доступных
		/// </summary>
		public async Task DeleteAvailableCategoriesUserAsync(AvailableCategoriesUser availableCategoriesUser)
		{
			await _availableCategoriesUserService.DeleteAvailableCategoriesUserAsync(availableCategoriesUser.id); // Удаление из БД

			// Находим категорию в списке для отображения в UI и удаляем объект
			AvailableCategoriesUserExtended? availableCategoriesUserExtended = ListAvailableCategoriesUserExtendeds.FirstOrDefault(a => a.id == availableCategoriesUser.id);
			if (availableCategoriesUserExtended != null) { ListAvailableCategoriesUserExtendeds.Remove(availableCategoriesUserExtended); }
		}

		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Кнопка "добавить" категорию в UI
		/// </summary>
		private RelayCommand _addAvailableCategoriesUser { get; set; }
		public RelayCommand AddAvailableCategoriesUser
		{
			get
			{
				return _addAvailableCategoriesUser ??
					(_addAvailableCategoriesUser = new RelayCommand(async (obj) =>
					{
						isAddData = true;

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "изменить" категорию в UI. (Изменить категории у конкретного пользователя. Массово)
		/// </summary>
		private RelayCommand _editAvailableCategoriesUser { get; set; }
		public RelayCommand EditAvailableCategoriesUser
		{
			get
			{
				return _editAvailableCategoriesUser ??
					(_editAvailableCategoriesUser = new RelayCommand(async (obj) =>
					{
						isAddData = false;


					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "удалить" категорию в UI
		/// </summary>
		private RelayCommand _deleteAvailableCategoriesUser { get; set; }
		public RelayCommand DeleteAvailableCategoriesUser
		{
			get
			{
				return _deleteAvailableCategoriesUser ??
					(_deleteAvailableCategoriesUser = new RelayCommand(async (obj) =>
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
		/// Выбранная доступная категория пользователя в UI
		/// </summary>
		private AvailableCategoriesUserExtended _selectedAvailableCategoriesUser { get; set; }
		public AvailableCategoriesUserExtended SelectedAvailableCategoriesUser
		{
			get { return _selectedAvailableCategoriesUser; }
			set
			{
				_selectedAvailableCategoriesUser = value; OnPropertyChanged(nameof(SelectedAvailableCategoriesUser));
				OnPropertyChanged(nameof(IsWorkButtonEnable));
			}
		}

		/// <summary>
		/// Отображение кнопки «удалить» и «редактировать» в UI.
		/// </summary>
		private bool _isWorkButtonEnable { get; set; }
		public bool IsWorkButtonEnable
		{
			get { return SelectedAvailableCategoriesUser != null; } // Если в таблице выбранн объект, то кнопки доступны
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
