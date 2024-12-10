using CompanyNews.Models.Extended;
using CompanyNews.Services;
using CompanyNews.ViewModels.AdminApp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.CompilerServices;
using CompanyNews.Helpers;

namespace CompanyNews.ViewModels.Authorization
{
    public class AuthorizationViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Сервис для взаиодействия с бизнес-логикой
        /// </summary>
        private readonly AuthorizationService _authorizationService;

        public AuthorizationViewModel()
        {
            _authorizationService = ServiceLocator.GetService<AuthorizationService>();
            DisplayingNumberUsers(); // Выводим кол-во пользователей на экран
        }

        #region BasicOperations

        /// <summary>
        /// Выводим кол-во пользователей сервиса
        /// </summary>
        private async Task DisplayingNumberUsers()
        {
            NumberUsers = await _authorizationService.NumberUsers();
        }

        /// <summary>
        /// Вход в аккаунт
        /// </summary>
        public async Task<bool> LogInYourAccount(string login, string password)
        {
            return await _authorizationService.LogInYourAccount(login, password);
        }

        #endregion

        #region UI RelayCommand Operations

        /// <summary>
        /// Кнопка "вход" в UI
        /// </summary>
        private RelayCommand _entrance { get; set; }
        public RelayCommand Entrance
        {
            get
            {
                return _entrance ??
                    (_entrance = new RelayCommand(async (obj) =>
                    {

                    }, (obj) => true));
            }
        }

        #endregion

        #region Features

        /// <summary>
        /// Асинхронно получаем информацию из привязанного View
        /// </summary>
        public async Task InitializeAsync(AdminViewModelParameters adminViewModelParameters)
        {
            darkBackground = adminViewModelParameters.darkBackground;
            fieldIllumination = adminViewModelParameters.fieldIllumination;
            errorInput = adminViewModelParameters.errorInput;
        }

        #region View

        /// <summary>
        /// Кол-во пользователей сервиса
        /// </summary>
        private int _numberUsers { get; set; }
        public int NumberUsers
        {
            get { return _numberUsers; }
            set { _numberUsers = value; OnPropertyChanged(nameof(NumberUsers)); }
        }

        /// <summary>
        /// Затемненный фон позади Popup
        /// </summary>
        public Border? darkBackground { get; set; }

        /// <summary>
        /// Анимация полей
        /// </summary>
        public Storyboard? fieldIllumination { get; set; }

        /// <summary>
        /// Вывод ошибки и анимация текста на странице
        /// </summary>
        public TextBlock? errorInput { get; set; }

        #endregion

        #endregion

        #region Animation

        // выводим сообщения об ошибке с анимацией затухания
        private async void BeginFadeAnimation(TextBlock textBlock)
        {
            textBlock.IsEnabled = true;
            textBlock.Opacity = 1.0;

            Storyboard storyboard = new Storyboard();
            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(2),
            };
            Storyboard.SetTargetProperty(fadeAnimation, new System.Windows.PropertyPath(System.Windows.UIElement.OpacityProperty));
            storyboard.Children.Add(fadeAnimation);
            storyboard.Completed += (s, e) => textBlock.IsEnabled = false;
            storyboard.Begin(textBlock);
        }

        // запускаем анимации для TextBox (подсвечивание объекта)
        private void StartFieldIllumination(TextBox textBox)
        {
            fieldIllumination.Begin(textBox);
        }
        private void StartFieldIllumination(PasswordBox passwordBox)
        {
            fieldIllumination.Begin(passwordBox);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
