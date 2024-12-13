﻿using CompanyNews.Models;
using CompanyNews.Models.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.NewsCategoriesWorkDepartments
{
	/// <summary>
	/// Интерфейс для репозитория, доступные категории новостей у рабочих отделов, 
	/// обеспечивающий основные CRUD и другие операции
	/// </summary>
	public interface INewsCategoriesWorkDepartmentRepository
	{
		/// <summary>
		/// Получение доступных категории постов рабочего отдела из WorkDepartment 
		/// </summary>
		Task<NewsCategoriesWorkDepartmentExtended?> WorkDepartmentConvert
			(WorkDepartment? workDepartment);

		/// <summary>
		/// Получение доступных категории постов рабочего отдела по идентификатору
		/// </summary>
		/// <param name="id">id доступной категории</param>
		Task<NewsCategoriesWorkDepartmentExtended?> GetNewsCategoriesWorkDepartmentExtendedByIdAsync
			(int id);

		/// <summary>
		/// Получение списка всех доступных категорий постов рабочих отделов с группирвокой по отделу
		/// </summary>
		Task<IEnumerable<NewsCategoriesWorkDepartmentExtended>?> GetNewsCategoriesWorkDepartmentExtendedAsync();

		/// <summary>
		/// Добавить категорию поста рабочему отделу
		/// </summary>
		/// <param name="newsCategory">Данные новой категории рабочего отдела</param>
		Task<NewsCategoriesWorkDepartment> AddNewsCategoriesWorkDepartmentAsync
			(NewsCategoriesWorkDepartment newsCategoriesWorkDepartment);

		/// <summary>
		/// Удалить категорию поста у рабочего отдела по идентификатору
		/// </summary>
		/// <param name="id">id категории поста</param>
		Task DeleteNewsCategoriesWorkDepartmentAsync(int id);
	}
}