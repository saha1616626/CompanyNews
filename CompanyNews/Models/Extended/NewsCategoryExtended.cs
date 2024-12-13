using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
	/// <summary>
	/// Класс содержит идентификатор доступной категории у рабочего отдела
	/// </summary>
	public class NewsCategoryExtended : NewsCategory
	{
		/// <summary>
		/// Идентификатор категории для определенного рабочего отдела (NewsCategoriesWorkDepartment)
		/// </summary>
		public int NewsCategoriesWorkDepartmentExtendedId { get; set; }
	}
}
