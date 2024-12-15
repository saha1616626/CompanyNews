using CompanyNews.Helpers;
using CompanyNews.Helpers.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.ViewModels.AdminApp.WorkingWithData
{
	/// <summary>
	/// Класс для работы над аккаунтами. Добавление и редактирование данных.
	/// </summary>
	public class AccountWorkingViewModel : INotifyPropertyChanged
	{



		#region WorkingWithPages

		/// <summary>
		/// Возврат на предыдущую страницу.
		/// </summary>
		private RelayCommand _refund { get; set; }
		public RelayCommand Refund
		{
			get
			{
				return _refund ??
					(_refund = new RelayCommand(async (obj) =>
					{
						// Переход на страницу "Учетные записи"
						HamburgerMenuEvent.OpenPageAccount();
					}, (obj) => true));
			}
		}

		#endregion

		#region Features

		/// <summary>
		/// CheckBox для блокироваки аккаунта в UI. 
		/// </summary>
		private bool _isBlockAccount { get; set; }
		public bool IsBlockAccount
		{
			get { return _isBlockAccount; }
			set
			{
				_isBlockAccount = value; 
				OnPropertyChanged(nameof(IsBlockAccount));
				if (IsBlockAccount)
				{
					IsReasonBlockingMessages = true;
				}
				else
				{
					IsReasonBlockingMessages = false;
				}
			}
		}

		/// <summary>
		/// Отображение или скрытие поля для ввода причины блокироваки аккаунта в UI. 
		/// </summary>
		private bool _isReasonBlockingMessages { get; set; }
		public bool IsReasonBlockingMessages
		{
			get { return _isReasonBlockingMessages; }
			set { _isReasonBlockingMessages = value; OnPropertyChanged(nameof(IsReasonBlockingMessages)); }
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
