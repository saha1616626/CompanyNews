using CompanyNews.Data;
using CompanyNews.Models;
using CompanyNews.Models.Extended;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.AvailableCategoriesUsers
{
	/// <summary>
	/// Реализация интерфейса для репозитория доступные категории новостей у пользователя, 
	/// обеспечивающий основные CRUD и другие операции
	/// </summary>
	public class AvailableCategoriesUserRepository : IAvailableCategoriesUserRepository
	{
		private readonly CompanyNewsDbContext _context;

		public AvailableCategoriesUserRepository(CompanyNewsDbContext context)
		{
			_context = context;
		}

		#region Convert

		/// <summary>
		/// Замена идентификатора на соответствующее значение из БД 
		/// </summary>
		public async Task<AvailableCategoriesUserExtended?> AvailableCategoriesUserConvert
			(AvailableCategoriesUser? availableCategoriesUser)
		{
			// Проверяем, не равен ли availableCategoriesUser null
			if (availableCategoriesUser == null) { return null; }

			AvailableCategoriesUserExtended availableCategoriesUserExtended = new AvailableCategoriesUserExtended();
			availableCategoriesUserExtended.id = availableCategoriesUser.id;

			// Получение учетной записи пользователя
			Account? account = await _context.Accounts.FindAsync(availableCategoriesUser.accountId);
			if(account == null) { return null; }
			availableCategoriesUserExtended.account = account;

			// Получение доступных категорий пользователя
			List<Models.AvailableCategoriesUser>? availableCategoriesUsers = await _context.AvailableCategoriesUsers
				.Where(acu => acu.accountId == availableCategoriesUser.accountId).ToListAsync();
			if(availableCategoriesUser == null) { return null; }

			// Список доступных категорий
			List<NewsCategoryExtended> newsCategoryExtendeds = new List<NewsCategoryExtended>();

			foreach (var item in availableCategoriesUsers)
			{
				// Получаем категорию, которая доступна пользователю и добавляем в список
				Models.NewsCategory? newsCategory = await _context.NewsCategories.FindAsync(item.newsCategoryId);
				if (newsCategory == null) { continue; }

				// Вносим данные полученной категории в модифицированную категорию
				NewsCategoryExtended newsCategoryExtended = new NewsCategoryExtended();
				newsCategoryExtended.id = newsCategory.id;
				newsCategoryExtended.name = newsCategory.name;
				newsCategoryExtended.description = newsCategory.description;
				newsCategoryExtended.isArchived = newsCategory.isArchived;
				newsCategoryExtended.availableCategoriesUserExtendedId = item.id; // Идентификатор доступной категории пользователя

				newsCategoryExtendeds.Add(newsCategoryExtended);
			}

			availableCategoriesUserExtended.categories = newsCategoryExtendeds;

			return availableCategoriesUserExtended;
		}

		#endregion

		#region GettingData

		/// <summary>
		/// Получение доступной категории поста пользователя по идентификатору
		/// </summary>
		public async Task<AvailableCategoriesUserExtended?> GetAvailableCategoriesUserExtendedByIdAsync
			(int id)
		{
			AvailableCategoriesUser? availableCategoriesUser = await _context.AvailableCategoriesUsers.FindAsync(id);	
			if(availableCategoriesUser == null) { return null; }

			return await AvailableCategoriesUserConvert(availableCategoriesUser);
		}

		/// <summary>
		/// Получение списка всех доступных категорий постов пользователя
		/// </summary>
		public async Task<IEnumerable<AvailableCategoriesUserExtended>?> GetAvailableCategoriesUserExtendedAsync()
		{
			IEnumerable<AvailableCategoriesUser> availableCategoriesUsers = await _context.AvailableCategoriesUsers.ToListAsync();
			if (availableCategoriesUsers == null) { return null; }


			// Список доступных категорий 
			List<AvailableCategoriesUserExtended> availableCategoriesUserExtendeds = new List<AvailableCategoriesUserExtended>();

			foreach (var availableCategory in availableCategoriesUsers)
			{
				// Преобразование идентификатора на соответствующие значение из БД
				if (await AvailableCategoriesUserConvert(availableCategory) == null) { continue; }
				AvailableCategoriesUserExtended? availableCategoriesUserExtended = 
					await AvailableCategoriesUserConvert(availableCategory);
				availableCategoriesUserExtendeds.Add(availableCategoriesUserExtended);
			}
			
			return availableCategoriesUserExtendeds;
		}

		#endregion

		#region WorkingData

		/// <summary>
		/// Добавить категорию поста пользователю
		/// </summary>
		public async Task<AvailableCategoriesUser> AddAvailableCategoriesUserAsync
			(AvailableCategoriesUser availableCategoriesUser)
		{
			_context.AvailableCategoriesUsers.Add(availableCategoriesUser);
			await _context.SaveChangesAsync();
			return availableCategoriesUser;
		}

		/// <summary>
		/// Удалить категорию поста у пользователя по идентификатору
		/// </summary>
		/// <param name="id">id категории поста</param>
		public async Task DeleteAvailableCategoriesUserAsync(int id)
		{
			var availableCategoriesUser = await _context.AvailableCategoriesUsers.FindAsync(id);
			if (availableCategoriesUser == null) { return; }
			_context.AvailableCategoriesUsers.Remove(availableCategoriesUser);
			await _context.SaveChangesAsync();
		}

		#endregion

	}
}
