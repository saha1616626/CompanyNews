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

namespace CompanyNews.ViewModels.AdminApp
{
	public class MessageUserViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly NewsPostService _newsPostService;
		private readonly MessageUserService _messageUserServiceService;
		private readonly NewsCategoryService _newsCategoryService;

		/// <summary>
		/// Отображаемый список учетных записей в UI
		/// </summary>
		private ObservableCollection<MessagesNewsPostExtended> _listMessagesNewsPostExtendeds;
		public ObservableCollection<MessagesNewsPostExtended> ListMessagesNewsPostExtendeds
		{
			get { return _listMessagesNewsPostExtendeds; }
			set { _listMessagesNewsPostExtendeds = value; OnPropertyChanged(nameof(ListMessagesNewsPostExtendeds)); }
		}

		public MessageUserViewModel()
		{
			_newsPostService = ServiceLocator.GetService<NewsPostService>();
			_messageUserServiceService = ServiceLocator.GetService<MessageUserService>();
			_newsCategoryService = ServiceLocator.GetService<NewsCategoryService>();
			ListMessagesNewsPostExtendeds = new ObservableCollection<MessagesNewsPostExtended>();
			SettingUpPage(); // Первоначальная настройка страницы
			LoadMessageUser(); // Выводим список на экран
		}

		#region CRUD Operations

