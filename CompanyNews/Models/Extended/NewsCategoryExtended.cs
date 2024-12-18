using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models.Extended
{
	/// <summary>
	/// Класс содержит идентификатор доступной категории у рабочего отдела
	/// </summary>
	public class NewsCategoryExtended : NewsCategory, INotifyPropertyChanged
	{
		/// <summary>
		/// Идентификатор категории для определенного рабочего отдела (NewsCategoriesWorkDepartment)
		/// </summary>
		private int _newsCategoriesWorkDepartmentExtendedId { get; set; }
		public int NewsCategoriesWorkDepartmentExtendedId
		{
			get { return _newsCategoriesWorkDepartmentExtendedId; }
			set {  _newsCategoriesWorkDepartmentExtendedId = value; OnPropertyChanged(); }
		}

		/// <summary>
		/// Добавлена категория в список или нет
		/// </summary>
		private bool? _isAddCategory { get; set; }
		public bool? IsAddCategory
		{
			get { return _isAddCategory; }
			set { _isAddCategory = value; OnPropertyChanged(nameof(IsAddCategory)); }
		}

		/// <summary>
		/// Удалена категория из списка или нет
		/// </summary>
		private bool? _isDeleteCategory { get; set; }
		public bool? IsDeleteCategory
		{
			get { return _isDeleteCategory; }
			set { _isDeleteCategory = value; OnPropertyChanged(nameof(IsDeleteCategory)); }
		}

		/// <summary>
		/// Свойство для определения, выбрана ли категория
		/// </summary>
		private bool? _isSelected { get; set; }
		public bool? IsSelected
		{
			get { return _isSelected; }
			set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
