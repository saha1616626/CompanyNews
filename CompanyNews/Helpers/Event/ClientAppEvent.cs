using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Helpers.Event
{
	/// <summary>
	/// События на клиентской стороне
	/// </summary>
	public class ClientAppEvent
	{

		/// <summary>
		/// Подписка на событие - выход из поста.
		/// </summary>
		public static event EventHandler<EventAggregator> exitPost;
		/// <summary>
		/// Вызов метода выхода из поста.
		/// </summary>
		public static void ExitPost()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			exitPost?.Invoke(null, new EventAggregator());
		}
	}
}
