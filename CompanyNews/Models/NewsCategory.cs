using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models
{
    /// <summary>
    /// Категория новости в сообществе
    /// </summary>
    public class NewsCategory
    {
        public int id { get; set; }
		[Required(ErrorMessage = "Название обязательно для заполнения!")]
		public string name { get; set; }
        public string? description { get; set; }
        /// <summary>
        /// Категория в архиве?
        /// </summary>
        public bool isArchived { get; set; }

		/// <summary>
		/// Навигационное свойство для связи с категориями доступными пользователям
		/// </summary>
		public ICollection<NewsCategoriesWorkDepartment> newsCategoriesWorkDepartments { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с постами
        /// </summary>
        public ICollection<NewsPost> newsPosts { get; set; }
    }
}
