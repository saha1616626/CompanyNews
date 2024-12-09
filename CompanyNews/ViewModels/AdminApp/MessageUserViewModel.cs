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
	public class MessageUserViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly MessageUserService _messageUserServiceService;

		/// <summary>
		/// Отображаемый список учетных записей в UI
		/// </summary>
		public ObservableCollection<MessageUserExtended> ListMessageUserExtendeds;

		public MessageUserViewModel()
		{
			_messageUserServiceService = ServiceLocator.GetService<MessageUserService>();
			ListMessageUserExtendeds = new ObservableCollection<MessageUserExtended>();
			LoadMessageUser(); // Выводим список на экран
		}

		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех сообщений в UI.
		/// </summary>
		private async Task LoadMessageUser()
		{
			var messageUsers = await _messageUserServiceService.GetAllMessageUserAsync();
			foreach (var messageUser in messageUsers)
			{
				ListMessageUserExtendeds.Add(messageUser);
			}
		}

		/// <summary>
		/// Добавить сообщение
		/// </summary>
		public async Task AddMessageUserAsync(MessageUser messageUser)
		{
			var addedMessageUser = await _messageUserServiceService.AddMessageUserAsync(messageUser); // Добавление в БД + возврат обновленного объекта
			ListMessageUserExtendeds.Add(await _messageUserServiceService.MessageUserConvert(addedMessageUser)); // Обновление коллекции
		}

		/// <summary>
		/// Изменить сообщение
		/// </summary>
		public async Task UpdateMessageUserAsync(MessageUser messageUser)
		{
			await _messageUserServiceService.UpdateMessageUserAsync(messageUser); // Обновление данных в БД

			// Находим сообщение в списке для отображения в UI и заменяем объект
			MessageUserExtended? messageUserExtended = ListMessageUserExtendeds.FirstOrDefault(a => a.id == messageUser.id);
			if (messageUserExtended != null) { messageUserExtended = await _messageUserServiceService.MessageUserConvert(messageUser); }
		}

		/// <summary>
		/// Удалить сообщение
		/// </summary>
		public async Task DeleteMessageUserAsync(MessageUser messageUser)
		{
			await _messageUserServiceService.DeleteMessageUserAsync(messageUser.id); // Удаление из БД

			// Находим сообщение в списке для отображения в UI и удаляем объект
			MessageUserExtended? messageUserExtended = ListMessageUserExtendeds.FirstOrDefault(a => a.id == messageUser.id);
			if (messageUserExtended != null) { ListMessageUserExtendeds.Remove(messageUserExtended); }
		}

		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Кнопка "добавить" сообщение в UI
		/// </summary>
		private RelayCommand _addMessageUser { get; set; }
		public RelayCommand AddMessageUser
		{
			get
			{
				return _addMessageUser ??
					(_addMessageUser = new RelayCommand(async (obj) =>
					{
						isAddData = true;

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "изменить" сообщение в UI
		/// </summary>
		private RelayCommand _editMessageUser { get; set; }
		public RelayCommand EditMessageUser
		{
			get
			{
				return _editMessageUser ??
					(_editMessageUser = new RelayCommand(async (obj) =>
					{
						isAddData = false;


					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка "удалить" сообщение в UI
		/// </summary>
		private RelayCommand _deleteMessageUser { get; set; }
		public RelayCommand DeleteMessageUser
		{
			get
			{
				return _deleteMessageUser ??
					(_deleteMessageUser = new RelayCommand(async (obj) =>
					{

					}, (obj) => true));
			}
		}

		/// <summary>
		/// Кнопка сохранения новых или изменения старых данных сообщения в UI
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
		/// Выбранное сообщение в UI
		/// </summary>
		private MessageUser _selectedMessageUser { get; set; }
		public MessageUser SelectedMessageUser
		{
			get { return _selectedMessageUser; }
			set { _selectedMessageUser = value; OnPropertyChanged(nameof(SelectedMessageUser));
				OnPropertyChanged(nameof(IsWorkButtonEnable));
			}
		}

		/// <summary>
		/// Отображение кнопки «удалить» и «редактировать» в UI.
		/// </summary>
		private bool _isWorkButtonEnable { get; set; }
		public bool IsWorkButtonEnable
		{
			get { return SelectedMessageUser != null; } // Если в таблице выбранн объект, то кнопки доступны
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
