using System.Windows;
using System.Windows.Controls;

namespace Check.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            Grid grid = new Grid();

            grid.   RowDefinitions.Add(new    RowDefinition());
            grid.   RowDefinitions.Add(new    RowDefinition());
            grid.   RowDefinitions.Add(new    RowDefinition());
            grid.   RowDefinitions.Add(new    RowDefinition());
            grid.   RowDefinitions.Add(new    RowDefinition());
            grid.   RowDefinitions.Add(new    RowDefinition());
            grid.   RowDefinitions.Add(new    RowDefinition());
            grid.   RowDefinitions.Add(new    RowDefinition());
            grid.   RowDefinitions.Add(new    RowDefinition());
            grid.   RowDefinitions.Add(new    RowDefinition());

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            TextBlock textBlock = new TextBlock { Text    = "Settings", FontSize = 18d, FontWeight = FontWeights.Bold,               Margin = new Thickness(4d) } ; Grid.SetRow(textBlock, 0); Grid.SetColumn(textBlock, 0); grid.Children.Add(textBlock);
            Button    button    = new Button    { Content = "Undo move <Ctrl+Z>"                                     , Height = 20d, Margin = new Thickness(4d) } ; Grid.SetRow(button   , 1); Grid.SetColumn(button   , 1); grid.Children.Add(button   );
                      button    = new Button    { Content = "Redo move <Ctrl+Y>"                                     , Height = 20d, Margin = new Thickness(4d) } ; Grid.SetRow(button   , 2); Grid.SetColumn(button   , 1); grid.Children.Add(button   );

            Content = grid;
        }
    }
}
