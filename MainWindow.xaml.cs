using Check.Models;
using Check.ViewModels;
using Check.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Check
{
    public partial class MainWindow
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            PositionView positionView = PositionView = new PositionView(new PositionViewModel(new Position())) { Margin = new Thickness(10d) };

            Viewbox      viewBox      =                new Viewbox { Stretch = Stretch.Uniform, Child = positionView };

            SettingsView settingsView =                new SettingsView(); Grid.SetColumn(settingsView, 1);

            Grid         grid         =                new Grid();

            grid.   RowDefinitions.Add(new    RowDefinition());

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            grid.Children.Add(viewBox     );
            grid.Children.Add(settingsView);

            Content = grid;
        }

        #endregion

        #region Event handlers

        // ReSharper disable once AsyncVoidEventHandlerMethod
        protected override async void OnKeyDown(KeyEventArgs ea)
        {
            await PositionView.CheckForControlKeys(ea);
        }

        #endregion

        #region Private properties

        private PositionView PositionView { get; }

        #endregion
    }
}
