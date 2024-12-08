using System;
using System.Collections.Generic;
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
        public string name { get; set; }
        public string? description { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с аккаунтом
        /// </summary>
        public ICollection<Account> accounts { get; set; }
    }
}
