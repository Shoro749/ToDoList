using Data.context;
using Data.models;
using System.Windows;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(DataContext context, User user)
        {
            InitializeComponent();
        }
    }
}