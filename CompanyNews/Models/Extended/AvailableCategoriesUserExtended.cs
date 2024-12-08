﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
	class AvailableCategoriesUserExtended
	{
		/// <summary>
		/// Расширенная модель доступных категорий новостей для пользователя. Идентификаторы в базовой 
		/// модели заменены на соответствующие значения
		/// </summary>

		public int id { get; set; }
		public int accountId { get; set; }
		public int newsCategoryId { get; set; }
		public string newsCategoryName { get; set; }
	}
}