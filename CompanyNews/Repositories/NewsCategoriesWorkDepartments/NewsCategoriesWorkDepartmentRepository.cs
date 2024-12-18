using CompanyNews.Data;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.NewsCategoriesWorkDepartments
{
	/// <summary>
	/// Реализация интерфейса для репозитория, доступные категории новостей у рабочих отделов, 
	/// обеспечивающий основные CRUD и другие операции
	/// </summary>
	public class NewsCategoriesWorkDepartmentRepository : INewsCategoriesWorkDepartmentRepository
	{
		private readonly CompanyNewsDbContext _context;

		public NewsCategoriesWorkDepartmentRepository(CompanyNewsDbContext context)
		{
			_context = context;
		}

		#region Convert

		/// <summary>
		///  Получение доступных категории постов рабочего отдела из WorkDepartment
		/// </summary>
		public NewsCategoriesWorkDepartmentExtended? WorkDepartmentConvert
			(WorkDepartment? workDepartment)
		{
			// Проверяем, не равен ли workDepartment null
			if (workDepartment == null) { return null; }

			NewsCategoriesWorkDepartmentExtended newsCategoriesWorkDepartmentExtended = new NewsCategoriesWorkDepartmentExtended();
			newsCategoriesWorkDepartmentExtended.workDepartment = workDepartment;

			// Получение категорий для данного рабочего отдела
			List<NewsCategoriesWorkDepartment> newsCategoriesWorkDepartments = _context.NewsCategoriesWorkDepartments.Where(ncwd => ncwd.workDepartmentId == workDepartment.id).ToList();
			// Если категорий нет у всех рабочего отдела, то возваращаем пустой список
			if (newsCategoriesWorkDepartments == null) { newsCategoriesWorkDepartmentExtended.categories = null; return newsCategoriesWorkDepartmentExtended; }

			// Список доступных категорий
			ObservableCollection<NewsCategoryExtended> newsCategoryExtendeds = new ObservableCollection<NewsCategoryExtended>();

			foreach (var item in newsCategoriesWorkDepartments)
			{
				// Получаем категорию
				Models.NewsCategory newsCategory = _context.NewsCategories.FirstOrDefault(newsCategory => newsCategory.id == item.newsCategoryId);
				if (newsCategory == null) { continue; }

				// Вносим данные полученной категории в модифицированную категорию
				NewsCategoryExtended newsCategoryExtended = new NewsCategoryExtended();
				newsCategoryExtended.NewsCategoriesWorkDepartmentExtendedId = item.id; // Идентификатор доступной категории рабочего отдела (NewsCategoriesWorkDepartment)
				newsCategoryExtended.id = newsCategory.id;
				newsCategoryExtended.name = newsCategory.name;
				if (newsCategory.description != null) { newsCategoryExtended.description = newsCategory.description; }
				newsCategoryExtended.isArchived = newsCategory.isArchived;

				newsCategoryExtendeds.Add(newsCategoryExtended);
			}

			// Если не нашлись категории для рабочего отдела, то возварщаем null категорий
			if (newsCategoryExtendeds == null) { newsCategoriesWorkDepartmentExtended.categories = null; return newsCategoriesWorkDepartmentExtended; }

			newsCategoriesWorkDepartmentExtended.categories = newsCategoryExtendeds;

			return newsCategoriesWorkDepartmentExtended;
		}

		#endregion

		#region GettingData

		/// <summary>
		/// Получение доступных категорий постов рабочего отдела по идентификатору отдела
		/// </summary>
		public NewsCategoriesWorkDepartmentExtended? GetNewsCategoriesWorkDepartmentExtendedByIdAsync
			(int id)
		{
			WorkDepartment? workDepartment = _context.WorkDepartments.FirstOrDefault(workDepartment => workDepartment.id == id);
			if (workDepartment == null) { return null; }

			return WorkDepartmentConvert(workDepartment);
		}

		/// <summary>
		/// Получение списка всех доступных категорий постов рабочих отделов с группирвокой по отделу
		/// </summary>
		public ObservableCollection<NewsCategoriesWorkDepartmentExtended>? GetNewsCategoriesWorkDepartmentExtendedAsync()
		{
			IEnumerable<WorkDepartment> workDepartments = _context.WorkDepartments.ToList();
			if (workDepartments == null) { return null; }


			// Список доступных категорий 
			ObservableCollection<NewsCategoriesWorkDepartmentExtended> newsCategoriesWorkDepartmentExtendeds = new ObservableCollection<NewsCategoriesWorkDepartmentExtended>();

			foreach (var workDepartment in workDepartments)
			{
				// Преобразование идентификатора на соответствующие значение из БД
				if (WorkDepartmentConvert(workDepartment) == null) { continue; }
				NewsCategoriesWorkDepartmentExtended? newsCategoriesWorkDepartmentExtended =
					WorkDepartmentConvert(workDepartment);
				if (newsCategoriesWorkDepartmentExtended != null)
				{
					newsCategoriesWorkDepartmentExtendeds.Add(newsCategoriesWorkDepartmentExtended);
				}
			}

			return newsCategoriesWorkDepartmentExtendeds;
		}

		#endregion

		#region WorkingData

		/// <summary>
		/// Добавить категорию поста рабочему отделу
		/// </summary>
		public NewsCategoriesWorkDepartment AddNewsCategoriesWorkDepartmentAsync
			(NewsCategoriesWorkDepartment newsCategoriesWorkDepartment)
		{
			_context.NewsCategoriesWorkDepartments.Add(newsCategoriesWorkDepartment);
			_context.SaveChanges();
			return newsCategoriesWorkDepartment;
		}

		/// <summary>
		/// Удалить все категории поста у рабочего отдела по идентификатору рабочего отдела
		/// </summary>
		/// <param name="id">id категории поста</param>
		public void DeleteNewsCategoriesWorkDepartmentAsync(int id)
		{
			List<NewsCategoriesWorkDepartment> newsCategoriesWorkDepartments = _context.NewsCategoriesWorkDepartments.Where(ncwd => ncwd.workDepartmentId == id).ToList();
			if (newsCategoriesWorkDepartments == null) { return; }

			foreach(NewsCategoriesWorkDepartment item in newsCategoriesWorkDepartments)
			{
				_context.NewsCategoriesWorkDepartments.Remove(item);
			}

			_context.SaveChanges();
		}

		#endregion

	}
}
