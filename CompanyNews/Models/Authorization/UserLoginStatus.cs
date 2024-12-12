using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Authorization
{
	/// <summary>
	/// Состояние входа пользователя
	/// </summary>
	public class UserLoginStatus
	{
		/// <summary>
		/// Вошел ли пользователь в систему?
		/// </summary>
		public bool isUserLoggedIn {  get; set; }

		/// <summary>
		/// Учетная запись пользователя
		/// </summary>
		public int? accountId { get; set; }

		/// <summary>
		/// Роль пользователя в системе
		/// </summary>
		public string? accountRole { get; set; }

		/// <summary>
		/// Заблокирован профиль у пользователя?
		/// </summary>
		public bool? isProfileBlocked { get; set; }

		/// <summary>
		/// Причина блокировки аккаунта
		/// </summary>
		public string? reasonBlockingAccount { get; set; }

		/// <summary>
		/// Можно оставлять комментарии пользователю?
		/// </summary>
		public bool? isCanLeaveComments { get; set; }

		/// <summary>
		/// Причина запрета комментирования постов
		/// </summary>
		public string? reasonBlockingMessages { get; set; }
	}
}
