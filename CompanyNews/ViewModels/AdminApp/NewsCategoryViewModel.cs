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
	public class NewsCategoryViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly NewsCategoryService _newsCategoryService;

		/// <summary>
		/// Отображаемый список категорий в UI
		/// </summary>
		public ObservableCollection<NewsCategory> ListNewsCategories;

		public NewsCategoryViewModel()
		{
			_newsCategoryService = ServiceLocator.GetService<NewsCategoryService>();
			ListNewsCategories = new ObservableCollection<NewsCategory>();
			LoadNewsCategory(); // Выводим список на экран
		}

		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех категорий в UI.
		/// </summary>
		private async Task LoadNewsCategory()
		{
			var newsCategories = await _newsCategoryService.GetAllNewsCategoriesAsync();
			foreach (var newsCategory in newsCategories)
			{
				ListNewsCategories.Add(newsCategory);
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

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка сохранения новых или изменения старых данных категории в UI
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
