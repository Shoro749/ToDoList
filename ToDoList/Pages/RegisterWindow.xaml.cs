using Data.models;
using Services.services;
using Services.Services;
using System.Text.RegularExpressions;
using System.Windows;

namespace ToDoList.Pages
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly UserService _userService;
        public RegisterWindow(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            string username = tb_username.Text.Trim();
            string password = tb_password.Password.Trim();
            string email = tb_email.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_userService.IsTakenName(username))
            {
                MessageBox.Show("Username is already taken.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = new User
            {
                Username = username,
                PasswordHash = PasswordHasher.HashPassword(password),
            };

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (Regex.IsMatch(email, emailPattern) && !string.IsNullOrWhiteSpace(email)) user.Email = email;

            _userService.Add(user);
            DialogResult = true;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
