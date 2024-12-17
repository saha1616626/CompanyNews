using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
	/// <summary>
	/// Все сообщения под постом
	/// </summary>
	public class MessagesNewsPostExtended : INotifyPropertyChanged
	{
		/// <summary>
		/// Пост с известной категорией
		/// </summary>
		private NewsPostExtended _newsPostExtended { get; set; }
		public NewsPostExtended NewsPostExtended
		{
			get { return _newsPostExtended; }
			set { _newsPostExtended = value; OnPropertyChanged(nameof(NewsPostExtended)); }
		}

		/// <summary>
		/// Сообщения
		/// </summary>
		private ObservableCollection<MessageUserExtended>? _messageUserExtendeds { get; set; }
		public ObservableCollection<MessageUserExtended>? MessageUserExtendeds
		{
			get { return _messageUserExtendeds; }
			set {  _messageUserExtendeds = value; OnPropertyChanged(nameof(MessageUserExtendeds)); }
		}

		
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
