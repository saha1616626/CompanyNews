﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models
{
    /// <summary>
    /// Сообщение пользователя к посту
    /// </summary>
    public class MessageUser
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
		[Required(ErrorMessage = "Пользователь обязателен для выбора!")]
		public int accountId { get; set; }

		/// <summary>
		/// Тело сообщения пользователя
		/// </summary>
		[Required(ErrorMessage = "Сообщение обязательно для ввода!")]
		public string message { get; set; }

        /// <summary>
        /// Статус модерации сообщения (На проверке и одобрено)
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

        /// <summary>
        /// Навигационное свойство для связи с постом
        /// </summary>
        public NewsPost newsPost { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с аккаунтом от которого было отправлено сообщение
        /// </summary>
        public Account account { get; set; }
    }
}
