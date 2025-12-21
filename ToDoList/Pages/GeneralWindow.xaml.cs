using Data.context;
using Data.models;
using Repositories.Repositories;
using Services.Interfaces;
using Services.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ToDoList.Pages
{
    /// <summary>
    /// Interaction logic for GeneralWindow.xaml
    /// </summary>
    public partial class GeneralWindow : Window
    {
        private readonly User _user;
        private readonly IService<Lists> _listService;
        private readonly IService<User> _userService;
        private readonly IService<Tasks> _tasksService;
        private readonly ITaskService _taskService;
        private Lists _currentList;
        public GeneralWindow(DataContext context, User user)
        {
            InitializeComponent();
            _listService = new Service<Lists>(context);
            _userService = new Service<User>(context);
            _taskService = new TaskService(context);
            _tasksService = new TaskService(context);

            _user = user;
            
            UpdateLists();
        }

        private void UpdateLists()
        {
            var lists = _listService.GetAll();
            foreach (var list in lists)
            {
                AddDynamicMenuItem(list);
            }
        }

        private void AddDynamicMenuItem(Lists list)
        {
            Grid itemGrid = new Grid { Margin = new Thickness(0, 5, 0, 0) };

            itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });

            Button itemButton = new Button
            {
                Name = $"btn_{list.Id}",
                Content = list.Name,
                Tag = list,

                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(0),

                Foreground = (Brush)new BrushConverter().ConvertFromString("#AAAAAA"),
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };

            itemButton.Click += ListItemSelectedClick;

            Style buttonStyle = new Style(typeof(Button));
            buttonStyle.Setters.Add(new Setter(Button.CursorProperty, Cursors.Hand));
    
            Trigger mouseOverTrigger = new Trigger { Property = IsMouseOverProperty, Value = true };
            mouseOverTrigger.Setters.Add(new Setter(Button.BackgroundProperty, (Brush)new BrushConverter().ConvertFromString("#444444")));
            buttonStyle.Triggers.Add(mouseOverTrigger);
    
            itemButton.Style = buttonStyle;


            Grid.SetColumn(itemButton, 0);
            itemGrid.Children.Add(itemButton);

            ContextMenu contextMenu = new ContextMenu();

            MenuItem editItem = new MenuItem { Header = "Edit" };
            editItem.Click += ContextMenuEditClick;
            contextMenu.Items.Add(editItem);

            MenuItem deleteItem = new MenuItem { Header = "Delete" };
            deleteItem.Click += ContextMenuDeleteClick;
            contextMenu.Items.Add(deleteItem);

            contextMenu.Items.Add(new Separator());

            Button optionsButton = new Button
            {
                Content = "...",
                FontWeight = FontWeights.Bold,
                Background = Brushes.Transparent,
                Foreground = (Brush)new BrushConverter().ConvertFromString("#AAAAAA"),
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Height = 25,
                Width = 25,
                Padding = new Thickness(0),
                Tag = list,

                ContextMenu = contextMenu
            };

            optionsButton.Click += OptionsButtonClick;

            Grid.SetColumn(optionsButton, 1);
            itemGrid.Children.Add(optionsButton);

            Separator separator = new Separator
            {
                Background = (Brush)new BrushConverter().ConvertFromString("#444444"),
                Margin = new Thickness(0, 5, 0, 10)
            };

            sp_lists.Children.Add(itemGrid);
            sp_lists.Children.Add(separator);
        }

        private void OptionsButtonClick(object sender, RoutedEventArgs e)
        {
            Button sourceButton = (Button)sender;

            if (sourceButton.ContextMenu != null)
            {
                sourceButton.ContextMenu.PlacementTarget = sourceButton;
                sourceButton.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                sourceButton.ContextMenu.IsOpen = true;
            }
        }

        private void ContextMenuEditClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;

            ContextMenu contextMenu = menuItem.Parent as ContextMenu;
            if (contextMenu == null) return;

            Button optionsButton = contextMenu.PlacementTarget as Button;
            if (optionsButton == null) return;

            Lists itemToUpdate = optionsButton.Tag as Lists;
            if (itemToUpdate == null) return;

            UpdateListWindow window = new UpdateListWindow(itemToUpdate.Name);
            bool? result = window.ShowDialog();

            if (result == true)
            {
                string newName = window.listName;
                if (!string.IsNullOrWhiteSpace(newName) && newName != itemToUpdate.Name)
                {
                    itemToUpdate.Name = newName;
                    _listService.Update(itemToUpdate.Id, itemToUpdate);

                    foreach (var child in sp_lists.Children)
                    {
                        if (child is Grid grid)
                        {
                            foreach (var el in grid.Children)
                            {
                                if (el is Button btn && btn.Tag == itemToUpdate)
                                {
                                    btn.Content = newName;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ContextMenuDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem menuItem = (MenuItem)sender;
                ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
                Button sourceButton = (Button)contextMenu.PlacementTarget;

                Lists itemToDelete = sourceButton.Tag as Lists;

                if (itemToDelete == null) return;

                _listService.Delete(itemToDelete.Id);
                dg_tasks.Visibility = Visibility.Hidden;
                _currentList = null;

                Grid itemGrid = sourceButton.Parent as Grid;

                if (itemGrid != null)
                {
                    StackPanel parentStackPanel = itemGrid.Parent as StackPanel;

                    if (parentStackPanel != null)
                    {
                        int gridIndex = parentStackPanel.Children.IndexOf(itemGrid);
                        parentStackPanel.Children.RemoveAt(gridIndex);

                        if (gridIndex < parentStackPanel.Children.Count)
                        {
                            UIElement nextElement = parentStackPanel.Children[gridIndex];
                            if (nextElement is Separator)
                            {
                                parentStackPanel.Children.RemoveAt(gridIndex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void ListItemSelectedClick(object sender, RoutedEventArgs e)
        {
            Button selectedButton = sender as Button;

            if (selectedButton != null)
            {
                Lists selectedList = selectedButton.Tag as Lists;

                if (selectedList != null)
                {
                    _currentList = selectedList;
                    LoadListTasks(selectedList.Id);
                    dg_tasks.Visibility = Visibility.Visible;
                }
            }
        }

        private void LoadListTasks(int id)
        {
            try { dg_tasks.ItemsSource = _taskService.GetByListId(id); }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void CreateNewList(object sender, RoutedEventArgs e)
        {
            try
            {
                var list = new Lists
                {
                    User = _user,
                    Name = "New list"
                };
                _listService.Add(list);
                UpdateLists();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }

        private void DeleteUserClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _userService.Delete(_user.Id);

                MainWindow mainWindow = new MainWindow();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
                this.Close();
            }
        }

        private void AddTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentList == null)
                {
                    MessageBox.Show("Select a list.");
                    return;
                }

                TaskWindow window = new TaskWindow(_currentList);
                bool? result = window.ShowDialog();

                if (result == true)
                {
                    var task = window.newTask;
                    _tasksService.Add(task);
                    LoadListTasks(task.List.Id);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void UpdateTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var selected = dg_tasks.SelectedItem as Tasks;

                if (selected == null)
                {
                    MessageBox.Show("Select a task to edit.");
                    return;
                }

                TaskWindow window = new TaskWindow(selected, _currentList);
                bool? result = window.ShowDialog();

                if (result == true)
                {
                    var task = window.newTask;
                    _tasksService.Update(task.Id, task);
                    LoadListTasks(task.List.Id);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void DeleteTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var selected = dg_tasks.SelectedItem as Tasks;

                if (selected == null)
                {
                    MessageBox.Show("Select a task to delete.");
                    return;
                }

                _tasksService.Delete(selected.Id);
                LoadListTasks(selected.List.Id);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFilterStatus == null || _currentList == null) return;

            var selectedItem = cbFilterStatus.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;

            string statusText = selectedItem.Content.ToString();

            if (cbFilterStatus.SelectedIndex == 0) LoadListTasks(_currentList.Id);
            else dg_tasks.ItemsSource = _taskService.GetByStatus(_currentList.Id, statusText);
        }
    }
}
