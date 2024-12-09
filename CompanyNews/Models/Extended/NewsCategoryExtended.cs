using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
	/// <summary>
	/// Класс содержит инормацию о категории и о пользовательской категории
	/// </summary>
	public class NewsCategoryExtended : NewsCategory
	{
		public int availableCategoriesUserExtendedId { get; set; }
	}
}
