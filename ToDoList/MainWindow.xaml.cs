using Data.context;
using Data.models;
using Services.services;
using Services.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToDoList.Pages;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DataContext _context;
        private readonly UserService _userService;
        public MainWindow()
        {
            InitializeComponent();
            _context = new DataContext();
            _userService = new UserService(_context);
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = tb_username.Text.Trim();
                string password = tb_password.Password.Trim();

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please fill in all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var user = _userService.GetByName(login);

                if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                {
                    MessageBox.Show("User not found or password incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                GeneralWindow generalWindow = new GeneralWindow(_context, user);
                Application.Current.MainWindow = generalWindow;
                generalWindow.Show();
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            RegisterWindow window = new RegisterWindow(_userService);
            window.ShowDialog();
        }
    }
}