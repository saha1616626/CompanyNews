using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
	/// <summary>
	/// Расширенная модель доступных категорий новостей для пользователя. Идентификаторы в базовой 
	/// модели заменены на соответствующие значения
	/// </summary>
	public class AvailableCategoriesUserExtended
	{

		public int id { get; set; }

		/// <summary>
		/// Учетная запись пользователя
		/// </summary>
		public Account account { get; set; }

		/// <summary>
		/// Список категорий у пользователя
		/// </summary>
		public IEnumerable<NewsCategoryExtended>? categories { get; set; }
	}
}
