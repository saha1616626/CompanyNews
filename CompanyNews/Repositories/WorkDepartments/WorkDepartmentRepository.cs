using CompanyNews.Data;
using CompanyNews.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.WorkDepartments
{
	/// <summary>
	/// Реализация интерфейса управления рабочими отделами через CompanyNewsDbContext
	/// </summary>
	class WorkDepartmentRepository : IWorkDepartmentRepository
	{
		private readonly CompanyNewsDbContext _context;

		public WorkDepartmentRepository(CompanyNewsDbContext context)
		{
			_context = context;
		}

		#region GettingData

		/// <summary>
		/// Получение рабочего отдела по id
		/// </summary>
		public async Task<WorkDepartment?> GetWorkDepartmentByIdAsync(int id)
		{
			WorkDepartment? workDepartment = await  _context.WorkDepartments.FirstOrDefaultAsync(wd => wd.id == id);
			if (workDepartment == null) { return null; }

			return workDepartment;
		}

		/// <summary>
		/// Получение списка рабочих отделов
		/// </summary>
		public async Task<IEnumerable<WorkDepartment>?> GetAllWorkDepartmentsAsync()
		{
			// Получаем список рабочих отделов
			IEnumerable<WorkDepartment> workDepartments = await _context.WorkDepartments.ToListAsync();
			if (workDepartments == null) { return null; }

			return workDepartments;
		}

		#endregion

		#region WorkingData

		/// <summary>
		/// Добавить рабочий отдел
		/// </summary>
		public async Task<WorkDepartment> AddWorkDepartmentAsync(WorkDepartment workDepartment)
		{
			_context.WorkDepartments.Add(workDepartment);
			await _context.SaveChangesAsync();
			return workDepartment; // Возвращаем объект с обновленными данными, включая Id
		}

		/// <summary>
		/// Изменить рабочий отдел
		/// </summary>
		public async Task UpdateWorkDepartmentAsync(WorkDepartment workDepartment)
		{
			if (workDepartment == null) throw new ArgumentNullException(nameof(workDepartment));

			// Убедимся, что рабочий отдел существует
			var existingNewsWorkDepartment = await _context.WorkDepartments.FindAsync(workDepartment.id);
			if (existingNewsWorkDepartment == null) throw new KeyNotFoundException($"Рабочий отдел с ID {existingNewsWorkDepartment.id} не найден.");

			// Обновление данных. Данным методом можно обновить только указанные поля в workDepartment
			_context.Entry(existingNewsWorkDepartment).CurrentValues.SetValues(workDepartment);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Удалить рабочий отдел
		/// </summary>
		public async Task DeleteWorkDepartmentAsync(int id)
		{
			var workDepartment = await _context.WorkDepartments.FindAsync(id);
			if (workDepartment == null) { return; }
			_context.WorkDepartments.Remove(workDepartment);
			await _context.SaveChangesAsync();
		}

		#endregion
	}
}
