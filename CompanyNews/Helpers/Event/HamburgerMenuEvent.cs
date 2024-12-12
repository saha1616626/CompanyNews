using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Helpers.Event
{
	/// <summary>
	/// Обработка событий связанных с «гамбургер» меню
	/// </summary>
	public class HamburgerMenuEvent
    {
		/// <summary>
		/// Подписка на событие - закрыть гамбургер меню.
		/// </summary>
		public static event EventHandler<EventAggregator> closeHamburgerMenu;
		/// <summary>
		/// Вызов метода для закрытия гамбургер меню. 
		/// </summary>
		public static void CloseHamburgerMenu()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			closeHamburgerMenu?.Invoke(null, new EventAggregator());
		}

		/// <summary>
		/// Подписка на событие - возврат на предшествующую страницу при выходе из личного кабинета.
		/// </summary>
		public static event EventHandler<EventAggregator> backPreviousPage;
		/// <summary>
		/// Вызов метода для возврата на предшествующую страницу. 
		/// </summary>
		public static void BackPreviousPage()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			backPreviousPage?.Invoke(null, new EventAggregator());
		}
	}
}
