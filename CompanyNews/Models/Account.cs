using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models
{
    /// <summary>
    /// Учетная запись
    /// </summary>
    public class Account
    {
        public int id {  get; set; }
        [Required(ErrorMessage = "Логин обязателен для заполнения!")]
        public string login { get; set; }
        public string password { get; set; }

        /// <summary>
        /// Роль пользователя в системе (Администратор, Редактор, Пользователь)
        /// </summary>
        public string accountRole { get; set; }

        /// <summary>
        /// Внешний ключ для связи с отделом в котором работает сотрудник
        /// </summary>
        public int? workDepartmentId { get; set; } 
        public string phoneNumber { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string? patronymic { get; set; }

		/// <summary>
		/// Фото человека
		/// </summary>
		public byte[]? image { get; set; }

		/// <summary>
		/// Заблокирован профиль у пользователя?
		/// </summary>
		public bool isProfileBlocked { get; set; }
        
        /// <summary>
        /// Причина блокировки аккаунта
        /// </summary>
        public string? reasonBlockingAccount {  get; set; }

        /// <summary>
        /// Можно оставлять комментарии пользователю?
        /// </summary>
        public bool isCanLeaveComments {  get; set; }

        /// <summary>
        /// Причина запрета комментирования постов
        /// </summary>
        public string? reasonBlockingMessages { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с отделом в котором работает сотрудник
        /// </summary>
        public WorkDepartment workDepartment { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с привязанными категориями у пользователя
        /// </summary>
        public ICollection<AvailableCategoriesUser> availableCategoriesUsers { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с отправленными сообщениями пользователей
        /// </summary>
        public ICollection<MessageUser> messageUsers { get; set; }
    }
}
