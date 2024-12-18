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
			ListMessageUserExtendeds = new ObservableCollection<MessageUserExtended>();
			LoadMessage();

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
			if(newsPostExtended.message != null)
			{
				Message = newsPostExtended.message;
			}
			if(newsPostExtended.image != null)
			{
				Image = newsPostExtended.image;
			}

			ListMessageUserExtendeds.Clear(); // Очистка списка перед заполнением
			Account account = await _authorizationService.GetUserAccount(); // Получаем пользователя

			// Загружаем пост и сообщения (сообщения сначала новые)
			ObservableCollection<MessagesNewsPostExtended> messagesNewsPostExtendeds = await _newsPostService.GettingPostsWithMessages();
			if (messagesNewsPostExtendeds.Any())
			{
				newsPostExtendedSelected = newsPostExtended;

				foreach (var post in messagesNewsPostExtendeds)
				{
					if(post.NewsPostExtended.id == newsPostExtended.id)
					{
						if(post.MessageUserExtendeds != null)
						{
							foreach(var message in post.MessageUserExtendeds.Reverse())
							{
								if(account != null)
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

			// Если нет комментариев, отображаем табличку
			if(ListMessageUserExtendeds.Count == 0)
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

		#endregion

		#region Features

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
		private bool _isNoComments {  get; set; }
		public bool IsNoComments
		{
			get { return _isNoComments; }
			set { _isNoComments = value;  OnPropertyChanged(nameof(IsNoComments)); }
		}

		/// <summary>
		/// Дата публикации
		/// </summary>
		private DateTime _datePublication {  get; set; }
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
