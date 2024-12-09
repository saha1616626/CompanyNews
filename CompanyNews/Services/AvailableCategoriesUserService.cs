using CompanyNews.Models.Extended;
using CompanyNews.Models;
using CompanyNews.Repositories.AvailableCategoriesUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Services
{
	/// <summary>
	/// Сервис обеспечивает бизнес-логику взаимодействия с репозиторием "доступная категория пользователя"
	/// </summary>
	public class AvailableCategoriesUserService
	{
		private readonly IAvailableCategoriesUserRepository _availableCategoriesUserRepository;
		public AvailableCategoriesUserService(IAvailableCategoriesUserRepository availableCategoriesUserRepository)
		{
			_availableCategoriesUserRepository = availableCategoriesUserRepository;
		}

		#region Convert

		public async Task<AvailableCategoriesUserExtended> AvailableCategoriesUserConvert(AvailableCategoriesUser availableCategoriesUser)
		{
			return await _availableCategoriesUserRepository.AvailableCategoriesUserConvert(availableCategoriesUser);
		}

		#endregion

		#region GettingData

		public async Task<AvailableCategoriesUserExtended> GetAvailableCategoriesUserExtendedByIdAsync(int id)
		{
			return await _availableCategoriesUserRepository.GetAvailableCategoriesUserExtendedByIdAsync(id);
		}

		public async Task<IEnumerable<AvailableCategoriesUserExtended>> GetAvailableCategoriesUserExtendedAsync()
		{
			return await _availableCategoriesUserRepository.GetAvailableCategoriesUserExtendedAsync();
		}

		#endregion

		#region CRUD Operations

		public async Task<AvailableCategoriesUser> AddAvailableCategoriesUserAsync(AvailableCategoriesUser availableCategoriesUser)
		{
			var addedAvailableCategoriesUser = await _availableCategoriesUserRepository.AddAvailableCategoriesUserAsync(availableCategoriesUser);
			return addedAvailableCategoriesUser; // Возвращаем добавленную категорию пользователя
		}

		public async Task DeleteAvailableCategoriesUserAsync(int id)
		{
			await _availableCategoriesUserRepository.DeleteAvailableCategoriesUserAsync(id);
		}

		#endregion

	}
}
