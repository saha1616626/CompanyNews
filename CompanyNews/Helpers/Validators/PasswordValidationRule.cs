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
    /// Валидация пароля
    /// </summary>
    public class PasswordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            // Проверка на заполнение
            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "Пароль должен быть заполнен.");
            }

            // Проверка на минимальную длину
            if(input.Length < 8)
            {
				return new ValidationResult(false, "Пароль должен содержать не менее 8 символов.");
			}

			// @"^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[!@#$%^&*()_\-+=\[\]{};':""\\|,.<>\/?]).+$" - Пароль должен содержать цифры, латинские буквы и специальные символы!

			// Проверка на латинский ввод
			if (!Regex.IsMatch(input, @"^[a-zA-Z0-9!@#$%^&*()_\-+=\[\]{};':""\\|,.<>\/?]*$"))

			{
				return new ValidationResult(false, "Пароль может содержать только цифры, латинские буквы и специальные символы.");
			}

            return ValidationResult.ValidResult;
        }

	}
}
