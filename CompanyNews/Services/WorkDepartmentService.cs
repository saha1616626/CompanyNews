using CompanyNews.Models;
using CompanyNews.Repositories.WorkDepartments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Services
{
	/// <summary>
	/// Сервис обеспечивает бизнес-логику взаимодействия с репозиторием "рабочий отдел"
	/// </summary>
	public class WorkDepartmentService
	{
		private readonly IWorkDepartmentRepository _workDepartmentRepository;
		public WorkDepartmentService(IWorkDepartmentRepository workDepartmentRepository)
		{
			_workDepartmentRepository = workDepartmentRepository;
		}

		#region GettingData

		public async Task<WorkDepartment> GetWorkDepartmentByIdAsync(int id)
		{
			return await _workDepartmentRepository.GetWorkDepartmentByIdAsync(id);
		}

		public async Task<IEnumerable<WorkDepartment>> GetAllWorkDepartmentsAsync()
		{
			return await _workDepartmentRepository.GetAllWorkDepartmentsAsync();
		}

		#endregion

		#region CRUD Operations

		public async Task<WorkDepartment> AddWorkDepartmentAsync(WorkDepartment workDepartment)
		{
			var addedWorkDepartment = _workDepartmentRepository.AddWorkDepartmentAsync(workDepartment);
			return await addedWorkDepartment; // Возвращаем добавленный рабочий отдел
		}

		public async Task UpdateWorkDepartmentAsync(WorkDepartment workDepartment)
		{
			await _workDepartmentRepository.UpdateWorkDepartmentAsync(workDepartment);
		}

		public async Task DeleteWorkDepartmentAsync(int id)
		{
			await _workDepartmentRepository.DeleteWorkDepartmentAsync(id);
		}

		#endregion
	}
}
