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
	public class NewsPostViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly NewsPostService _newsPostService;

		/// <summary>
		/// Отображаемый список постов в UI
		/// </summary>
		public ObservableCollection<NewsPostExtended> ListNewsPostExtendeds;

		public NewsPostViewModel()
		{
			_newsPostService = ServiceLocator.GetService<NewsPostService>();
			ListNewsPostExtendeds = new ObservableCollection<NewsPostExtended>();
			LoadNewsPost(); // Выводим список на экран
		}

		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех постов в UI.
		/// </summary>
		private async Task LoadNewsPost()
		{
			var newsPosts = await _newsPostService.GetAllNewsPostsAsync();
			foreach (var newsPost in newsPosts)
			{
				ListNewsPostExtendeds.Add(newsPost);
			}
		}

		/// <summary>
		/// Добавить пост
		/// </summary>
		public async Task AddAccountAsync(NewsPost newsPost)
		{
			var addedNewsPost = await _newsPostService.AddNewsPostAsync(newsPost); // Добавление в БД + возврат обновленного объекта
			ListNewsPostExtendeds.Add(await _newsPostService.NewsPostConvert(addedNewsPost)); // Обновление коллекции
		}

		/// <summary>
		/// Изменить пост
		/// </summary>
		public async Task UpdateAccountAsync(NewsPost newsPost)
		{
			await _newsPostService.UpdateNewsPostAsync(newsPost); // Обновление данных в БД

			// Находим пост в списке для отображения в UI и заменяем объект
			NewsPostExtended? newsPostExtended = ListNewsPostExtendeds.FirstOrDefault(a => a.id == newsPost.id);
			if (newsPostExtended != null) { newsPostExtended = await _newsPostService.NewsPostConvert(newsPost); }
		}

		/// <summary>
		/// Удалить пост
		/// </summary>
		public async Task DeleteAccountAsync(NewsPost newsPost)
		{
			await _newsPostService.DeleteNewsPostAsync(newsPost.id); // Удаление из БД

			// Находим пост в списке для отображения в UI и удаляем объект
			NewsPostExtended? newsPostExtended = ListNewsPostExtendeds.FirstOrDefault(a => a.id == newsPost.id);
			if (newsPostExtended != null) { ListNewsPostExtendeds.Remove(newsPostExtended); }
		}

		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Кнопка "добавить" пост в UI
		/// </summary>
		private RelayCommand _addNewsPost { get; set; }
		public RelayCommand AddNewsPost
		{
			get
			{
				return _addNewsPost ??
					(_addNewsPost = new RelayCommand(async (obj) =>
					{
						isAddData = true;

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "изменить" пост в UI
		/// </summary>
		private RelayCommand _editNewsPost { get; set; }
		public RelayCommand EditNewsPost
		{
			get
			{
				return _editNewsPost ??
					(_editNewsPost = new RelayCommand(async (obj) =>
					{
						isAddData = false;


					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "удалить" пост в UI
		/// </summary>
		private RelayCommand _deleteNewsPost { get; set; }
		public RelayCommand DeleteNewsPost
		{
			get
			{
				return _deleteNewsPost ??
					(_deleteNewsPost = new RelayCommand(async (obj) =>
					{

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка сохранения новых или изменения старых данных поста в UI
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
