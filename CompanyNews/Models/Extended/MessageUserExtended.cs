using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
    public class MessageUserExtended : INotifyPropertyChanged
	{
		public int id { get; set; }
		public DateTime datePublication { get; set; }

		/// <summary>
		/// Пост для которого было написано сообщение
		/// </summary>
		[Required(ErrorMessage = "Пост обязателен для выбора!")]
		public int newsPostId { get; set; }

		/// <summary>
		/// Отправитель сообщения
		/// </summary>
		public int accountId { get; set; }

		[Required(ErrorMessage = "Пользователь обязателен для выбора!")]
		public Account Account { get; set; }

		/// <summary>
		/// Тело сообщения пользователя
		/// </summary>
		[Required(ErrorMessage = "Сообщение обязательно для ввода!")]
		public string message { get; set; }

		/// <summary>
		/// Статус модерации сообщения (Модерация, Одобрено и Отклонено)
		/// </summary>
		private string _status { get; set; }
		public string status
		{
			get { return  _status; }
			set { _status = value; OnPropertyChanged(nameof(status)); }
		}
		/// <summary>
		/// Дата и время модерации
		/// </summary>
		public DateTime? dateModeration { get; set; }

		/// <summary>
		/// Причина отклонения прохождения модерации
		/// </summary>
		private string? _rejectionReason { get; set; }
		public string? rejectionReason
		{
			get { return _rejectionReason; }
			set { _rejectionReason = value; OnPropertyChanged(nameof(rejectionReason)); }
		}

		#region Button

		/// <summary>
		/// Кнопка заблокировать аккаунт
		/// </summary>
		private bool? _isBlockAccount { get; set; }
		public bool? IsBlockAccount
		{
			get { return _isBlockAccount; }
			set { _isBlockAccount = value; OnPropertyChanged(nameof(IsBlockAccount)); }
		}

		/// <summary>
		/// Кнопка разблокировать аккаунт
		/// </summary>
		private bool? _isUnlockAccount { get; set; }
		public bool? IsUnlockAccount
		{
			get { return _isUnlockAccount; }
			set { _isUnlockAccount = value; OnPropertyChanged(nameof(IsUnlockAccount)); }
		}

		/// <summary>
		/// Кнопка одобрить сообщение
		/// </summary>
		private bool? _isApproveMessage { get; set; }
		public bool? IsApproveMessage
		{
			get { return _isApproveMessage; }
			set { _isApproveMessage = value; OnPropertyChanged(nameof(IsApproveMessage)); }
		}

		/// <summary>
		/// Кнопка отклонить сообщение
		/// </summary>
		private bool? _isRejectMessage { get; set; }
		public bool? IsRejectMessage
		{
			get { return _isRejectMessage; }
			set { _isRejectMessage = value; OnPropertyChanged(nameof(IsRejectMessage)); }
		}

		/// <summary>
		/// Кнопка восстановить сообщение после отклонения
		/// </summary>
		private bool? _isRestoreMessage { get; set; }
		public bool? IsRestoreMessage
		{
			get { return _isRestoreMessage; }
			set { _isRestoreMessage = value; OnPropertyChanged(nameof(IsRestoreMessage)); }
		}

		/// <summary>
		/// Кнопка удалить сообщение
		/// </summary>
		private bool? _isDeleteMessage { get; set; }
		public bool? IsDeleteMessage
		{
			get { return _isDeleteMessage; }
			set { _isDeleteMessage = value; OnPropertyChanged(nameof(IsDeleteMessage)); }
		}

		#endregion

		/// <summary>
		/// Пользовательское сообщение помечается
		/// </summary>
		private bool? _isMessageRight { get; set; }
		public bool? IsMessageRight
		{
			get { return _isMessageRight; }
			set { _isMessageRight = value; OnPropertyChanged(nameof(IsMessageRight)); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