		/// <summary>
		/// Вывод списка всех постов и их сообщений в UI.
		/// </summary>
		private async Task LoadMessageUser()
		{
			// Вывод всех сообщений
			if (FullListSelected)
			{
				if(SelectedCategory == null)
				{
					ListMessagesNewsPostExtendeds.Clear(); // Чистка коллекции перед заполнением
					ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
					if(messagesNewsPostExtendeds != null)
					{
						foreach (var newsPost in messagesNewsPostExtendeds)
						{
							if (newsPost.MessageUserExtendeds != null && newsPost.MessageUserExtendeds.Count > 0)
							{
								MessagesNewsPostExtended messagesNewsPostExtended = new MessagesNewsPostExtended();
								messagesNewsPostExtended.NewsPostExtended = newsPost.NewsPostExtended;
								messagesNewsPostExtended.MessageUserExtendeds = newsPost.MessageUserExtendeds;
								ListMessagesNewsPostExtendeds.Add(messagesNewsPostExtended);
							}
						}
					}
				}
				else
				{
					ListMessagesNewsPostExtendeds.Clear(); // Чистка коллекции перед заполнением
					ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
					if (messagesNewsPostExtendeds != null)
					{
						foreach(var newsPost in messagesNewsPostExtendeds)
						{
							if(newsPost.MessageUserExtendeds != null && newsPost.MessageUserExtendeds.Count > 0 && newsPost.NewsPostExtended.newsCategoryId == SelectedCategory.id)
							{
								MessagesNewsPostExtended messagesNewsPostExtended = new MessagesNewsPostExtended();
								messagesNewsPostExtended.NewsPostExtended = newsPost.NewsPostExtended;

								ObservableCollection<MessageUserExtended> messageUserExtendeds = new ObservableCollection<MessageUserExtended>();
								foreach(var meesage in newsPost.MessageUserExtendeds)
								{
									messageUserExtendeds.Add(meesage);
								}
								messagesNewsPostExtended.MessageUserExtendeds = messageUserExtendeds;
								ListMessagesNewsPostExtendeds.Add(messagesNewsPostExtended);
							}
						}
					}
				}
			}

			// Ожидают проверки
			if (OnVerificationListSelected)
			{
				if (SelectedCategory == null)
				{
					ListMessagesNewsPostExtendeds.Clear(); // Чистка коллекции перед заполнением
					ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
					if (messagesNewsPostExtendeds != null)
					{
						foreach (var newsPost in messagesNewsPostExtendeds)
						{
							if (newsPost.MessageUserExtendeds != null && newsPost.MessageUserExtendeds.Count > 0)
							{
								MessagesNewsPostExtended messagesNewsPostExtended = new MessagesNewsPostExtended();
								messagesNewsPostExtended.NewsPostExtended = newsPost.NewsPostExtended;

								ObservableCollection<MessageUserExtended> messageUserExtendeds = new ObservableCollection<MessageUserExtended>();
								foreach (var meesage in newsPost.MessageUserExtendeds)
								{
									if(meesage.status == "На проверке")
									{
										messageUserExtendeds.Add(meesage);
									}
								}
								if(messageUserExtendeds.Count > 0)
								{
									messagesNewsPostExtended.MessageUserExtendeds = messageUserExtendeds;
									ListMessagesNewsPostExtendeds.Add(messagesNewsPostExtended);
								}
							}
						}
					}
				}
				else
				{
					ListMessagesNewsPostExtendeds.Clear(); // Чистка коллекции перед заполнением
					ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
					if (messagesNewsPostExtendeds != null)
					{
						foreach (var newsPost in messagesNewsPostExtendeds)
						{
							if (newsPost.MessageUserExtendeds != null && newsPost.MessageUserExtendeds.Count > 0 && newsPost.NewsPostExtended.newsCategoryId == SelectedCategory.id)
							{
								MessagesNewsPostExtended messagesNewsPostExtended = new MessagesNewsPostExtended();
								messagesNewsPostExtended.NewsPostExtended = newsPost.NewsPostExtended;

								ObservableCollection<MessageUserExtended> messageUserExtendeds = new ObservableCollection<MessageUserExtended>();
								foreach (var meesage in newsPost.MessageUserExtendeds)
								{
									if (meesage.status == "На проверке")
									{
										messageUserExtendeds.Add(meesage);
									}
								}
								if (messageUserExtendeds.Count > 0)
								{
									messagesNewsPostExtended.MessageUserExtendeds = messageUserExtendeds;
									ListMessagesNewsPostExtendeds.Add(messagesNewsPostExtended);
								}
							}
						}
					}
				}
			}

			// Проверенные
			if (VerifiedListSelected)
			{
				if (SelectedCategory == null)
				{
					ListMessagesNewsPostExtendeds.Clear(); // Чистка коллекции перед заполнением
					ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
					if (messagesNewsPostExtendeds != null)
					{
						foreach (var newsPost in messagesNewsPostExtendeds)
						{
							if (newsPost.MessageUserExtendeds != null && newsPost.MessageUserExtendeds.Count > 0)
							{
								MessagesNewsPostExtended messagesNewsPostExtended = new MessagesNewsPostExtended();
								messagesNewsPostExtended.NewsPostExtended = newsPost.NewsPostExtended;

								ObservableCollection<MessageUserExtended> messageUserExtendeds = new ObservableCollection<MessageUserExtended>();
								foreach (var meesage in newsPost.MessageUserExtendeds)
								{
									if (meesage.status == "Одобрено")
									{
										messageUserExtendeds.Add(meesage);
									}
								}
								if (messageUserExtendeds.Count > 0)
								{
									messagesNewsPostExtended.MessageUserExtendeds = messageUserExtendeds;
									ListMessagesNewsPostExtendeds.Add(messagesNewsPostExtended);
								}
							}
						}
					}
				}
				else
				{
					ListMessagesNewsPostExtendeds.Clear(); // Чистка коллекции перед заполнением
					ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
					if (messagesNewsPostExtendeds != null)
					{
						foreach (var newsPost in messagesNewsPostExtendeds)
						{
							if (newsPost.MessageUserExtendeds != null && newsPost.MessageUserExtendeds.Count > 0 && newsPost.NewsPostExtended.newsCategoryId == SelectedCategory.id)
							{
								MessagesNewsPostExtended messagesNewsPostExtended = new MessagesNewsPostExtended();
								messagesNewsPostExtended.NewsPostExtended = newsPost.NewsPostExtended;

								ObservableCollection<MessageUserExtended> messageUserExtendeds = new ObservableCollection<MessageUserExtended>();
								foreach (var meesage in newsPost.MessageUserExtendeds)
								{
									if (meesage.status == "Одобрено")
									{
										messageUserExtendeds.Add(meesage);
									}
								}
								if (messageUserExtendeds.Count > 0)
								{
									messagesNewsPostExtended.MessageUserExtendeds = messageUserExtendeds;
									ListMessagesNewsPostExtendeds.Add(messagesNewsPostExtended);
								}
							}
						}
					}
				}
			}

			// Отклоненные
			if (RejectedListSelected)
			{
				if (SelectedCategory == null)
				{
					ListMessagesNewsPostExtendeds.Clear(); // Чистка коллекции перед заполнением
					ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
					if (messagesNewsPostExtendeds != null)
					{
						foreach (var newsPost in messagesNewsPostExtendeds)
						{
							if (newsPost.MessageUserExtendeds != null && newsPost.MessageUserExtendeds.Count > 0)
							{
								MessagesNewsPostExtended messagesNewsPostExtended = new MessagesNewsPostExtended();
								messagesNewsPostExtended.NewsPostExtended = newsPost.NewsPostExtended;

								ObservableCollection<MessageUserExtended> messageUserExtendeds = new ObservableCollection<MessageUserExtended>();
								foreach (var meesage in newsPost.MessageUserExtendeds)
								{
									if (meesage.status == "Отклонено")
									{
										messageUserExtendeds.Add(meesage);
									}
								}
								if (messageUserExtendeds.Count > 0)
								{
									messagesNewsPostExtended.MessageUserExtendeds = messageUserExtendeds;
									ListMessagesNewsPostExtendeds.Add(messagesNewsPostExtended);
								}
							}
						}
					}
				}
				else
				{
					ListMessagesNewsPostExtendeds.Clear(); // Чистка коллекции перед заполнением
					ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
					if (messagesNewsPostExtendeds != null)
					{
						foreach (var newsPost in messagesNewsPostExtendeds)
						{
							if (newsPost.MessageUserExtendeds != null && newsPost.MessageUserExtendeds.Count > 0 && newsPost.NewsPostExtended.newsCategoryId == SelectedCategory.id)
							{
								MessagesNewsPostExtended messagesNewsPostExtended = new MessagesNewsPostExtended();
								messagesNewsPostExtended.NewsPostExtended = newsPost.NewsPostExtended;

								ObservableCollection<MessageUserExtended> messageUserExtendeds = new ObservableCollection<MessageUserExtended>();
								foreach (var meesage in newsPost.MessageUserExtendeds)
								{
									if (meesage.status == "Отклонено")
									{
										messageUserExtendeds.Add(meesage);
									}
								}
								if (messageUserExtendeds.Count > 0)
								{
									messagesNewsPostExtended.MessageUserExtendeds = messageUserExtendeds;
									ListMessagesNewsPostExtendeds.Add(messagesNewsPostExtended);
								}
							}
						}
					}
				}
			}

		}

