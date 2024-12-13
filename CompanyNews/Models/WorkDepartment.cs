using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models
{
    /// <summary>
    /// Рабочий отдел
    /// </summary>
    public class WorkDepartment
    {
        public int id { get; set; }
		[Required(ErrorMessage = "Рабочий отдел обязателен для ввода!")]
		public string name { get; set; }
        public string? description { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с аккаунтом
        /// </summary>
        public ICollection<Account> accounts { get; set; }

		/// <summary>
		/// Навигационное свойство для связи с привязанными категориями у рабочего отдела
		/// </summary>
		public ICollection<NewsCategoriesWorkDepartment> newsCategoriesWorkDepartments { get; set; }
	}
}
