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

		#region OpenPageInMenu

		/// <summary>
		/// Событие — переход на страницу для работы с учетными записями.
		/// </summary>
		public static event EventHandler<EventAggregator> openPageAccount;
		/// <summary>
		/// Вызов метода для перехода на страницу для работы с учетными записями. 
		/// </summary>
		public static void OpenPageAccount()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			openPageAccount?.Invoke(null, new EventAggregator());
		}

		/// <summary>
		/// Событие — переход на страницу для работы с рабочими отделами.
		/// </summary>
		public static event EventHandler<EventAggregator> openPageWorkDepartment;
		/// <summary>
		/// Вызов метода для перехода на страницу для работы с рабочими отделами.
		/// </summary>
		public static void OpenPageWorkDepartment()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			openPageWorkDepartment?.Invoke(null, new EventAggregator());
		}

		/// <summary>
		/// Событие — переход на страницу для работы с категориями постов.
		/// </summary>
		public static event EventHandler<EventAggregator> openPageNewsCategory;
		/// <summary>
		/// Вызов метода для перехода на страницу для работы с категориями постов.
		/// </summary>
		public static void OpenPageNewsCategory()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			openPageNewsCategory?.Invoke(null, new EventAggregator());
		}

		/// <summary>
		/// Событие — переход на страницу для работы с новостными постами.
		/// </summary>
		public static event EventHandler<EventAggregator> openPageNewsPost;
		/// <summary>
		/// Вызов метода для перехода на страницу для работы с новостными постами.
		/// </summary>
		public static void OpenPageNewsPost()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			openPageNewsPost?.Invoke(null, new EventAggregator());
		}

		/// <summary>
		/// Событие — переход на страницу для работы с сообщениями пользователей.
		/// </summary>
		public static event EventHandler<EventAggregator> openPageMessageUser;
		/// <summary>
		/// Вызов метода для перехода на страницу для работы с сообщениями пользователей.
		/// </summary>
		public static void OpenPageMessageUser()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			openPageMessageUser?.Invoke(null, new EventAggregator());
		}

		#endregion

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
