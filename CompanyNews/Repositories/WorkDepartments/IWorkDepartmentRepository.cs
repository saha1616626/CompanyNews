using CompanyNews.Models.Extended;
using CompanyNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.WorkDepartments
{
	/// <summary>
	/// Интерфейс репозитория — управляет доступом базы данных для работы с рабочим отделом
	/// </summary>
	public interface IWorkDepartmentRepository
    {
		/// <summary>
		/// Получение рабочего отдела
		/// </summary>
		Task<WorkDepartment> GetWorkDepartmentByIdAsync(int id);

		/// <summary>
		/// Получение списка рабочих отделов
		/// </summary>
		IEnumerable<WorkDepartment> GetAllWorkDepartmentsAsync();

		/// <summary>
		/// Добавить рабочий отдел
		/// </summary>
		/// <param name="workDepartment">Данные нового рабочего отдела</param>
		Task<WorkDepartment> AddWorkDepartmentAsync(WorkDepartment workDepartment);

		/// <summary>
		/// Изменить аккаунт
		/// </summary>
		/// <param name="workDepartment">Измененные данные</param>
		Task UpdateWorkDepartmentAsync(WorkDepartment workDepartment);

		/// <summary>
		/// Удалить аккаунт
		/// </summary>
		/// <param name="id">id пользователя</param>
		Task DeleteWorkDepartmentAsync(int id);
	}
}
