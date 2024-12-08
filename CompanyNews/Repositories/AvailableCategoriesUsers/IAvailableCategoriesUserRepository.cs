using CompanyNews.Models;
using CompanyNews.Models.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.AvailableCategoriesUsers
{
	/// <summary>
	/// Интерфейс для репозитория доступные категории новостей у пользователя, 
	/// обеспечивающий основные CRUD и другие операции
	/// </summary>
	public interface IAvailableCategoriesUserRepository
    {
		/// <summary>
		/// Получение AvailableCategoriesUserExtended из AvailableCategoriesUser. 
		/// Замена идентификатора на соответствующее значение из БД 
		/// </summary>
		Task<AvailableCategoriesUserExtended> AvailableCategoriesUserConvert(AvailableCategoriesUser availableCategoriesUser);

		/// <summary>
		/// Получение AvailableCategoriesUser из AvailableCategoriesUserExtended. 
		/// Замена значения на соответствующий идентификатор из БД 
		/// </summary>
		Task<AvailableCategoriesUser> AvailableCategoriesUserExtendedConvert(AvailableCategoriesUserExtended availableCategoriesUserExtended);

		/// <summary>
		/// Получение доступной категории пользователю по идентификатору
		/// </summary>
		/// <param name="id">id доступной категории</param>
		Task<AvailableCategoriesUserExtended> GetAvailableCategoriesUserExtendedByIdAsync(int id);

		/// <summary>
		/// Получение списка доступных категорий у пользователя
		/// </summary>
		Task<IEnumerable<AvailableCategoriesUserExtended>> GetAvailableCategoriesUserExtendedAsync();

		/// <summary>
		/// Добавить категорию поста пользователю
		/// </summary>
		/// <param name="newsCategory">Данные нового категории</param>
		Task AddAvailableCategoriesUserAsync(AvailableCategoriesUser availableCategoriesUser);

		/// <summary>
		/// Удалить категорию поста у пользователя по идентификатору
		/// </summary>
		/// <param name="id">id категории поста</param>
		Task DeleteAvailableCategoriesUserAsync(int id);
	}
}
