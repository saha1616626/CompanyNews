using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Helpers.Event
{
	/// <summary>
	/// Обработка событий связанных с авторизацией
	/// </summary>
	public class AuthorizationEvent
	{
		/// <summary>
		/// Подписка на событие - выход из аккаунта. Переход на страницу авторизации.
		/// </summary>
		public static event EventHandler<EventAggregator> logOutYourAccount;
		/// <summary>
		/// Вызов метода перехода на страницу авторизации после выхода из аккаунта. 
		/// </summary>
		public static void LogOutYourAccount()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			logOutYourAccount?.Invoke(null, new EventAggregator());
		}
	}
}
