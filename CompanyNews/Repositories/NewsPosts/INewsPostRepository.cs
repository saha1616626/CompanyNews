using CompanyNews.Models;
using CompanyNews.Models.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Repositories.NewsCategory
{
	/// <summary>
	/// Интерфейс для репозитория постов новостей, 
	/// предоставляющий основные CRUD-операции и другие функции для работы с новостными постами.
	/// </summary>
	public interface INewsPostRepository
    {
		/// <summary>
		/// Получение NewsPostExtended из NewsPost. 
		/// Замена идентификатора на соответствующее значение из БД 
		/// </summary>
		NewsPostExtended? NewsPostConvert(NewsPost? newsPosts);

		/// <summary>
		/// Получение NewsPost из NewsPostExtended. 
		/// Замена значения на соответствующий идентификатор из БД 
		/// </summary>
		Task<NewsPost?> NewsPostExtendedConvert(NewsPostExtended? newsPostExtended);

		/// <summary>
		/// Получение поста новости по идентификатору.
		/// </summary>
		/// <param name="id">Идентификатор поста новости.</param>
		Task<NewsPostExtended?> GetNewsPostByIdAsync(int id);

		/// <summary>
		/// Получение списка всех постов новостей.
		/// </summary>
		IEnumerable<NewsPostExtended>? GetAllNewsPostsAsync();

		/// <summary>
		/// Добавление нового поста новости.
		/// </summary>
		/// <param name="newsPost">Данные нового поста новости.</param>
		NewsPost AddNewsPostAsync(NewsPost newsPost);

		/// <summary>
		/// Обновление существующего поста новости.
		/// </summary>
		/// <param name="newsPost">Обновленные данные поста новости.</param>
		void UpdateNewsPostAsync(NewsPost newsPost);

		/// <summary>
		/// Удаление поста новости по идентификатору.
		/// </summary>
		/// <param name="id">Идентификатор поста новости.</param>
		Task DeleteNewsPostAsync(int id);
	}
}
