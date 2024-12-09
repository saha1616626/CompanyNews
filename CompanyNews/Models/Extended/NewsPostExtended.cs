using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
	/// <summary>
	/// Расширенная модель новостного поста. Идентификаторы в базовой 
	/// модели заменены на соответствующие значения
	/// </summary>
	public class NewsPostExtended
    {
		public int id { get; set; }
		public int newsCategoryId { get; set; }
		[Required(ErrorMessage = "Категория обязательна для выбора!")]
		public string newsCategoryName { get; set; }
		public DateTime datePublication { get; set; }

		/// <summary>
		/// Фото к посту
		/// </summary>
		public byte[]? image { get; set; }

		/// <summary>
		/// Тело сообщения поста
		/// </summary>
		public string? message { get; set; }
	}
}
