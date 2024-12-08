using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models
{ 
	/// <summary>
	/// Новостной пост
	/// </summary>
	public class NewsPost
    {
        public int id { get; set; }
        public int newsCategoryId { get; set; }
        public DateTime datePublication { get; set; }

        /// <summary>
        /// Фото к посту
        /// </summary>
        public byte[]? image { get; set; }
        
        /// <summary>
        /// Тело сообщения поста
        /// </summary>
        public string? message { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с категорией
        /// </summary>
        public NewsCategory newsCategory { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с отправленными сообщениями
        /// </summary>
        public ICollection<MessageUser> messageUsers { get; set; }
    }
}