		/// <summary>
		/// Добавить сообщение
		/// </summary>
		public async Task AddMessageUserAsync(MessageUser messageUser)
		{
			//var addedMessageUser = await _messageUserServiceService.AddMessageUserAsync(messageUser); // Добавление в БД + возврат обновленного объекта
			//ListMessagesNewsPostExtendeds.Add(await _messageUserServiceService.MessageUserConvert(addedMessageUser)); // Обновление коллекции
		}

		/// <summary>
		/// Изменить сообщение
		/// </summary>
		public async Task UpdateMessageUserAsync(MessageUser messageUser)
		{
			//await _messageUserServiceService.UpdateMessageUserAsync(messageUser); // Обновление данных в БД

			//// Находим сообщение в списке для отображения в UI и заменяем объект
			//MessageUserExtended? messageUserExtended = ListMessagesNewsPostExtendeds.FirstOrDefault(a => a.id == messageUser.id);
			//if (messageUserExtended != null) { messageUserExtended = await _messageUserServiceService.MessageUserConvert(messageUser); }
		}

		/// <summary>
		/// Удалить сообщение
		/// </summary>
		public async Task DeleteMessageUserAsync(MessageUser messageUser)
		{
			//await _messageUserServiceService.DeleteMessageUserAsync(messageUser.id); // Удаление из БД

			//// Находим сообщение в списке для отображения в UI и удаляем объект
			//MessageUserExtended? messageUserExtended = ListMessagesNewsPostExtendeds.FirstOrDefault(a => a.id == messageUser.id);
			//if (messageUserExtended != null) { ListMessagesNewsPostExtendeds.Remove(messageUserExtended); }
		}

		#endregion

		#region UI RelayCommand Operations

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async void SettingUpPage()
		{
			FullListSelected = true; // Список всех значений по умолчанию. Все остальные false
			OnVerificationListSelected = false;
			VerifiedListSelected = false;
			RejectedListSelected = false;
			DarkBackground = Visibility.Collapsed; // Скрываем фон для Popup

			// Получаем список категорий для фильтрации
			ListCategory = new List<NewsCategory>(await _newsCategoryService.GetAllNewsCategoriesAsync());
		}

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

