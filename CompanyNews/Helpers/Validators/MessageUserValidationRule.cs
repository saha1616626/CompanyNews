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
	/// Валидация сообщения, отправленного пользователем.
	/// </summary>
	public class MessageUserValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string input = value as string;

			// Проверка на заполнение
			if (string.IsNullOrEmpty(input))
			{
				return new ValidationResult(false, "Введите сообщение.");
			}

			return ValidationResult.ValidResult;
		}
	}
}
