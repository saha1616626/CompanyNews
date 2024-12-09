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
	/// Валидация номера телефона
	/// </summary>
	public class PhoneNumberValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string input = value as string;

			// Проверка на заполнение
			if (string.IsNullOrEmpty(input))
			{
				return new ValidationResult(false, "Номер телефона должен быть заполнен.");
			}

			// Проверка на длину
			if(input.Length != 11)
			{
				return new ValidationResult(false, "Номер телефона должен содержать 11 цифр.");
			}

			// Проверка на символы
			if(!Regex.IsMatch(input, @"^[0-9]+$"))
			{
				return new ValidationResult(false, "Номер телефона должен содержать только цифры.");
			}

			// Проверка на первый символ
			if (!input.StartsWith("7"))
			{
				return new ValidationResult(false, "Номер телефона должен начинаться с '7'.");
			}

			return ValidationResult.ValidResult;
		}

	}
}
