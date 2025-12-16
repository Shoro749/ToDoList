using Data.context;
using Data.models;
using Services.Interfaces;
using Services.Services;
using System.Windows;
using System.Windows.Controls;
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
        public GeneralWindow(DataContext context, User user)
        {
            InitializeComponent();
            _listService = new Service<Lists>(context);
            _user = user;
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

            TextBlock itemText = new TextBlock
            {
                Text = list.Name,
                Foreground = (Brush)new BrushConverter().ConvertFromString("#AAAAAA"),
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };
            Grid.SetColumn(itemText, 0);
            itemGrid.Children.Add(itemText);

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
            ContextMenu contextMenu = (ContextMenu)menuItem.Parent;
            Button sourceButton = (Button)contextMenu.PlacementTarget;

            Lists itemToUpdate = sourceButton.Tag as Lists;
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
    }
}
