using CompanyNews.Data;
using CompanyNews.Models;
using Microsoft.EntityFrameworkCore;
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
	/// Валидация логина учетной записи на уникальность (отсутствие дубликата в БД).
	/// </summary>
	public class LoginUniquenessValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string input = value as string;

			CompanyNewsDbContext _context = new CompanyNewsDbContext();
			List<Account> accounts = _context.Accounts.ToList();
			if (accounts.Any(accounts => accounts.name.ToLowerInvariant() 
				== input.ToLowerInvariant().Trim()))
			{
				return new ValidationResult(false, "Логин занят.");
			}

			return ValidationResult.ValidResult;
		}

	}
}
