using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models
{
    /// <summary>
    /// Доступные категории новостей для рабочего отдела
    /// </summary>
    public class NewsCategoriesWorkDepartment
    {
        public int id { get; set; }
        public int workDepartmentId { get; set; }
        public int newsCategoryId { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с категорией, которая доступна пользователю
        /// </summary>
        public NewsCategory newsCategory {  get; set; }

        /// <summary>
        /// Навигационное свойство для связи с рабочим отделом, которое привязано к категории
        /// </summary>
        public WorkDepartment workDepartment { get; set; }
    }
}
