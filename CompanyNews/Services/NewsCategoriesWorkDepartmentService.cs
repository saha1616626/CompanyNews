﻿using CompanyNews.Models.Extended;
using CompanyNews.Models;
using CompanyNews.Repositories.NewsCategoriesWorkDepartments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CompanyNews.Services
{
	/// <summary>
	/// Сервис обеспечивает бизнес-логику взаимодействия с репозиторием "доступная категория рабочего отдела"
	/// </summary>
	public class NewsCategoriesWorkDepartmentService
	{
		private readonly INewsCategoriesWorkDepartmentRepository _newsCategoriesWorkDepartmentRepository;
		public NewsCategoriesWorkDepartmentService(INewsCategoriesWorkDepartmentRepository newsCategoriesWorkDepartmentRepository)
		{
			_newsCategoriesWorkDepartmentRepository = newsCategoriesWorkDepartmentRepository;
		}

		#region Convert


		/// <summary>
		/// Получение доступных категории постов рабочего отдела из WorkDepartment 
		/// </summary>
		public async Task<NewsCategoriesWorkDepartmentExtended?> WorkDepartmentConvert
			(WorkDepartment? workDepartment)
		{
			return _newsCategoriesWorkDepartmentRepository.WorkDepartmentConvert(workDepartment);
		}

		#endregion

		#region GettingData

		/// <summary>
		/// Получение доступных категории постов рабочего отдела по идентификатору
		/// </summary>
		/// <param name="id">id доступной категории</param>
		public async Task<NewsCategoriesWorkDepartmentExtended?> GetNewsCategoriesWorkDepartmentExtendedByIdAsync
			(int id)
		{
			return _newsCategoriesWorkDepartmentRepository.GetNewsCategoriesWorkDepartmentExtendedByIdAsync(id);
		}

		/// <summary>
		/// Получение списка всех доступных категорий постов рабочих отделов с группирвокой по отделу
		/// </summary>
		public async Task<ObservableCollection<NewsCategoriesWorkDepartmentExtended>?> GetNewsCategoriesWorkDepartmentExtendedAsync()
		{
			return _newsCategoriesWorkDepartmentRepository.GetNewsCategoriesWorkDepartmentExtendedAsync();
		}

		#endregion

		#region CRUD Operations

		/// <summary>
		/// Добавить категорию поста рабочему отделу
		/// </summary>
		/// <param name="newsCategory">Данные новой категории рабочего отдела</param>
		public async Task<NewsCategoriesWorkDepartment> AddNewsCategoriesWorkDepartmentAsync
			(NewsCategoriesWorkDepartment newsCategoriesWorkDepartment)
		{
			var addedAvailableCategoriesUser = _newsCategoriesWorkDepartmentRepository.AddNewsCategoriesWorkDepartmentAsync(newsCategoriesWorkDepartment);
			return addedAvailableCategoriesUser; // Возвращаем добавленную категорию рабочего отдела
		}


		/// <summary>
		/// Удалить все категории поста у рабочего отдела по идентификатору рабочего отдела
		/// </summary>
		/// <param name="id">id рабочего отдела</param>
		public async Task DeleteNewsCategoriesWorkDepartmentAsync(int id)
		{
			_newsCategoriesWorkDepartmentRepository.DeleteNewsCategoriesWorkDepartmentAsync(id);
		}

		#endregion

	}
}