		public bool IsBlockAccount { get; set; }
		public bool IsUnlockAccount { get; set; }
		public bool IsApproveMessage { get; set; }
		public bool IsRejectMessage { get; set; }
		public bool IsRestoreMessage { get; set; }
		public bool IsDeleteMessage { get; set; }

		/// <summary>
		/// Подтверждение блокировки пользователя.
		/// </summary>
		public void CheckBlockAccount(MessageUserExtended messageUserExtended)
		{
			IsBlockAccount = true;
			IsUnlockAccount = false;
			IsApproveMessage = false;
			IsRejectMessage = false;
			IsRestoreMessage = false;
			IsDeleteMessage = false;
		}

		/// <summary>
		/// Блокировка пользователя (писать сообщения)
		/// </summary>
		public void BlockAccount(MessageUserExtended messageUserExtended)
		{
			// Ищем пользователя во всех сообщениях, чтобы изменить кнопку
			foreach(var post in ListMessagesNewsPostExtendeds)
			{
				// В посте просматриваем сообщения
				foreach(var mes in post.MessageUserExtendeds)
				{
					if(mes.Account.id == messageUserExtended.Account.id)
					{
						mes.IsBlockAccount = false; // Кнопка заблокировать скрыта
						mes.IsUnlockAccount = true; // Кнопка разблокиовать видна
					}
				}
			}
		}

		/// <summary>
		/// Подтверждение разблокировки пользователя.
		/// </summary>
		public void CheckUnlockAccount(MessageUserExtended messageUserExtended)
		{
			IsBlockAccount = false;
			IsUnlockAccount = true;
			IsApproveMessage = false;
			IsRejectMessage = false;
			IsRestoreMessage = false;
			IsDeleteMessage = false;
		}

		/// <summary>
		/// Разблокировать пользователя (писать сообщения)
		/// </summary>
		public void UnlockAccount(MessageUserExtended messageUserExtended)
		{
			// Ищем пользователя во всех сообщениях, чтобы изменить кнопку
			foreach (var post in ListMessagesNewsPostExtendeds)
			{
				// В посте просматриваем сообщения
				foreach (var mes in post.MessageUserExtendeds)
				{
					if (mes.Account.id == messageUserExtended.Account.id)
					{
						mes.IsBlockAccount = true; // Кнопка заблокировать видна
						mes.IsUnlockAccount = false; // Кнопка разблокиовать скрыта
					}
				}
			}
		}

		/// <summary>
		/// Подтверждение одобрения сообщения.
		/// </summary>
		public void CheckApproveMessage(MessageUserExtended messageUserExtended)
		{
			IsBlockAccount = false;
			IsUnlockAccount = false;
			IsApproveMessage = true;
			IsRejectMessage = false;
			IsRestoreMessage = false;
			IsDeleteMessage = false;
		}

		/// <summary>
		/// Одобрить сообщение пользователя
		/// </summary>
		public void ApproveMessage(MessageUserExtended messageUserExtended)
		{
			// Ищем нужный пост
			MessagesNewsPostExtended messagesNewsPostExtended = ListMessagesNewsPostExtendeds
				.FirstOrDefault(post => post.NewsPostExtended.id == messageUserExtended.newsPostId);

			if (messagesNewsPostExtended != null)
			{
				// Ищем нужное сообщение
				MessageUserExtended messageUser = messagesNewsPostExtended.MessageUserExtendeds.FirstOrDefault(message => message.id == messageUserExtended.id);

				if (messageUser != null)
				{
					messageUserExtended.IsApproveMessage = false; // Кнопка одобрить сообщение скрыта
					messageUserExtended.IsRejectMessage = true; // Кнопка отклонить сообщение видна
					messageUserExtended.IsRestoreMessage = true; // Кнопка восстановить сообщение после отклонения видна
					messageUserExtended.status = "Одобрено";
				}
			}
		}

		/// <summary>
		/// Подтверждение отклонения сообщения.
		/// </summary>
		public void CheckRejectMessage(MessageUserExtended messageUserExtended)
		{
			IsBlockAccount = false;
			IsUnlockAccount = false;
			IsApproveMessage = false;
			IsRejectMessage = true;
			IsRestoreMessage = false;
			IsDeleteMessage = false;
		}

