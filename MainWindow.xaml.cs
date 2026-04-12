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

            Viewbox viewBox = new Viewbox { Stretch = Stretch.Uniform, Child = positionView };

            Content = viewBox;
        }

        #endregion

        #region Event handlers

        protected override void OnKeyDown(KeyEventArgs ea)
        {
            PositionView.CheckForControlKeys(ea);
        }

        #endregion

        #region Private properties

        private PositionView PositionView { get; }

        #endregion
    }
}
