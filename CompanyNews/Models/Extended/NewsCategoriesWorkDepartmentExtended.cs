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
	public class NewsCategoriesWorkDepartmentExtended
	{

		/// <summary>
		/// Рабочий депарамент
		/// </summary>
		public WorkDepartment workDepartment { get; set; }

		/// <summary>
		/// Список категорий у рабочего отдела
		/// </summary>
		public IEnumerable<NewsCategoryExtended>? categories { get; set; }
	}
}
