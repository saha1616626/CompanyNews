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
	/// Валидация категории поста на уникальность (отсутствие дубликата в БД).
	/// </summary>
	public class NameNewsCategoryUniquenessValidationRule : ValidationRule
	{
		private readonly CompanyNewsDbContext _context;

		public NameNewsCategoryUniquenessValidationRule(CompanyNewsDbContext context)
		{
			_context = context; // Контекст будет передан через контейнер
		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string input = value as string;

			List<NewsCategory> newsCategories = _context.NewsCategories.ToList();
			if (newsCategories.Any(newsCategory => newsCategory.name.ToLowerInvariant()
				== input.ToLowerInvariant().Trim()))
			{
				return new ValidationResult(false, "Категория уже существует.");
			}

			return ValidationResult.ValidResult;
		}
	}
}
