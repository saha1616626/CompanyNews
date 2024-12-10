using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Helpers
{
	/// <summary>
	/// Хэширование паролей
	/// </summary>
	public class PasswordHasher
	{
		private const int SaltSize = 16; // 128 бит
		private const int HashSize = 20; // 160 бит
		private const int Iterations = 10000; // число итераций PBKDF2

		/// <summary>
		/// Метод хэширования
		/// </summary>
		/// <param name="password"> Пароль </param>
		/// <returns> Пароль в зашифрованном виде </returns>
		public static string HashPassword(string password)
		{
			// Генерация соли
			byte[] salt = GenerateSalt();

			// Вычисление хэш пароля с использованием соли
			byte[] hash = ComputeHash(password, salt);

			// Объединение соли и хэша в одну строку для хранения
			return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
		}

		/// <summary>
		/// Проверка пароля, введенного пользователем
		/// </summary>
		/// <param name="password">Новый пароль</param>
		/// <param name="hashedPassword">Текущий пароль</param>
		/// <returns>Совпадает пароль или нет</returns>
		public static bool VerifyPassword(string password, string hashedPassword)
		{
			// Разделение строки на соль и хэш
			string[] parts = hashedPassword.Split(':');
			byte[] salt = Convert.FromBase64String(parts[0]);
			byte[] expectedHash = Convert.FromBase64String(parts[1]);

			// Вычисляем хэш введенного пароля с использованием сохраненной соли
			byte[] hash = ComputeHash(password, salt);

			// Сравнение хэша пароля пользователя и сохраненного
			return ByteArraysEqual(hash, expectedHash);
		}

		/// <summary>
		/// Генерация соли
		/// </summary>
		/// <returns>Соль</returns>
		private static byte[] GenerateSalt()
		{
			byte[] salt = new byte[SaltSize];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}
			return salt;
		}

		/// <summary>
		/// Вычисление хэша пароля с использованием соли
		/// </summary>
		/// <param name="password">Пароль в незашифрованном виде.</param>
		/// <param name="salt">Соль.</param>
		/// <returns></returns>
		private static byte[] ComputeHash(string password, byte[] salt)
		{
			using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
			{
				return pbkdf2.GetBytes(HashSize);
			}
		}

		/// <summary>
		/// Сравнение хэша пароля пользователя и сохраненного
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool ByteArraysEqual(byte[] a, byte[] b)
		{
			// Сравнение двух байтовых массивов в постоянном времени, чтобы предотвратить утечку информации о различиях
			if (a.Length != b.Length)
			{
				return false;
			}

			int diff = 0;
			for (int i = 0; i < a.Length; i++)
			{
				diff |= a[i] ^ b[i];
			}
			return diff == 0;
		}
	}
}
