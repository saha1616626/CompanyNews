using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models
{
    /// <summary>
    /// Доступные категории новостей для пользователя
    /// </summary>
    public class AvailableCategoriesUser
    {
        public int id { get; set; }
        public int accountId { get; set; }
        public int newsCategoryId { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с категорией, которая доступна пользователю
        /// </summary>
        public NewsCategory newsCategory {  get; set; }

        /// <summary>
        /// Навигационное свойство для связи с аккаунтом, который привязан к категории
        /// </summary>
        public Account account { get; set; }
    }
}
