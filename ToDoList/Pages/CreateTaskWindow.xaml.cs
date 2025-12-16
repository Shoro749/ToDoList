using Data.models;
using System.Windows;
using System.Windows.Controls;

namespace ToDoList.Pages
{
    /// <summary>
    /// Interaction logic for CreateTaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public Tasks newTask;
        private readonly Lists _list;
        public TaskWindow(Lists list)
        {
            InitializeComponent();
            Title = "Add New Task";
            _list = list;
            dpDate.SelectedDate = DateTime.Now.Date;
            cbStatus.SelectedIndex = 0;
            newTask = new Tasks();
        }

        public TaskWindow(Tasks existingTask, Lists list)
        {
            InitializeComponent();
            Title = "Edit Task";

            _list = list;

            tbDescription.Text = existingTask.Desc;
            dpDate.SelectedDate = existingTask.Date;

            string status = existingTask.Status;
            for (int i = 0; i < cbStatus.Items.Count; i++)
            {
                if (((ComboBoxItem)cbStatus.Items[i]).Content.ToString() == status)
                {
                    cbStatus.SelectedIndex = i;
                    break;
                }
            }

            newTask = existingTask;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbDescription.Text))
            {
                MessageBox.Show("The task description cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            newTask.Desc = tbDescription.Text.Trim();
            newTask.Status = ((ComboBoxItem)cbStatus.SelectedItem).Content.ToString();
            newTask.Date = dpDate.SelectedDate ?? DateTime.Now.Date;
            newTask.List = _list;

            DialogResult = true;
        }
    }
}