		/// <summary>
		/// Отклонить сообщение пользователя
		/// </summary>
		public void RejectMessage(MessageUserExtended messageUserExtended)
		{
			// Ищем нужный пост
			MessagesNewsPostExtended messagesNewsPostExtended = ListMessagesNewsPostExtendeds
				.FirstOrDefault(post => post.NewsPostExtended.id == messageUserExtended.newsPostId);

			if (messagesNewsPostExtended != null)
			{
				// Ищем нужное сообщение
				MessageUserExtended messageUser = messagesNewsPostExtended.MessageUserExtendeds.FirstOrDefault(message => message.id == messageUserExtended.id);

				if (messageUser != null)
				{
					messageUserExtended.IsApproveMessage = true; // Кнопка одобрить сообщение видна
					messageUserExtended.IsRejectMessage = false; // Кнопка отклонить сообщение скрыта
					messageUserExtended.IsRestoreMessage = true; // Кнопка восстановить сообщение после отклонения видна
					messageUserExtended.status = "Отклонено";
				}
			}
		}

		/// <summary>
		/// Подтверждение восстановления сообщения.
		/// </summary>
		public void CheckRestoreMessage(MessageUserExtended messageUserExtended)
		{
			IsBlockAccount = false;
			IsUnlockAccount = false;
			IsApproveMessage = false;
			IsRejectMessage = false;
			IsRestoreMessage = true;
			IsDeleteMessage = false;
		}

		/// <summary>
		/// Восстановить сообщение пользователя
		/// </summary>
		public void RestoreMessage(MessageUserExtended messageUserExtended)
		{
			// Ищем нужный пост
			MessagesNewsPostExtended messagesNewsPostExtended = ListMessagesNewsPostExtendeds
				.FirstOrDefault(post => post.NewsPostExtended.id == messageUserExtended.newsPostId);

			if (messagesNewsPostExtended != null)
			{
				// Ищем нужное сообщение
				MessageUserExtended messageUser = messagesNewsPostExtended.MessageUserExtendeds.FirstOrDefault(message => message.id == messageUserExtended.id);

				if (messageUser != null)
				{
					messageUserExtended.IsApproveMessage = true; // Кнопка одобрить сообщение видна
					messageUserExtended.IsRejectMessage = true; // Кнопка отклонить сообщение видна
					messageUserExtended.IsRestoreMessage = false; // Кнопка восстановить сообщение после отклонения скрыта
					messageUserExtended.status = "На проверке";
				}
			}
		}

		/// <summary>
		/// Подтверждение удаления сообщения.
		/// </summary>
		public void CheckDeleteMessage(MessageUserExtended messageUserExtended)
		{
			IsBlockAccount = false;
			IsUnlockAccount = false;
			IsApproveMessage = false;
			IsRejectMessage = false;
			IsRestoreMessage = false;
			IsDeleteMessage = true;
		}

