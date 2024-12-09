using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CompanyNews.Helpers.Validators
{
    /// <summary>
    /// Валидация логина
    /// </summary>
    public class LoginValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
			string input = value as string;

			// Проверка на заполнение
			if (string.IsNullOrEmpty(input))
			{
				return new ValidationResult(false, "Логин должен быть заполнен.");
			}

			// Проверка на минимальную длину
			if (input.Length < 8)
			{
				return new ValidationResult(false, "Логин должен содержать не менее 8 символов.");
			}

			// Проверка на латинский ввод
			if (!Regex.IsMatch(input, @"^[a-zA-Z0-9]*$"))

			{
				return new ValidationResult(false, "Логин может содержать только цифры и латинские буквы.");
			}

			return ValidationResult.ValidResult;
		}

	}
}
