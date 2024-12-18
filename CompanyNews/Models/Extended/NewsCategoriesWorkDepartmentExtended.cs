using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
	/// <summary>
	/// Расширенная модель доступных категорий новостей для пользователя. Идентификаторы в базовой 
	/// модели заменены на соответствующие значения
	/// </summary>
	public class NewsCategoriesWorkDepartmentExtended : INotifyPropertyChanged
	{

		/// <summary>
		/// Рабочий депарамент
		/// </summary>
		private WorkDepartment _workDepartment { get; set; }
		public WorkDepartment workDepartment
		{
			get { return _workDepartment; }
			set { _workDepartment = value; OnPropertyChanged(nameof(workDepartment)); }
		}

		/// <summary>
		/// Список категорий у рабочего отдела
		/// </summary>
		private ObservableCollection<NewsCategoryExtended>? _categories { get; set; }
		public ObservableCollection<NewsCategoryExtended>? categories
		{
			get { return _categories; }
			set { _categories = value; OnPropertyChanged(nameof(categories)); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
