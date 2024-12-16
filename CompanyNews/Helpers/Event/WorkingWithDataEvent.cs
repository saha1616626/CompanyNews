using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Helpers.Event
{
	/// <summary>
	/// Обработка событий связанных с работой над данными
	/// </summary>
	public class WorkingWithDataEvent
	{
		#region Account 

		/// <summary>
		/// Подписка на событие - успешное добавление данных. На странице для работы с аккаунтами.
		/// </summary>
		public static event EventHandler<EventAggregator> dataWasAddedSuccessfullyAccount;
		/// <summary>
		/// Вызов метода успешного добавление данных. На странице для работы с аккаунтами.
		/// </summary>
		public static void DataWasAddedSuccessfullyAccount()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			dataWasAddedSuccessfullyAccount?.Invoke(null, new EventAggregator());
		}

		/// <summary>
		/// Подписка на событие - успешное изменение данных. На странице для работы с аккаунтами.
		/// </summary>
		public static event EventHandler<EventAggregator> dataWasChangedSuccessfullyAccount;
		/// <summary>
		/// Вызов метода успешного изменения данных. На странице для работы с аккаунтами.
		/// </summary>
		public static void DataWasChangedSuccessfullyAccount()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			dataWasChangedSuccessfullyAccount?.Invoke(null, new EventAggregator());
		}

		#endregion


		#region NewsCategory

		/// <summary>
		/// Подписка на событие - успешное добавление данных. На странице для работы с категориями.
		/// </summary>
		public static event EventHandler<EventAggregator> dataWasAddedSuccessfullyNewsCategory;
		/// <summary>
		/// Вызов метода успешного добавление данных. На странице для работы с категориями.
		/// </summary>
		public static void DataWasAddedSuccessfullyNewsCategory()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			dataWasAddedSuccessfullyNewsCategory?.Invoke(null, new EventAggregator());
		}

		/// <summary>
		/// Подписка на событие - успешное изменение данных. На странице для работы с категориями.
		/// </summary>
		public static event EventHandler<EventAggregator> dataWasChangedSuccessfullyNewsCategory;
		/// <summary>
		/// Вызов метода успешного изменения данных. На странице для работы с категориями.
		/// </summary>
		public static void DataWasChangedSuccessfullyNewsCategory()
		{
			// Проверяем, есть ли подписчики на событие и вызываем их
			dataWasChangedSuccessfullyNewsCategory?.Invoke(null, new EventAggregator());
		}

		#endregion
	}
}
