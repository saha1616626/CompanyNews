using CompanyNews.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.ViewModels.AdminApp.WorkingWithData
{
	public class WorkDepartmentWorkingViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Сервисы для взаиодействия с бизнес-логикой
		/// </summary>
		private readonly NewsCategoryService _newsCategoryService; // Категория
		private readonly WorkDepartmentService _workDepartmentService; // Рабочий отдел

		public WorkDepartmentWorkingViewModel()
		{
			_newsCategoryService = ServiceLocator.GetService<NewsCategoryService>();
			_workDepartmentService = ServiceLocator.GetService<WorkDepartmentService>();


		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
