using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
    public class MessageUserExtendet
    {
		public int id { get; set; }
		public DateTime datePublication { get; set; }

		/// <summary>
		/// Пост для которого было написано сообщение
		/// </summary>
		public int newsPostId { get; set; }

		/// <summary>
		/// Отправитель сообщения
		/// </summary>
		public int accountId { get; set; }

		/// <summary>
		/// Тело сообщения пользователя
		/// </summary>
		public string message { get; set; }

		/// <summary>
		/// Статус модерации сообщения (Модерация, Одобрено и Отклонено)
		/// </summary>
		public string status { get; set; }

		/// <summary>
		/// Причина отклонения прохождения модерации
		/// </summary>
		public string? rejectionReason { get; set; }
	}
}
