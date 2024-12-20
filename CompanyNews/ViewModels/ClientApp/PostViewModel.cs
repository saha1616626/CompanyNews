using CompanyNews.Helpers;
using CompanyNews.Helpers.Event;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using CompanyNews.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CompanyNews.ViewModels.ClientApp
{
	/// <summary>
	/// Работа пользователя с постом
	/// </summary>
	public class PostViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервис для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly AuthorizationService _authorizationService;
		private readonly NewsPostService _newsPostService;
		private readonly MessageUserService _messageUserService;

		/// <summary>
		/// Сообщения
		/// </summary>
		private ObservableCollection<MessageUserExtended>? _ListMessageUserExtendeds { get; set; }
		public ObservableCollection<MessageUserExtended>? ListMessageUserExtendeds
		{
			get { return _ListMessageUserExtendeds; }
			set { _ListMessageUserExtendeds = value; OnPropertyChanged(nameof(ListMessageUserExtendeds)); }
		}

		public PostViewModel()
		{
			_authorizationService = ServiceLocator.GetService<AuthorizationService>();
			_newsPostService = ServiceLocator.GetService<NewsPostService>();
			_messageUserService = ServiceLocator.GetService<MessageUserService>();
			ListMessageUserExtendeds = new ObservableCollection<MessageUserExtended>();
			LoadMessage();
			IsSendMessage = false;
		}

		#region WorkingWithPages

		/// <summary>
		/// Первоначальная настройка страницы
		/// </summary>
		public async Task InitialPageSetup(NewsPostExtended newsPostExtended)
		{
			NewMessagesFirstSelected = true;
			OldMessagesFirstSelected = false;

			// Устанавливаем свойства для поста в UI
			if (newsPostExtended.datePublication != null)
			{
				DatePublication = newsPostExtended.datePublication;
			}
			if (newsPostExtended.message != null)
			{
				Message = newsPostExtended.message;
			}
			if (newsPostExtended.image != null)
			{
				Image = newsPostExtended.image;
			}

			ListMessageUserExtendeds.Clear(); // Очистка списка перед заполнением
			Account account = await _authorizationService.GetUserAccount(); // Получаем пользователя

			if (account != null)
			{
				if (account.isCanLeaveComments)
				{
					// Если профиль заблокирован
					IsSendMessageBox = false;
					IsBlockMessageBox = true;
				}
				else
				{
					IsSendMessageBox = true;
					IsBlockMessageBox = false;
				}
			}

			// Загружаем пост и сообщения (сообщения сначала новые)
			ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
			if (messagesNewsPostExtendeds.Any())
			{
				newsPostExtendedSelected = newsPostExtended;

				foreach (var post in messagesNewsPostExtendeds)
				{
					if (post.NewsPostExtended.id == newsPostExtended.id)
					{
						if (post.MessageUserExtendeds != null)
						{
							foreach (var message in post.MessageUserExtendeds.Reverse())
							{
								if (message.status == "На проверке" || message.status == "Одобрено")
								{
									if (account != null)
									{
										if (message.accountId == account.id)
										{
											message.IsMessageRight = true; // Пользовательское сообщение находится справа
											ListMessageUserExtendeds.Add(message);
										}
										else
										{
											ListMessageUserExtendeds.Add(message);
										}
									}
									else
									{
										ListMessageUserExtendeds.Add(message);
									}
								}
							}
						}
					}
				}
			}

			// Если нет комментариев, отображаем табличку
			if (ListMessageUserExtendeds.Count == 0)
			{
				IsNoComments = true;
				IsVisibleListBox = false; // Скрываем фильтрацию
			}
			else
			{
				IsNoComments = false;
				IsVisibleListBox = true;
			}
		}

		/// <summary>
		/// Список сообщений
		/// </summary>
		public async Task LoadMessage()
		{
			// Сначала новые сообщения
			if (NewMessagesFirstSelected)
			{
				ListMessageUserExtendeds.Clear(); // Очистка списка перед заполнением
				Account account = await _authorizationService.GetUserAccount(); // Получаем пользователя

				// Загружаем пост и сообщения (сообщения сначала новые)
				ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
				if (messagesNewsPostExtendeds.Any())
				{
					foreach (var post in messagesNewsPostExtendeds)
					{
						if (post.NewsPostExtended.id == newsPostExtendedSelected.id)
						{
							if (post.MessageUserExtendeds != null)
							{
								foreach (var message in post.MessageUserExtendeds.Reverse())
								{
									if (message.status == "На проверке" || message.status == "Одобрено")
									{
										if (account != null)
										{
											if (message.accountId == account.id)
											{
												message.IsMessageRight = true; // Пользовательское сообщение находится справа
												ListMessageUserExtendeds.Add(message);
											}
											else
											{
												ListMessageUserExtendeds.Add(message);
											}
										}
										else
										{
											ListMessageUserExtendeds.Add(message);
										}
									}

								}
							}
						}
					}
				}
			}

			// Сначала старые сообщения
			if (OldMessagesFirstSelected)
			{
				ListMessageUserExtendeds.Clear(); // Очистка списка перед заполнением
				Account account = await _authorizationService.GetUserAccount(); // Получаем пользователя

				// Загружаем пост и сообщения (сообщения сначала новые)
				ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
				if (messagesNewsPostExtendeds.Any())
				{
					foreach (var post in messagesNewsPostExtendeds)
					{
						if (post.NewsPostExtended.id == newsPostExtendedSelected.id)
						{
							if (post.MessageUserExtendeds != null)
							{
								foreach (var message in post.MessageUserExtendeds)
								{
									if (message.status == "На проверке" || message.status == "Одобрено")
									{
										if (account != null)
										{
											if (message.accountId == account.id)
											{
												message.IsMessageRight = true; // Пользовательское сообщение находится справа
												ListMessageUserExtendeds.Add(message);
											}
											else
											{
												ListMessageUserExtendeds.Add(message);
											}
										}
										else
										{
											ListMessageUserExtendeds.Add(message);
										}
									}


								}
							}
						}
					}
				}
			}

		}

		#endregion

		#region CRUD Operations

		/// <summary>
		/// Выход из поста
		/// </summary>
		private RelayCommand _returnHome { get; set; }
		public RelayCommand ReturnHome
		{
			get
			{
				return _returnHome ??
					(_returnHome = new RelayCommand(async (obj) =>
					{
						ClientAppEvent.ExitPost();
					}, (obj) => true));
			}
		}

		/// <summary>
		/// Ввод текста в поле
		/// </summary>
		public void TextMessage(string text)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				IsSendMessage = true; // Включаем кнопку для отправки, если не пустое поле
			}
			else
			{
				IsSendMessage = false;
			}
		}

		/// <summary>
		/// Отправить сообщение
		/// </summary>
		private RelayCommand _sendMessage { get; set; }
		public RelayCommand SendMessage
		{
			get
			{
				return _sendMessage ??
					(_sendMessage = new RelayCommand(async (obj) =>
					{
						if (MessageUser.Trim() != "")
						{
							Account account = await _authorizationService.GetUserAccount();

							if (account != null)
							{
								if (newsPostExtendedSelected != null)
								{
									MessageUser messageUser = new MessageUser();
									messageUser.datePublication = DateTime.Now;
									messageUser.newsPostId = newsPostExtendedSelected.id;
									messageUser.accountId = account.id;
									messageUser.message = MessageUser.Trim();
									messageUser.status = "На проверке";

									messageUser = _messageUserService.AddMessageUserAsync(messageUser);

									MessageUser = "";
									IsNoComments = false;
									IsVisibleListBox = true;

									if (messageUser != null)
									{
										MessageUserExtended messageUserExtendet = new MessageUserExtended();
										messageUserExtendet.id = messageUser.id;
										messageUserExtendet.datePublication = messageUser.datePublication;
										messageUserExtendet.newsPostId = messageUser.newsPostId;
										messageUserExtendet.accountId = messageUser.accountId;
										messageUserExtendet.Account = account;
										messageUserExtendet.message = messageUser.message;
										messageUserExtendet.status = messageUser.status;
										if (messageUser.dateModeration != null) { messageUserExtendet.dateModeration = messageUser.dateModeration; }
										if (messageUser.rejectionReason != null) { messageUserExtendet.rejectionReason = messageUser.rejectionReason; }
										messageUserExtendet.IsMessageRight = true;

										// Добавляем новый элемент в начало списка
										//ListMessageUserExtendeds.Insert(0, messageUserExtendet);
										//ListMessageUserExtendeds.Add(messageUserExtendet);
										await Task.Delay(500);
										LoadMessage();
									}
								}
							}
						}
					}, (obj) => true));
			}
		}


		#endregion

		#region Features

		/// <summary>
		/// Сообщение о блокировки
		/// </summary>
		private bool _isBlockMessageBox { get; set; }
		public bool IsBlockMessageBox
		{
			get { return _isBlockMessageBox; }
			set
			{
				_isBlockMessageBox = value; OnPropertyChanged(nameof(IsBlockMessageBox));
			}
		}

		/// <summary>
		/// Видимость поля для отправки сообщения
		/// </summary>
		private bool _isSendMessageBox { get; set; }
		public bool IsSendMessageBox
		{
			get { return _isSendMessageBox; }
			set
			{
				_isSendMessageBox = value; OnPropertyChanged(nameof(IsSendMessageBox));
			}
		}

		/// <summary>
		/// Сообщение
		/// </summary>
		private string _messageUser { get; set; }

		public string MessageUser
		{
			get { return _messageUser; }
			set
			{
				_messageUser = value;
				OnPropertyChanged(nameof(MessageUser));
			}
		}


		/// <summary>
		/// Видимость кнопки отправить сообщение
		/// </summary>
		private bool _isSendMessage { get; set; }

		public bool IsSendMessage
		{
			get { return _isSendMessage; }
			set
			{
				_isSendMessage = value;
				OnPropertyChanged(nameof(IsSendMessage));
			}
		}

		// Выбранный пост
		public NewsPostExtended newsPostExtendedSelected = new NewsPostExtended();

		/// <summary>
		/// Видимость селектора фильтра
		/// </summary>
		private bool _isVisibleListBox { get; set; }
		public bool IsVisibleListBox
		{
			get { return _isVisibleListBox; }
			set { _isVisibleListBox = value; OnPropertyChanged(nameof(IsVisibleListBox)); LoadMessage(); }
		}

		/// <summary>
		/// Сначала старые сообщения
		/// </summary>
		private bool _oldMessagesFirstSelected { get; set; }
		public bool OldMessagesFirstSelected
		{
			get { return _oldMessagesFirstSelected; }
			set { _oldMessagesFirstSelected = value; OnPropertyChanged(nameof(OldMessagesFirstSelected)); LoadMessage(); }
		}

		/// <summary>
		/// Сначала новые сообщения
		/// </summary>
		private bool _newMessagesFirstSelected { get; set; }
		public bool NewMessagesFirstSelected
		{
			get { return _newMessagesFirstSelected; }
			set { _newMessagesFirstSelected = value; OnPropertyChanged(nameof(NewMessagesFirstSelected)); LoadMessage(); }
		}

		/// <summary>
		/// Если данные не найдены
		/// </summary>
		private bool _isNoComments { get; set; }
		public bool IsNoComments
		{
			get { return _isNoComments; }
			set { _isNoComments = value; OnPropertyChanged(nameof(IsNoComments)); }
		}

		/// <summary>
		/// Дата публикации
		/// </summary>
		private DateTime _datePublication { get; set; }
		public DateTime DatePublication
		{
			get { return _datePublication; }
			set { _datePublication = value; OnPropertyChanged(nameof(DatePublication)); }
		}

		/// <summary>
		/// Сообщение
		/// </summary>
		private string _message { get; set; }
		public string Message
		{
			get { return _message; }
			set { _message = value; OnPropertyChanged(nameof(Message)); }
		}

		/// <summary>
		/// Сообщение
		/// </summary>
		private CroppedBitmap _image { get; set; }
		public CroppedBitmap Image
		{
			get { return _image; }
			set { _image = value; OnPropertyChanged(nameof(Image)); }
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