		/// <summary>
		/// Удалить сообщение пользователя
		/// </summary>
		public void DeleteMessage(MessageUserExtended messageUserExtended)
		{
			// Ищем нужный пост
			MessagesNewsPostExtended messagesNewsPostExtended = ListMessagesNewsPostExtendeds
				.FirstOrDefault(post => post.NewsPostExtended.id == messageUserExtended.newsPostId);

			if (messagesNewsPostExtended != null)
			{
				// Ищем нужное сообщение
				MessageUserExtended messageUser = messagesNewsPostExtended.MessageUserExtendeds.FirstOrDefault(message => message.id == messageUserExtended.id);

				if (messageUser != null)
				{
					messagesNewsPostExtended.MessageUserExtendeds.Remove(messageUser);
					messageUserExtended.IsDeleteMessage = false; // Кнопка удалить сообщение выключена
				}
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
			// Закрываем Popup
			StartPoup = false;
			DarkBackground = Visibility.Collapsed; // Скрываем фон
		}

		#region FeaturesPopup

		/// <summary>
		/// Popup для работы с данны
		/// </summary>
		private bool _startPoup { get; set; }
		public bool StartPoup
		{
			get { return _startPoup; }
			set
			{
				_startPoup = value;
				OnPropertyChanged(nameof(StartPoup));
			}
		}

		/// <summary>
		/// Данные передаются в Popup, как предпросмотр перед удалением
		/// </summary>
		private string _actionsWithData { get; set; }
		public string ActionsWithData
		{
			get { return _actionsWithData; }
			set { _actionsWithData = value; OnPropertyChanged(nameof(ActionsWithData)); }
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

		#region ListSelected

		/// <summary>
		/// Выбранная категория
		/// </summary>
		private NewsCategory _selectedCategory { get; set; }
		public NewsCategory SelectedCategory
		{
			get { return _selectedCategory; }
			set { _selectedCategory = value; OnPropertyChanged(nameof(SelectedCategory)); LoadMessageUser(); }
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
		/// Выбран список всех сообщений в UI
		/// </summary>
		private bool _fullListSelected { get; set; }
		public bool FullListSelected
		{
			get { return _fullListSelected; }
			set
			{
				_fullListSelected = value; OnPropertyChanged(nameof(FullListSelected)); 
				LoadMessageUser();
			}
		}

		/// <summary>
		/// Выбран список сообщений на проверке в UI
		/// </summary>
		private bool _onVerificationListSelected { get; set; }
		public bool OnVerificationListSelected
		{
			get { return _onVerificationListSelected; }
			set
			{
				_onVerificationListSelected = value; OnPropertyChanged(nameof(OnVerificationListSelected));
				LoadMessageUser();
			}
		}

		/// <summary>
		/// Выбран список сообщений проверенных в UI
		/// </summary>
		private bool _verifiedListSelected { get; set; }
		public bool VerifiedListSelected
		{
			get { return _verifiedListSelected; }
			set
			{
				_verifiedListSelected = value; OnPropertyChanged(nameof(VerifiedListSelected));
				LoadMessageUser();
			}
		}

		/// <summary>
		/// Выбран список сообщений отклоненных в UI
		/// </summary>
		private bool _rejectedListSelected { get; set; }
		public bool RejectedListSelected
		{
			get { return _rejectedListSelected; }
			set
			{
				_rejectedListSelected = value; OnPropertyChanged(nameof(RejectedListSelected));
				LoadMessageUser();
			}
		}

		#endregion

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

		/// <summary>
		/// Cписок для фильтров таблицы
		/// </summary>
		public ObservableCollection<MessagesNewsPostExtended> ListSearch { get; set; } = new ObservableCollection<MessagesNewsPostExtended>();

		/// <summary>
		/// Поиск сообщения по ключевому слову
		/// </summary>
		public void SearchNewsPost(string searchByValue)
		{
			if (!string.IsNullOrWhiteSpace(searchByValue))
			{
				LoadMessageUser(); // обновляем список
				ListSearch.Clear(); // очищаем список поиска данных

				foreach(var item in ListMessagesNewsPostExtendeds)
				{
					if(item.MessageUserExtendeds != null && item.MessageUserExtendeds.Count > 0)
					{
						MessagesNewsPostExtended messagesNewsPostExtended = new MessagesNewsPostExtended();
						messagesNewsPostExtended.NewsPostExtended = item.NewsPostExtended;

						ObservableCollection<MessageUserExtended> messageUserExtendeds = new ObservableCollection<MessageUserExtended>();

						// Проходимся по списку сообщений
						foreach (var itemMessage in item.MessageUserExtendeds)
						{
							string message = itemMessage.message.ToLower();

							// Если есть совпдаение, то добавляем в список
							if (message.Contains(searchByValue.ToLowerInvariant()))
							{
								messageUserExtendeds.Add(itemMessage);
							}
						}

						if(messageUserExtendeds != null && messageUserExtendeds.Count > 0)
						{
							messagesNewsPostExtended.MessageUserExtendeds = messageUserExtendeds;
							ListSearch.Add(messagesNewsPostExtended);
						}
						else
						{
							continue;
						}
					}
				}

				ListMessagesNewsPostExtendeds.Clear(); // Очистка список перед заполнением
				ListMessagesNewsPostExtendeds = new ObservableCollection<MessagesNewsPostExtended>(ListSearch);  // Обновление списка

				if (ListSearch.Count == 0)
				{
					if (systemMessage != null && systemMessageBorder != null)
					{
						// Оповещениие об отсутствии данных
						systemMessage.Text = $"Сообщение не найдено.";
						systemMessageBorder.Visibility = System.Windows.Visibility.Visible;
						// Исчезание сообщения
						BeginFadeAnimation(systemMessage);
						BeginFadeAnimation(systemMessageBorder);
					}
				}

			}
			else
			{
				ListMessagesNewsPostExtendeds.Clear(); // Очистка список перед заполнением
				LoadMessageUser(); // обновляем список
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
