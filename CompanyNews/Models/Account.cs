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
		[Required(ErrorMessage = "Пароль обязателен для заполнения!")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль должен содержать не менее 8 символов!")]
		public string password { get; set; }

        /// <summary>
        /// Роль пользователя в системе (Администратор, Редактор, Пользователь)
        /// </summary>
        [Required(ErrorMessage = "Роль обязательна для заполнения!")]
        public string accountRole { get; set; }

		/// <summary>
		/// Внешний ключ для связи с отделом в котором работает сотрудник
		/// </summary>
		[Required(ErrorMessage = "Рабочий отдел обязателен для выбора!")]
		public int? workDepartmentId { get; set; }
		[Required(ErrorMessage = "Номер телефона обязателен для заполнения!")]
		[StringLength(11, MinimumLength = 11, ErrorMessage = "Номер телефона должен содержать 11 цифр!")]
		public string phoneNumber { get; set; }
		[Required(ErrorMessage = "Имя обязательно для заполнения!")]
		public string name { get; set; }
		[Required(ErrorMessage = "Фамилия обязательна для заполнения!")]
		public string surname { get; set; }
        public string? patronymic { get; set; }

        /// <summary>
        /// Описание профиля
        /// </summary>
        public string? profileDescription { get; set; }

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
        /// Навигационное свойство для связи с отправленными сообщениями пользователей
        /// </summary>
        public ICollection<MessageUser> messageUsers { get; set; }
    }
}
