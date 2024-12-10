using CompanyNews.Data;
using CompanyNews.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CompanyNews.Helpers.Validators
{
	/// <summary>
	/// Валидация рабочего отдела на уникальность (отсутствие дубликата в БД).
	/// </summary>
	public class NameWorkDepartamentUniquenessValidationRule : ValidationRule
	{
		private readonly CompanyNewsDbContext _context;

		public NameWorkDepartamentUniquenessValidationRule(CompanyNewsDbContext context)
		{
			_context = context; // Контекст будет передан через контейнер
		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string input = value as string;

			List<WorkDepartment> workDepartment = _context.WorkDepartments.ToList();
			if (workDepartment.Any(workDepartment => workDepartment.name.ToLowerInvariant()
				== input.ToLowerInvariant().Trim()))
			{
				return new ValidationResult(false, "Рабочий отдел уже существует.");
			}

			return ValidationResult.ValidResult;
		}
	}
}
