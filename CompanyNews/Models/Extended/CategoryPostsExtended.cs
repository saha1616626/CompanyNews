using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
	/// <summary>
	/// Расширенная модель постов сообщества. Группировка постов по категориям.
	/// </summary>
	public class CategoryPostsExtended : NewsCategory
	{
		/// <summary>
		/// Кол-во подписчиков в категории
		/// </summary>
		public int? numberSubscribers { get; set; }

		/// <summary>
		/// Список постов у текущей категории
		/// </summary>
		public IEnumerable<NewsPost>? newsPosts { get; set; }
	}
}
