using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
    public class MessageUserExtended
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
		public string status { get; set; }

		/// <summary>
		/// Дата и время модерации
		/// </summary>
		public DateTime? dateModeration { get; set; }

		/// <summary>
		/// Причина отклонения прохождения модерации
		/// </summary>
		public string? rejectionReason { get; set; }
	}
}
