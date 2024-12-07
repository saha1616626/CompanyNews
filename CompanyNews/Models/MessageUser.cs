using System;
using System.Collections.Generic;
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
        public string rejectionReason { get; set; }

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
