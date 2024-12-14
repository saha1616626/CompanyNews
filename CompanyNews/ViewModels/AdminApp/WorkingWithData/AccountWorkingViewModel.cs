using CompanyNews.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.ViewModels.AdminApp.WorkingWithData
{
	/// <summary>
	/// Класс для работы над аккаунтами. Добавление и редактирование данных.
	/// </summary>
	public class AccountWorkingViewModel
	{
		/// <summary>
		/// Кнопка сохранения новых или изменения старых данных аккаунта в UI Popup
		/// </summary>

		private RelayCommand _saveData { get; set; }
		public RelayCommand SaveData
		{
			get
			{
				return _saveData ??
					(_saveData = new RelayCommand(async (obj) =>
					{

						//if (isAddData) // Логика при добавлении данных
						//{

						//}
						//else // Логика при редактировании данных
						//{

						//}

					}, (obj) => true));
			}
		}
	}
}
