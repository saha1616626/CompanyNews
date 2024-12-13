using CompanyNews.Data;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
		public async Task<NewsCategoriesWorkDepartmentExtended?> WorkDepartmentConvert
			(WorkDepartment? workDepartment)
		{
			// Проверяем, не равен ли workDepartment null
			if (workDepartment == null) { return null; }

			NewsCategoriesWorkDepartmentExtended newsCategoriesWorkDepartmentExtended = new NewsCategoriesWorkDepartmentExtended();
			newsCategoriesWorkDepartmentExtended.workDepartment = workDepartment;

			// Получение категорий для данного рабочего отдела
			List<NewsCategoriesWorkDepartment> newsCategoriesWorkDepartments = await _context.NewsCategoriesWorkDepartments.Where(ncwd => ncwd.workDepartmentId == workDepartment.id).ToListAsync();
			// Если категорий нет у всех рабочего отдела, то возваращаем пустой список
			if (newsCategoriesWorkDepartments == null) { newsCategoriesWorkDepartmentExtended.categories = null; return newsCategoriesWorkDepartmentExtended; }

			// Список доступных категорий
			List<NewsCategoryExtended> newsCategoryExtendeds = new List<NewsCategoryExtended>();

			foreach(var item in newsCategoriesWorkDepartments)
			{
				// Получаем категорию
				Models.NewsCategory newsCategory = await _context.NewsCategories.FirstOrDefaultAsync(newsCategory => newsCategory.id == item.newsCategoryId);
				if (newsCategory == null) {  continue; }

				// Вносим данные полученной категории в модифицированную категорию
				NewsCategoryExtended newsCategoryExtended = new NewsCategoryExtended();
				newsCategoryExtended.NewsCategoriesWorkDepartmentExtendedId = item.id; // Идентификатор доступной категории рабочего отдела (NewsCategoriesWorkDepartment)
				newsCategoryExtended.id = newsCategory.id;
				newsCategoryExtended.name = newsCategory.name;
				if(newsCategory.description != null) { newsCategoryExtended.description = newsCategory.description; }
				newsCategoryExtended.isArchived = newsCategory.isArchived;

				newsCategoryExtendeds.Add(newsCategoryExtended);
			}

			// Если не нашлись категории для рабочего отдела, то возварщаем null категорий
			if(newsCategoryExtendeds == null) { newsCategoriesWorkDepartmentExtended.categories = null; return newsCategoriesWorkDepartmentExtended; }

			newsCategoriesWorkDepartmentExtended.categories = newsCategoryExtendeds;

			return newsCategoriesWorkDepartmentExtended;
		}

		#endregion

		#region GettingData

		/// <summary>
		/// Получение доступных категорий постов рабочего отдела по идентификатору отдела
		/// </summary>
		public async Task<NewsCategoriesWorkDepartmentExtended?> GetNewsCategoriesWorkDepartmentExtendedByIdAsync
			(int id)
		{
			WorkDepartment? workDepartment = await _context.WorkDepartments.FirstOrDefaultAsync(workDepartment => workDepartment.id == id);	
			if(workDepartment == null) { return null; }

			return await WorkDepartmentConvert(workDepartment);
		}

		/// <summary>
		/// Получение списка всех доступных категорий постов рабочих отделов с группирвокой по отделу
		/// </summary>
		public async Task<IEnumerable<NewsCategoriesWorkDepartmentExtended>?> GetNewsCategoriesWorkDepartmentExtendedAsync()
		{
			IEnumerable<WorkDepartment> workDepartments = await _context.WorkDepartments.ToListAsync();
			if (workDepartments == null) { return null; }


			// Список доступных категорий 
			List<NewsCategoriesWorkDepartmentExtended> newsCategoriesWorkDepartmentExtendeds = new List<NewsCategoriesWorkDepartmentExtended>();

			foreach (var workDepartment in workDepartments)
			{
				// Преобразование идентификатора на соответствующие значение из БД
				if (await WorkDepartmentConvert(workDepartment) == null) { continue; }
				NewsCategoriesWorkDepartmentExtended? newsCategoriesWorkDepartmentExtended = 
					await WorkDepartmentConvert(workDepartment);
				if(newsCategoriesWorkDepartmentExtended != null)
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
		public async Task<NewsCategoriesWorkDepartment> AddNewsCategoriesWorkDepartmentAsync
			(NewsCategoriesWorkDepartment newsCategoriesWorkDepartment)
		{
			_context.NewsCategoriesWorkDepartments.Add(newsCategoriesWorkDepartment);
			await _context.SaveChangesAsync();
			return newsCategoriesWorkDepartment;
		}

		/// <summary>
		/// Удалить категорию поста у рабочего отдела по идентификатору
		/// </summary>
		/// <param name="id">id категории поста</param>
		public async Task DeleteNewsCategoriesWorkDepartmentAsync(int id)
		{
			var newsCategoriesWorkDepartment = await _context.NewsCategoriesWorkDepartments.FirstOrDefaultAsync(ncwd => ncwd.id == id);
			if (newsCategoriesWorkDepartment == null) { return; }
			_context.NewsCategoriesWorkDepartments.Remove(newsCategoriesWorkDepartment);
			await _context.SaveChangesAsync();
		}

		#endregion

	}
}
