using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Check.Models;
using Check.ViewModels;
using Check.Views;

namespace Check
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            PositionView positionView = new PositionView(new PositionViewModel(new Position())) { Margin = new Thickness(10d) };

            Viewbox viewBox = new Viewbox { Stretch = Stretch.Uniform, Child = positionView };

            Content = viewBox;
        }
    }
}
