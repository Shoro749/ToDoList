using Data.context;
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
                    MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //if (_userService.IsTakenName(login))
                //{
                //    MessageBox.Show("Ім'я користувача уже зайняте.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}

                var user = _userService

                MainWindow mainWindow =  = new MainWindow(context);
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
