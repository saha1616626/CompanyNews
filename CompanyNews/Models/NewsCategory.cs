using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompanyNews.Models
{
    /// <summary>
    /// Категория новости в сообществе
    /// </summary>
    public class NewsCategory : INotifyPropertyChanged
	{
        private int _id { get; set; }
        public int id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(id)); }
        }

		[Required(ErrorMessage = "Название обязательно для заполнения!")]
		private string _name { get; set; }
        public string name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(name)); }
        }
        private string? _description { get; set; }
		public string description
		{
			get { return _description; }
			set { _description = value; OnPropertyChanged(nameof(description)); }
		}
		/// <summary>
		/// Категория в архиве?
		/// </summary>
		private bool _isArchived { get; set; }
		public bool isArchived
		{
			get { return _isArchived; }
			set { _isArchived = value; OnPropertyChanged(nameof(isArchived)); }
		}

		/// <summary>
		/// Навигационное свойство для связи с категориями доступными пользователям
		/// </summary>
		public ICollection<NewsCategoriesWorkDepartment> newsCategoriesWorkDepartments { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с постами
        /// </summary>
        public ICollection<NewsPost> newsPosts { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
