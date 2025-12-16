using Data.context;
using Services.services;
using Services.Services;
using System.Windows;

namespace ToDoList.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly DataContext _context;
        private readonly UserService _userService;
        public LoginWindow()
        {
            InitializeComponent();

            _context = new DataContext();
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

                MainWindow mainWindow = new MainWindow(_context, user);
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
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
