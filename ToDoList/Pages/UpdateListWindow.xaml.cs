using System.Windows;

namespace ToDoList.Pages
{
    /// <summary>
    /// Interaction logic for UpdateListWindow.xaml
    /// </summary>
    public partial class UpdateListWindow : Window
    {
        public string listName;
        public UpdateListWindow(string name)
        {
            InitializeComponent();
            tbNewName.Text = name;
        }

        private void ChangeButtonClick(object sender, RoutedEventArgs e)
        {
            listName = tbNewName.Text.Trim();

            if (string.IsNullOrWhiteSpace(listName))
            {
                MessageBox.Show("Please fill the new list name", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
